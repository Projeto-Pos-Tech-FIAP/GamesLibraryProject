using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Domain.DTOs.AuthDto;

namespace TechChallengeFase1.Api.Controllers
{
    /// <summary>
    /// Handles authentication operations using Keycloak
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IKeycloakService _keycloakService;

        public AuthController(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService ?? throw new ArgumentNullException(nameof(keycloakService));
        }

        /// <summary>
        /// Authenticates a user and returns an access token
        /// </summary>
        /// <param name="loginRequest">User credentials (username and password)</param>
        /// <returns>JWT access token</returns>
        /// <response code="200">Returns the authentication token</response>
        /// <response code="401">Invalid username or password</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromForm] LoginInputDto loginRequest)
        {
            var token = await _keycloakService.GetTokenAsync(loginRequest.Username, loginRequest.Password);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(token);
        }

        /// <summary>
        /// Refreshes an expired access token using a refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>New access token</returns>
        /// <response code="200">Returns a new token</response>
        /// <response code="400">Invalid refresh token</response>
        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("Refresh token must be provided.");

            var token = await _keycloakService.RefreshTokenAsync(refreshToken);

            return Ok(token);
        }
    }
}