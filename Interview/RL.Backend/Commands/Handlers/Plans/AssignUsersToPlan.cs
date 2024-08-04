using MediatR;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data.DataModels;
using RL.Data;
using Microsoft.EntityFrameworkCore;

namespace RL.Backend.Commands.Handlers.Plans
{
    public class AssignUsersToPlan : IRequestHandler<AssignUsersToPlanCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;
        public AssignUsersToPlan(RLContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Unit>> Handle(AssignUsersToPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PlanId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
                if (request.ProcedureId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));

                var isPlanExist = _context.Plans.Any(x => x.PlanId == request.PlanId);
                var procedure = _context.Procedures.SingleOrDefault(x => request.ProcedureId == x.ProcedureId);

                if (!isPlanExist)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"PlanId: {request.PlanId} not found"));

                if (procedure is null)
                {
                    return ApiResponse<Unit>.Fail(new Exception("Plan procedure was null"));
                }

                var userAlreadyExist = await _context.UserProcedurePlans?.AnyAsync(x => x.User.UserId == request.UserId && (x.PlanId==request.PlanId&&x.ProcedureId==request.ProcedureId));

                if (userAlreadyExist)
                    return ApiResponse<Unit>.Succeed(new Unit());

                var user = _context.Users.SingleOrDefault(x => x.UserId == request.UserId);
                var plan = _context.Plans.SingleOrDefault(x => x.PlanId == request.PlanId);

                _context.UserProcedurePlans.Add(
                    new UserProcedurePlan()
                    {
                        User = user,
                        UserId = user.UserId,
                        Procedure = procedure,
                        Plan = plan
                    }
                    );


                await _context.SaveChangesAsync(cancellationToken);

                return ApiResponse<Unit>.Succeed(new Unit());
            }
            catch (Exception e)
            {
                return ApiResponse<Unit>.Fail(e);
            }
        }
    }

    public class RemoveUsersToPlan : IRequestHandler<RemoveUsersFromPlanCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;
        public RemoveUsersToPlan(RLContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Unit>> Handle(RemoveUsersFromPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PlanId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
                if (request.ProcedureId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));

                var isPlanExist = await _context.Plans.AnyAsync(x => x.PlanId == request.PlanId,cancellationToken);
                var procedure = await _context.Procedures.SingleOrDefaultAsync(x => request.ProcedureId == x.ProcedureId, cancellationToken);

                if (!isPlanExist)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"PlanId: {request.PlanId} not found"));

                if (procedure is null)
                {
                    return ApiResponse<Unit>.Fail(new Exception("Plan procedure was null"));
                }

                var userProcedurePlan = await _context.UserProcedurePlans
                                           .FirstOrDefaultAsync(ppu => (ppu.PlanId == request.PlanId
                                           && ppu.ProcedureId == request.ProcedureId) && ppu.UserId == request.UserId, cancellationToken);
                if (userProcedurePlan != null)
                {
                    _context.UserProcedurePlans.Remove(userProcedurePlan);
                    await _context.SaveChangesAsync(cancellationToken);
                    return ApiResponse<Unit>.Succeed(new Unit());
                }
                return ApiResponse<Unit>.Fail(new ("Failed to unlink user"));
            }
            catch (Exception e)
            {
                return ApiResponse<Unit>.Fail(e);
            }
        }
    }
}
