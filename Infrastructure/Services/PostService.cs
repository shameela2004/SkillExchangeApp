using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Post;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Application.Utils;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Services
{
    public class PostService :IPostService
    {
        private readonly IGenericRepository<Post> _postRepository;
        private readonly IGenericRepository<PostComment> _postCommentRepository;
        private readonly IGenericRepository<PostLike> _postLikeRepository;
        private readonly IGenericService<Notification> _notificationService;
        private readonly IGenericRepository<User> _userRepository;
        private readonly SocialTextParser _textParser;
        public PostService(
            IGenericRepository<Post> postRepository,
            IGenericRepository<PostComment> postCommentRepository,
            IGenericRepository<PostLike> postLikeRepository,
            IGenericRepository<User> userRepository,
            IGenericService<Notification> notificationService,
            SocialTextParser textParser
            )
        {
            _postRepository = postRepository;
            _postCommentRepository = postCommentRepository;
            _postLikeRepository = postLikeRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _textParser = textParser;
        }
        public async Task<IEnumerable<PostDto>> GetPostsAsync(int page, int pageSize, string sortBy, bool descending, int userId)
        {
            IQueryable<Post> query = _postRepository.Table.Include(p => p.User);

            // Sorting
            query = (sortBy.ToLower(), descending) switch
            {
                ("createdat", true) => query.OrderByDescending(p => p.CreatedAt),
                ("createdat", false) => query.OrderBy(p => p.CreatedAt),
                ("likecount", true) => query.OrderByDescending(p => p.LikeCount),
                ("likecount", false) => query.OrderBy(p => p.LikeCount),
                ("commentcount", true) => query.OrderByDescending(p => p.CommentCount),
                ("commentcount", false) => query.OrderBy(p => p.CommentCount),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            // Get liked posts for this user
            var likedPostIds = await _postLikeRepository.Table
                 .Where(pl => pl.UserId == userId && !pl.IsDeleted)
                 .Select(pl => pl.PostId)
                 .ToListAsync();

            var posts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Project to DTO with HasLiked
            var postDtos = posts.Select(post => new PostDto
            {
                PostId = post.Id,
                UserId = post.UserId,
                UserName = post.User.Name,
                Content = post.Content,
                MediaUrl = post.MediaUrl,
                LikeCount = post.LikeCount,
                CommentCount = post.CommentCount,
                CreatedAt = post.CreatedAt,
                HasLiked = likedPostIds.Contains(post.Id) // <-- here is the magic!
            })
            .ToList();

            return postDtos;
        }


        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            return await _postRepository.GetByIdAsync(postId);
        }
        public async Task <PostDto?> GetPostDtoByIdAsync(int postId)
        {
            var post = await _postRepository.Table
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null) return null;
            var postDto = new PostDto
            {
                PostId = post.Id,
                UserId = post.UserId,
                UserName = post.User.Name,
                Content = post.Content,
                MediaUrl = post.MediaUrl,
                CreatedAt = post.CreatedAt,
                LikeCount = post.LikeCount,
                CommentCount = post.CommentCount
            };
            return postDto;
        }
        public async Task<bool> CreatePostAsync(int userId, CreatePostDto createDto)
        {
            var post = new Post
            {
                UserId = userId,
                Content = createDto.Content,
                MediaUrl = createDto.MediaUrl,
                CreatedAt = DateTime.UtcNow,
                LikeCount = 0,
                CommentCount = 0
            };
            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();
            // Parse mentions and notify users
            var (mentionedUsernames, _) = _textParser.ParseMentionsAndHashtags(createDto.Content);
            foreach (var username in mentionedUsernames)
            {
                var user = await _userRepository.Table.FirstOrDefaultAsync(u => u.Name == username);
                if (user != null)
                {
                    await _notificationService.AddAsync(new Notification
                    {
                        UserId = user.Id,
                        Title = "You were mentioned",
                        Message = $"You were mentioned by {post.UserId} in a post.",
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
            return true;
        }
        public async Task<bool> EditPostAsync(int postId, int userId, EditPostDto editDto)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null || post.UserId != userId)
                return false;

            post.Content = editDto.Content;
            post.MediaUrl = editDto.MediaUrl;

            await _postRepository.UpdateAsync(post);
            return true;
        }
        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null) return false;

            // Optional: Handle cascading deletion of comments, likes here if not configured in DB

            _postRepository.Remove(post);
            await _postRepository.SaveChangesAsync();
            return true;
        }

        //Post Commments
        public async Task<IEnumerable<PostComment>> GetCommentsByPostIdAsync(int postId, int page, int pageSize, string sortBy, bool descending)
        {
            IQueryable<PostComment> query = _postCommentRepository.Table.Include(c => c.User).Where(c => c.PostId == postId && !c.IsDeleted);

            query = (sortBy.ToLower(), descending) switch
            {
                ("createdat", true) => query.OrderByDescending(c => c.CreatedAt),
                ("createdat", false) => query.OrderBy(c => c.CreatedAt),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }



        public async Task<bool> AddCommentAsync(int postId, int userId, string commentText)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null) return false;

            var comment = new PostComment
            {
                PostId = postId,
                UserId = userId,
                Comment = commentText
            };
            await _postCommentRepository.AddAsync(comment);
            post.CommentCount += 1;
            await _postRepository.UpdateAsync(post);
            // Parse mentions in comment
            var (mentionedUsernames, _) = _textParser.ParseMentionsAndHashtags(commentText);

            // Notify mentioned users
            foreach (var username in mentionedUsernames)
            {
                var mentionedUser = await _userRepository.Table.FirstOrDefaultAsync(u => u.Name == username);
                if (mentionedUser != null)
                {
                    await _notificationService.AddAsync(new Notification
                    {
                        UserId = mentionedUser.Id,
                        Title = "You were mentioned in a comment",
                        Message = $"You were mentioned by {userId} in a comment.",
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
            return true;
        }
        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var comment=await _postCommentRepository.GetByIdAsync(commentId);
            if(comment == null) return false;
            var post = await _postRepository.GetByIdAsync(comment.PostId);
            post.CommentCount -= 1;
            await _postRepository.UpdateAsync(post);
            _postCommentRepository.Remove(comment);
            await _postCommentRepository.SaveChangesAsync();
          
            return true;
        }

        // Post Likes
        public async Task<bool> ToggleLikePostAsync(int postId, int userId)
        {
            var existingLike = await _postLikeRepository.Table
                .FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId && !pl.IsDeleted);

            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null) return false;

            if (existingLike != null)
            {
                // User already liked, so remove like (unlike)
                _postLikeRepository.Remove(existingLike);
                post.LikeCount = Math.Max(0, post.LikeCount - 1);
            }
            else
            {
                // Add new like
                var like = new PostLike
                {
                    PostId = postId,
                    UserId = userId,
                    LikedAt = DateTime.UtcNow
                };
                await _postLikeRepository.AddAsync(like);
                post.LikeCount += 1;
            }

            await _postRepository.UpdateAsync(post);
            return true;
        }


    }
}
