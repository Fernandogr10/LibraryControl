using System;
using System.Threading.Tasks;
using LibraryControl.Application.Commands.Users;
using LibraryControl.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllUsers.Query();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetUserById.Query(id);
            var result = await _mediator.Send(query);
            
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddUser.Command command)
        {
            var result = await _mediator.Send(command);
            return result == Guid.Empty ? Problem(statusCode: StatusCodes.Status400BadRequest) : CreatedAtAction(nameof(GetById), new { id = result }, command);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUser.Command(id);
            var result = await _mediator.Send(command);
            
            return result != Guid.Empty ? NoContent() : NotFound();
        }
    }
}
