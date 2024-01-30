using DiffServiceApp.Application.DiffCouple.Queries.GetResult;
using DiffServiceApp.Application.DiffCouple.Update;
using DiffServiceApp.Contracts.Requests;
using DiffServiceApp.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiffServiceApp.API.Controllers;

[Route("v1/diff")]
[ApiController]
public class DiffController(ISender _sender) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResultAsync(int id, CancellationToken cancellationToken)
    {
        var query = new GetDiffResultQuery(id);

        GetResultResponse result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }


    [HttpPut("{id:int}/{side}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromRoute] string side, [FromBody] UpdateDiffValueRequest request)
    {
        var command = new UpdateDiffCommand(id, request.Data, side);

        await _sender.Send(command);

        return Created();
    }
}
