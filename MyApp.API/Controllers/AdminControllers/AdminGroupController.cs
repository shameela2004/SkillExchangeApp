using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Group;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IGenericService<Group> _genericGroupService;
        private readonly IMapper _mapper;
        public AdminGroupController(IGroupService groupService, IGenericService<Group> genericGroupService, IMapper mapper)
        {
            _groupService = groupService;
            _genericGroupService = genericGroupService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _genericGroupService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<GroupDto>>(groups);
            return Ok(ApiResponse<IEnumerable<GroupDto>>.SuccessResponse(dtos, StatusCodes.Status200OK, "All groups fetched"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var success = await _groupService.DeleteGroupAsync(id, userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Delete failed"));

            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully", StatusCodes.Status200OK, "Group deleted"));
        }
    }
}
