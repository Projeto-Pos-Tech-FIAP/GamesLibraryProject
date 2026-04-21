using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFase1.Application.DTOs.UsersDto;
using TechChallengeFase1.Application.Interfaces;

namespace TechChallengeFase1.Api.Controllers
{
    /// <summary>
    /// Manages users integrated with Keycloak
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices _usersService;

        public UsersController(IUsersServices usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        /// <summary>
        /// Retrieves all users from Keycloak
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns the list of users</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _usersService.GetAllUsersAsync());
        }

        /// <summary>
        /// Creates a new user in Keycloak
        /// </summary>
        /// <remarks>
        /// Example request:
        ///
        ///     POST /api/users
        ///     {
        ///         "username": "john.doe",
        ///         "password": "StrongPass1!",
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "email": "john.doe@email.com",
        ///         "level": "User",
        ///         "dateOfBirth": "1995-05-10",
        ///         "gender": "Male"
        ///     }
        ///
        /// </remarks>
        /// <param name="command">User data for creation</param>
        /// <response code="204">User created successfully</response>
        /// <response code="400">Validation error</response>
        /// <response code="409">User already exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserInputDto command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            await _usersService.CreateUserAsync(command);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing user in Keycloak
        /// </summary>
        /// <remarks>
        /// Only the provided fields will be updated (partial update behavior).
        ///
        /// Example request:
        ///
        ///     PUT /api/users
        ///     {
        ///  
        ///         "firstName": "John",
        ///         "lastName": "Updated",
        ///         "gender": "Male",
        ///         "dateOfBirth": "1995-05-10"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">User updated successfully</response>
        /// <response code="400">Validation error</response>
        /// <response code="404">User not found</response>
        [HttpPut("{userEmail}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] string userEmail,[FromBody] EditUserDto command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            await _usersService.UpdateUserAsync(command, userEmail);

            return NoContent();
        }

        /// <summary>
        /// Enables or disables a user
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="enable">True to enable, false to disable</param>
        /// <response code="204">User status updated successfully</response>
        /// <response code="400">Invalid parameters</response>
        [HttpPut("enable")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetUserEnabledAsync([FromQuery] string userEmail, [FromQuery] bool enable)
        {
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("User email must be provided.");

            await _usersService.SetUserEnabledAsync(userEmail, enable);

            return NoContent();
        }

        /// <summary>
        /// Searches users by username or email
        /// </summary>
        /// <param name="username">Username (optional)</param>
        /// <param name="email">Email (optional)</param>
        /// <returns>User found</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="400">Username or email not provided</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByUsernameOrEmailAsync([FromQuery] string? username, [FromQuery] string? email)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
                return BadRequest("Either username or email must be provided.");

            return Ok(await _usersService.GetByUsernameOrEmailAsync(username, email));
        }
    }
}