using Moq;
using RL.Backend.Commands.Handlers.Plans;
using RL.Backend.Commands;
using RL.Backend.Exceptions;
using RL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace RL.Backend.UnitTests;

[TestClass]
public class AssignUsersToPlanTests
{

    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(int.MinValue)]
    public async Task AssignUsersToPlan_InvalidPlanId_ReturnsBadRequest(int planId)
    {
        //Given
        var context = new Mock<RLContext>();
        var sut = new AssignUsersToPlan(context.Object);
        var request = new AssignUsersToPlanCommand()
        {
            PlanId = planId,
            ProcedureId = 1
        };
        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(BadRequestException));
        result.Succeeded.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(int.MinValue)]
    public async Task AssignUsersToPlan_InvalidProcedureId_ReturnsBadRequest(int procedureId)
    {
        //Given
        var context = new Mock<RLContext>();
        var sut = new AssignUsersToPlan(context.Object);
        var request = new AssignUsersToPlanCommand()
        {
            PlanId = 1,
            ProcedureId = procedureId
        };
        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(BadRequestException));
        result.Succeeded.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(19)]
    [DataRow(35)]
    public async Task AssignUsersToPlan_PlanIdNotFound_ReturnsNotFound(int planId)
    {
        //Given
        var context = DbContextHelper.CreateContext();
        var sut = new AssignUsersToPlan(context);
        var request = new AssignUsersToPlanCommand()
        {
            PlanId = planId,
            ProcedureId = 1
        };

        context.Plans.Add(new Data.DataModels.Plan
        {
            PlanId = planId + 1
        });
        await context.SaveChangesAsync();

        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(NotFoundException));
        result.Succeeded.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(19)]
    [DataRow(35)]
    public async Task AssignUsersToPlan_ProcedureIdNotFound_ReturnsNotFound(int procedureId)
    {
        //Given
        var context = DbContextHelper.CreateContext();
        var sut = new AssignUsersToPlan(context);
        var request = new AssignUsersToPlanCommand()
        {
            PlanId = 1,
            ProcedureId = procedureId
        };

        context.Plans.Add(new Data.DataModels.Plan
        {
            PlanId = procedureId + 1
        });
        context.Procedures.Add(new Data.DataModels.Procedure
        {
            ProcedureId = procedureId + 1,
            ProcedureTitle = "Test Procedure"
        });
        await context.SaveChangesAsync();

        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(NotFoundException));
        result.Succeeded.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(19)]
    [DataRow(35)]
    public async Task AssignUsersToPlan_UserIdNotFound_ReturnsNotFound(int userId)
    {
        //Given
        var context = DbContextHelper.CreateContext();
        var sut = new AssignUsersToPlan(context);
        var request = new AssignUsersToPlanCommand()
        {
            PlanId = 1,
            ProcedureId = userId
        };

        context.Plans.Add(new Data.DataModels.Plan
        {
            PlanId = userId + 1
        });
        context.Procedures.Add(new Data.DataModels.Procedure
        {
            ProcedureId = userId + 1,
            ProcedureTitle = "Test Procedure"
        });
        await context.SaveChangesAsync();

        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(NotFoundException));
        result.Succeeded.Should().BeFalse();
    }
}

[TestClass]
public class RemoveUsersToPlanTests
{

    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(int.MinValue)]
    public async Task RemoveUsersToPlan_InvalidPlanId_ReturnsBadRequest(int planId)
    {
        //Given
        var context = new Mock<RLContext>();
        var sut = new RemoveUsersToPlan(context.Object);
        var request = new RemoveUsersFromPlanCommand()
        {
            PlanId = planId,
            ProcedureId = 1
        };
        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(BadRequestException));
        result.Succeeded.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(int.MinValue)]
    public async Task RemoveUsersToPlan_InvalidProcedureId_ReturnsBadRequest(int procedureId)
    {
        //Given
        var context = new Mock<RLContext>();
        var sut = new RemoveUsersToPlan(context.Object);
        var request = new RemoveUsersFromPlanCommand()
        {
            PlanId = 1,
            ProcedureId = procedureId
        };
        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(BadRequestException));
        result.Succeeded.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(19)]
    [DataRow(35)]
    public async Task RemoveUsersToPlan_PlanIdNotFound_ReturnsNotFound(int planId)
    {
        //Given
        var context = DbContextHelper.CreateContext();
        var sut = new RemoveUsersToPlan(context);
        var request = new RemoveUsersFromPlanCommand()
        {
            PlanId = planId,
            ProcedureId = 1
        };

        context.Plans.Add(new Data.DataModels.Plan
        {
            PlanId = planId + 1
        });
        await context.SaveChangesAsync();

        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(NotFoundException));
        result.Succeeded.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(19)]
    [DataRow(35)]
    public async Task RemoveUsersToPlan_ProcedureIdNotFound_ReturnsNotFound(int procedureId)
    {
        //Given
        var context = DbContextHelper.CreateContext();
        var sut = new RemoveUsersToPlan(context);
        var request = new RemoveUsersFromPlanCommand()
        {
            PlanId = 1,
            ProcedureId = procedureId
        };

        context.Plans.Add(new Data.DataModels.Plan
        {
            PlanId = procedureId + 1
        });
        context.Procedures.Add(new Data.DataModels.Procedure
        {
            ProcedureId = procedureId + 1,
            ProcedureTitle = "Test Procedure"
        });
        await context.SaveChangesAsync();

        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(NotFoundException));
        result.Succeeded.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(19)]
    [DataRow(35)]
    public async Task RemoveUsersToPlan_UserIdNotFound_ReturnsNotFound(int userId)
    {
        //Given
        var context = DbContextHelper.CreateContext();
        var sut = new RemoveUsersToPlan(context);
        var request = new RemoveUsersFromPlanCommand()
        {
            PlanId = 1,
            ProcedureId = userId
        };

        context.Plans.Add(new Data.DataModels.Plan
        {
            PlanId = userId + 1
        });
        context.Procedures.Add(new Data.DataModels.Procedure
        {
            ProcedureId = userId + 1,
            ProcedureTitle = "Test Procedure"
        });
        await context.SaveChangesAsync();

        //When
        var result = await sut.Handle(request, new CancellationToken());

        //Then
        result.Exception.Should().BeOfType(typeof(NotFoundException));
        result.Succeeded.Should().BeFalse();
    }
}