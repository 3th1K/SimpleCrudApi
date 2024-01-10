using CrudApiAssignment.Models;
using CrudApiAssignment.Queries;
using CrudApiAssignment.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudApiAssignment.Controllers
{
    [Route("api/simplecrudapp")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MainController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var token = await _mediator.Send(loginRequest);
                return token.Result;
            }
            catch (ValidationException ex) 
            {
                return ApiResult<ErrorResult>.Failure(ErrorType.ErrRequestValidationFailed, "Validation Falied", ex.Errors.Select(error => error.ErrorMessage).ToList()).Result;
            }
            
        }

        [Route("allUsers")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var data = await _mediator.Send(new GetAllUsersQuery());
            return data.Result;
        }

        [Route("user/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetSingleUser(string userId)
        {
            var data = await _mediator.Send(new GetSingleUserQuery(userId));
            return data.Result;
        }
    }
}
