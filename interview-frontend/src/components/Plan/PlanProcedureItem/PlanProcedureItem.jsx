import React, { useState, useEffect } from "react";
import ReactSelect from "react-select";
import { assignUserToProcedure, getAssignedUsers, removeUserFromProcedure } from "../../../api/api";

const PlanProcedureItem = ({ planId, procedure, users }) => {
    const [selectedUsers, setSelectedUsers] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchAssignedUsers = async () => {
            try {
                const assignedUsers = await getAssignedUsers(procedure.procedureId, planId);
                const assignedUserOptions = assignedUsers.map(user => ({ label: user.name, value: user.userId }));
                setSelectedUsers(assignedUserOptions);
            } catch (error) {
                console.error("Failed to fetch assigned users", error);
            }
        };

        fetchAssignedUsers();
    }, [procedure.procedureId, planId]);

    const handleAssignUserToProcedure = async (selectedOptions) => {
        setError(null); // Clear previous error
        const newSelectedUsers = selectedOptions.filter(option => !selectedUsers.some(user => user.value === option.value));
        const removedUsers = selectedUsers.filter(user => !selectedOptions.some(option => option.value === user.value));

        setSelectedUsers(selectedOptions);

        for (let option of newSelectedUsers) {
            try {
                await assignUserToProcedure(planId, procedure.procedureId, option.value);
            } catch (error) {
                setError("Failed to assign user. User might already be assigned to this procedure in the current plan.");
                setSelectedUsers(selectedUsers); // Revert to previous state
                break;
            }
        }

        for (let user of removedUsers) {
            try {
                await removeUserFromProcedure(planId, procedure.procedureId, user.value);
            } catch (error) {
                setError("Failed to remove user.");
                console.error("Failed to remove user", error);
            }
        }
    };

    return (
        <div className="py-2">
            <div>{procedure.procedureTitle}</div>
            {error && <div className="alert alert-danger">{error}</div>}
            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={handleAssignUserToProcedure}
            />
        </div>
    );
};

export default PlanProcedureItem;
