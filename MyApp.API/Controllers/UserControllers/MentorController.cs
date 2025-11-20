using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Mentor;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MentorController : ControllerBase
{
    private readonly IMentorService _mentorService;
    private readonly IMapper _mapper;

    public MentorController(IMentorService mentorService, IMapper mapper)
    {
        _mentorService = mentorService;
        _mapper = mapper;
    }
    [HttpPost("search")]
    public async Task<IActionResult> SearchMentors([FromBody] SearchMentorDto filterDto)
    {
        var mentors = await _mentorService.SearchMentorsAsync(filterDto);
        var mentorDtos = _mapper.Map<IEnumerable<MentorDto>>(mentors);
        return Ok(ApiResponse<IEnumerable<MentorDto>>.SuccessResponse(mentorDtos, StatusCodes.Status200OK, "Mentors fetched"));
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

    [Authorize(Roles = "Mentor")]
    [HttpGet("availabilities")]
    public async Task<IActionResult> GetMyAvailabilities()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var availabilities = await _mentorService.GetAvailabilitiesAsync(userId);
        return Ok(ApiResponse<List<MentorAvailabilityDto>>.SuccessResponse(availabilities, 200, "Availabilities fetched"));
    }
    [Authorize(Roles = "Mentor")]
    [HttpPost("add")]
    public async Task<IActionResult> AddAvailabilities([FromBody] List<MentorAvailabilityDto> availabilities)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var success = await _mentorService.AddAvailabilitiesAsync(userId, availabilities);
        if (!success)
            return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to add availabilities"));
        return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Availabilities added"));
    }
    [Authorize(Roles = "Mentor")]
    [HttpPut("update/{availabilityId}")]
    public async Task<IActionResult> UpdateAvailability(int availabilityId, [FromBody] MentorAvailabilityDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var success = await _mentorService.UpdateAvailabilityAsync(userId, availabilityId, dto);
        if (!success)
            return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to update availability"));
        return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Availability updated"));
    }
    [Authorize(Roles = "Mentor")]
    [HttpDelete("delete/{availabilityId}")]
    public async Task<IActionResult> DeleteAvailability(int availabilityId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var success = await _mentorService.DeleteAvailabilityAsync(userId, availabilityId);
        if (!success)
            return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to delete availability"));
        return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Availability deleted"));
    }

}
