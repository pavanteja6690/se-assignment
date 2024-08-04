using MediatR;
using RL.Backend.Models;

namespace RL.Backend.Commands
{
    public class AddProcedureToPlanCommand : IRequest<ApiResponse<Unit>>
    {
        public int PlanId { get; set; }
        public int ProcedureId { get; set; }
    }
    public class AssignUsersToPlanCommand : IRequest<ApiResponse<Unit>>
    {
        public int ProcedureId { get; set; }
        public int PlanId { get; set; }
        public int UserId { get; set; }

    }
    public class RemoveUsersFromPlanCommand : AssignUsersToPlanCommand { }
}