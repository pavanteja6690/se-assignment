const api_url = "http://localhost:10010";

export const startPlan = async () => {
    const url = `${api_url}/Plan`;
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({}),
    });

    if (!response.ok) throw new Error("Failed to create plan");

    return await response.json();
};

export const addProcedureToPlan = async (planId, procedureId) => {
    const url = `${api_url}/Plan/AddProcedureToPlan`;
    var command = { planId: planId, procedureId: procedureId };
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(command),
    });

    if (!response.ok) throw new Error("Failed to create plan");

    return true;
};

export const getProcedures = async () => {
    const url = `${api_url}/Procedures`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get procedures");

    return await response.json();
};

export const getPlanProcedures = async (planId) => {
    const url = `${api_url}/PlanProcedure?$filter=planId eq ${planId}&$expand=procedure`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get plan procedures");

    return await response.json();
};

export const getUsers = async () => {
    const url = `${api_url}/Users`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get users");

    return await response.json();
};
export const removeUserFromProcedure = async (procedureId,planId, userId) => {
    try {
        const url = `${api_url}/PlanProcedure/RemoveUsers`;
        var command = { procedureId,planId, userId };
        const response = await fetch(url, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify(command),
        });
        return response.json();
    } catch (error) {
        console.error('Error adding users to procedure:', error);
        throw new Error("Failed to Add users to procedure");

    }
};

export const getAssignedUsersProcedurePlans = async (procedureId,planId) => {
    try {
        const url = `${api_url}/PlanProcedure/GetAssignedUsers/${procedureId}/${planId}`;
        const response = await fetch(url, {
            method: "GET",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },});

            if (response.ok && response.status==200) return await response.json();
        return false;
    } catch (error) {
        console.error('Error getting users from procedure:', error);
        throw error;
    }
};

export const assignUsersToPlanProcedure = async (procedureId,planId, userId) => {
    try {
        const url = `${api_url}/PlanProcedure/AssignUsers`;
        var command = { procedureId,planId, userId };
        const response = await fetch(url, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify(command),
        });
        return response.json();
    } catch (error) {
        console.error('Error adding users to procedure:', error);
        throw new Error("Failed to Add users to procedure");

    }
};