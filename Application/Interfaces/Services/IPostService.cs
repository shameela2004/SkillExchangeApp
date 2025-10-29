using MyApp1.Application.DTOs.Post;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetPostsAsync(int page, int pageSize);
        Task<bool> CreatePostAsync(int userId, CreatePostDto createDto);
        Task<IEnumerable<PostComment>> GetCommentsByPostIdAsync(int postId);
        Task<bool> AddCommentAsync(int postId, int userId, string commentText);
        Task<bool> ToggleLikePostAsync(int postId, int userId);

    }
}
