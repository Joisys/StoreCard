using StoreCard.Application.Interfaces;

namespace StoreCard.Api.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using StoreCard.Application.Dtos.UserTransaction;
    using StoreCard.Domain.Enums;

    [ApiController]
    [Route("api/[controller]")]
    public class UserTransactionsController : ControllerBase
    {
        private readonly IUserTransactionService _userTransactionService;

        public UserTransactionsController(IUserTransactionService userTransactionService)
        {
            _userTransactionService = userTransactionService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserTransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserTransactionDto>>> GetAllUserTransactions()
        {
            try
            {
                var userTransactions = await _userTransactionService.GetAllUserTransactionsAsync();
                return Ok(userTransactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "An unexpected error occurred while getting all user userTransactions.",
                    detail = ex.Message
                });
            }

        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserTransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserTransactionDto>> GetUserTransaction(int id)
        {
            try
            {
                var userTransaction = await _userTransactionService.GetUserTransactionAsync(id);
                return Ok(userTransaction);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Transaction with ID {id} not found.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserTransactionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserTransactionDto>> CreateUserTransaction([FromBody] UserTransactionCreateDto dto)
        {
            if (dto == null)
                return BadRequest("User userTransaction data is required.");

            var userTransaction = await _userTransactionService.CreateUserTransactionAsync(dto);

            return CreatedAtAction(nameof(CreateUserTransaction), new { id = userTransaction.Id }, userTransaction);
        }

        // GET: api/transactions/summary?type=TotalPerUser&threshold=100
        [HttpGet("summary")]
        [ProducesResponseType(typeof(IEnumerable<UserTransactionSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionSummary([FromQuery] string type, [FromQuery] decimal? threshold = null)
        {
            if (!Enum.TryParse<SummaryType>(type, true, out var summaryType))
            {
                return BadRequest($"Invalid summary type: {type}. Allowed values: TotalPerUser, TotalPerTransactionType, HighVolume.");
            }

            try
            {
                var summary = await _userTransactionService.GetTransactionSummaryAsync(summaryType, threshold);
                return Ok(summary);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }

}
