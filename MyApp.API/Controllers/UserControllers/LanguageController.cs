using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Language;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IGenericService<Language> _genericLanguageService;
        private readonly IUserLanguageService _userLanguageService;
        private readonly IMapper _mapper;

        public LanguageController(IGenericService<Language> genericLanguageService, IUserLanguageService userLanguageService, IMapper mapper)
        {
            _genericLanguageService = genericLanguageService;
            _userLanguageService = userLanguageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguages()
        {
            var languages = await _genericLanguageService.GetAllAsync();
            var languageDtos = _mapper.Map<IEnumerable<LanguageResponseDto>>(languages);
            return Ok(ApiResponse<IEnumerable<LanguageResponseDto>>.SuccessResponse(languageDtos, StatusCodes.Status200OK, "Languages fetched successfully"));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLanguages(int userId)
        {
            var userLanguages = await _userLanguageService.GetUserLanguagesAsync(userId);
            var userLanguageDtos = _mapper.Map<IEnumerable<UserLanguageResponseDto>>(userLanguages);
            return Ok(ApiResponse<IEnumerable<UserLanguageResponseDto>>.SuccessResponse(userLanguageDtos, StatusCodes.Status200OK, "User languages fetched"));
        }

        [HttpPost("user")]
        public async Task<IActionResult> AddUserLanguage([FromBody] AddUserLanguageDto addDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _userLanguageService.AddUserLanguageAsync(userId, addDto);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to add language"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Language added successfully"));
        }

        [HttpDelete("user/{languageId}")]
        public async Task<IActionResult> RemoveUserLanguage( int languageId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _userLanguageService.RemoveUserLanguageAsync(userId, languageId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to remove language"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Language removed successfully"));
        }
    }
}
