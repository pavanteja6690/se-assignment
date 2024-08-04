using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Commands;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PlanProcedureController : ControllerBase
{
    private readonly ILogger<PlanProcedureController> _logger;
    private readonly RLContext _context;
    private readonly IMediator _mediator;
    public PlanProcedureController(ILogger<PlanProcedureController> logger, RLContext context, IMediator mediator)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    }

    [HttpGet]
    [EnableQuery]
    public IEnumerable<PlanProcedure> Get()
    {
        return _context.PlanProcedures;
    }
    [HttpPost("AssignUsers")]
    public async Task<IActionResult> AssignUser(AssignUsersToPlanCommand command)
    {
        var response = await _mediator.Send(command);
        return response.ToActionResult();
    }

    [HttpPost("RemoveUsers")]
    public async Task<IActionResult> RemoveUsers(RemoveUsersFromPlanCommand command)
    {
        var response = await _mediator.Send(command);
        if (response.Exception?.Message== "Failed to unlink user")
        {
            return NoContent();
        }
        return response.ToActionResult();
    }

    [HttpGet("GetAssignedUsers/{procedureId}/{planId}")]
    public async Task<IActionResult> GetUsers(int procedureId, int planId)
    {   
        var users =await _context.UserProcedurePlans
            .Where(x => x.PlanId == planId && x.ProcedureId == procedureId)
            .Select(u => u.User).ToListAsync();

        if (users?.Count <= 0)
        {
            return NoContent();
        }

        return Ok(users);

    }
}
