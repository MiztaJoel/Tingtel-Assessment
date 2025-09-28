using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tingtel_Assessment.Core.DTOs.Category;
using Tingtel_Assessment.Core.Interfaces;
using Tingtel_Assessment.Core.Repositories;
using Tingtel_Assessment.Interfaces;

namespace Tingtel_Assessment.Controllers
{
    [Route("api/generate-transactions")]
    [ApiController]
    public class GenerateTransactionController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IIdentityRepository _identityRepository;


        public GenerateTransactionController(
            IHttpContextAccessor contextAccessor,
            ITransactionRepository transactionRepository,
            IIdentityRepository identityRepository)
        {
            _transactionRepository = transactionRepository;
            _contextAccessor = contextAccessor;
            _identityRepository = identityRepository;
        }

        /// <summary>
        /// Creates transaction for authorize user 
        /// </summary>
        /// <param name="count">number of transaction.</param>
        /// <returns>The newly created transactions.</returns>
        [Authorize]
        [HttpPost("create-transations")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTransaction([FromBody] int count)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _identityRepository.GetUserByIdAsync(UserId!);

            string userId = user.UserId;

            await _transactionRepository.ResetAndGenerateTransactions(userId, count);
            return Ok("Transactions created successfully");

        }
        /// <summary>
        /// Calculating percentage increament  provided Current month is higher than previous month for authorize user 
        ///<param name="categoryname">Food,Transport,Utilities,Shopping,Health,Education.</param>
        /// </summary>
        /// <returns>Sending email if there any increment in certain item.</returns>
        [Authorize]
        [HttpGet("percentage-increment/{categoryname:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculatePercentageIncrement(string categoryname)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _identityRepository.GetUserByIdAsync(UserId!);
            var userid = user.UserId;
            string username = user.Username;
            string email = user.Email;


            string userId = user.UserId;

            await _transactionRepository.CheckIfRecentTransactionIsHigher(userid, categoryname,username,email);
            return Ok("Successfully check percentage increment");

        }

    }
}
