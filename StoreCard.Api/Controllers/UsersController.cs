using Microsoft.AspNetCore.Mvc;
using StoreCard.Application.Dtos.User;
using StoreCard.Application.Interfaces;

namespace StoreCard.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>The List of employees</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "An unexpected error occurred while getting all users.",
                    detail = ex.Message
                });
            }

        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            try
            {
                var user = await _userService.GetUserAsync(id);
                return user != null
                    ? Ok(user)
                    : NotFound($"User with Id {id} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with Id {UserId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST /api/user
        ///     {
        ///         "fullName": "John Doe"
        ///     }
        /// </remarks> 
        /// <param name="user"></param>
        /// <returns>A newly created user</returns>
        /// <response code="201">Return the newly created item </response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] UserDto user)
        {
            if (user is null)
                return BadRequest(new { error = "User data is required." });

            try
            {
                var createdUser = await _userService.CreateUserAsync(user);

                if (createdUser is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Failed to create user." });

                return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "An unexpected error occurred while creating the user.",
                    detail = ex.Message
                });
            }
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto user)
        {
            if (user is null)
                return BadRequest("Request body cannot be null.");

            if (id != user.Id)
                return BadRequest("The route Id does not match the user Id in the request body.");

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(user);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with Id {id} was not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "An unexpected error occurred while updating the user.",
                    detail = ex.Message
                });
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _userService.DeleteUserAsync(id);
                if (!deleted)
                    return NotFound($"User with Id {id} was not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "An unexpected error occurred while deleting the user.",
                    detail = ex.Message
                });
            }
        }

    }

}
