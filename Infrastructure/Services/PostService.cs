using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Post;
using MyApp1.Application.Interfaces.Services;
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
        public PostService(
            IGenericRepository<Post> postRepository,
            IGenericRepository<PostComment> postCommentRepository,
            IGenericRepository<PostLike> postLikeRepository)
        {
            _postRepository = postRepository;
            _postCommentRepository = postCommentRepository;
            _postLikeRepository = postLikeRepository;
        }
        public async Task<IEnumerable<Post>> GetPostsAsync(int page, int pageSize)
        {
            return await _postRepository.Table
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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
            return true;
        }
        public async Task<IEnumerable<PostComment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _postCommentRepository.Table
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
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
            return true;
        }

        public async Task<bool> ToggleLikePostAsync(int postId, int userId)
        {
            var existingLike = await _postLikeRepository.Table
                .FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);

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
