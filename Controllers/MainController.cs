using CrudApiAssignment.Models;
using CrudApiAssignment.Queries;
using CrudApiAssignment.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrudApiAssignment.Controllers
{
    [Route("simpleCrudApi")]
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
            var token = await _mediator.Send(loginRequest);
            return token.Result;

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

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] UserRequest request)
        {
            var data = await _mediator.Send(request);
            return data.Result;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
        {
            var data = await _mediator.Send(request);
            return data.Result;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await _mediator.Send(new DeleteUserQuery(id));
            return data.Result;
        }

        [HttpPost]
        [Route("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search([FromBody] SearchUserRequest searchUserRequest) 
        { 
            var data = await _mediator.Send(searchUserRequest);
            return data.Result;
        }
    }
}
