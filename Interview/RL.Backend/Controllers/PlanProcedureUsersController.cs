using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RL.Data.DataModels;
using RL.Data;
using Microsoft.EntityFrameworkCore;

namespace RL.Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PlanProcedureUsersController : ControllerBase
    {
        private readonly RLContext _context;

        public PlanProcedureUsersController(RLContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserToProcedure([FromBody] AssignUserRequest request)
        {
            var existingAssignment = await _context.planProcedureUsers
                                           .FirstOrDefaultAsync(ppu => ppu.PlanId == request.PlanId && ppu.ProcedureId == request.ProcedureId && ppu.UserId == request.UserId);
            if (existingAssignment != null)
            {
                return BadRequest("User is already assigned to this procedure in the current plan.");
            }

            var planProcedureUser = new PlanProcedureUser
            {
                PlanId = request.PlanId,
                ProcedureId = request.ProcedureId,
                UserId = request.UserId
            };
            _context.planProcedureUsers.Add(planProcedureUser);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{procedureId}/{planId}")]
        public async Task<IActionResult> GetAssignedUsers(int procedureId, int planId)
        {
            var users = await _context.planProcedureUsers
                                      .Where(ppu => ppu.ProcedureId == procedureId && ppu.PlanId == planId)
                                      .Select(ppu => ppu.User)
                                      .ToListAsync();
            return Ok(users);
        }
        [HttpDelete("RemoveUserFromProcedure")]
        public async Task<IActionResult> RemoveUserFromProcedure([FromBody] AssignUserRequest request)
        {
            var assignment = await _context.planProcedureUsers
                                           .FirstOrDefaultAsync(ppu => ppu.PlanId == request.PlanId && ppu.ProcedureId == request.ProcedureId && ppu.UserId == request.UserId);

            if (assignment == null)
            {
                return NotFound("User not found.");
            }

            _context.planProcedureUsers.Remove(assignment);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }

    public class AssignUserRequest
    {
        public int ProcedureId { get; set; }
        public int UserId { get; set; }

        public int PlanId { get; set; }
    }
}