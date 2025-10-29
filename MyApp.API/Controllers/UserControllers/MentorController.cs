using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MentorController : ControllerBase
{
    private readonly IMentorService _mentorService;

    public MentorController(IMentorService mentorService)
    {
        _mentorService = mentorService;
    }

    [HttpPost("apply-mentor")]
    public async Task<IActionResult> ApplyMentor([FromBody] MentorApplicationDto mentorDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var success = await _mentorService.ApplyMentorAsync(userId, mentorDto);

        if (!success)
            return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Mentor application failed or already applied"));

        return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Mentor application submitted successfully"));
    }

    [HttpGet("mentor-status")]
    public async Task<IActionResult> GetMentorStatus()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var status = await _mentorService.GetMentorApplicationStatusAsync(userId);

        if (string.IsNullOrEmpty(status))
            return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Mentor profile not found"));

        return Ok(ApiResponse<string>.SuccessResponse(status, StatusCodes.Status200OK, "Mentor status fetched successfully"));
    }
}
