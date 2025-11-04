using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Group;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto dto)
        {
            var mentorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var id = await _groupService.CreateGroupAsync(dto, mentorId);
            return Ok(ApiResponse<int>.SuccessResponse(id, StatusCodes.Status201Created, "Group created"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] CreateGroupDto dto)
        {
            var mentorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _groupService.UpdateGroupAsync(id, dto, mentorId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Update failed"));
            return Ok(ApiResponse<string>.SuccessResponse("Updated successfully", StatusCodes.Status200OK, "Group updated"));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Group not found"));

            var dto = _mapper.Map<GroupDto>(group);
            return Ok(ApiResponse<GroupDto>.SuccessResponse(dto, StatusCodes.Status200OK, "Group details"));
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

        [HttpPost("{groupId}/members")]
        [Authorize(Roles = "Mentor,Admin")]
        public async Task<IActionResult> AddMember(int groupId, [FromBody] AddGroupMemberDto memberDto)
        {
            var requesterUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var success = await _groupService.AddMemberAsync(groupId, memberDto.UserId, requesterUserId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Add member failed"));

            return Ok(ApiResponse<string>.SuccessResponse("Member added", StatusCodes.Status200OK, "Member added"));
        }

        [HttpDelete("{groupId}/members/{userId}")]
        [Authorize(Roles = "Mentor,Admin")]
        public async Task<IActionResult> RemoveMember(int groupId, int userId)
        {
            var requesterUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var success = await _groupService.RemoveMemberAsync(groupId, userId, requesterUserId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Remove member failed"));

            return Ok(ApiResponse<string>.SuccessResponse("Member removed", StatusCodes.Status200OK, "Member removed"));
        }

        [HttpGet("{id}/members")]
        public async Task<IActionResult> GetMembers(int id)
        {
            var members = await _groupService.GetGroupMembersAsync(id);
            return Ok(ApiResponse<IEnumerable<GroupMemberDto>>.SuccessResponse(members, StatusCodes.Status200OK, "Members fetched"));
        }


        [HttpPost]
        [Route("{groupId}/messages")]
        [Authorize]
        public async Task<IActionResult> SendMessage(int groupId, [FromBody] SendGroupMessageDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var messageId = await _groupService.SendMessageAsync(groupId, userId, dto);
            return Ok(ApiResponse<int>.SuccessResponse(messageId, StatusCodes.Status201Created, "Message sent"));
        }




        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetMessages(int id)
        {
            var messages = await _groupService.GetMessagesAsync(id);
            var messageDtos = _mapper.Map<IEnumerable<GroupMessageDto>>(messages);
            return Ok(ApiResponse<IEnumerable<GroupMessageDto>>.SuccessResponse(messageDtos, StatusCodes.Status200OK, "Messages fetched"));
        }
    }
}
