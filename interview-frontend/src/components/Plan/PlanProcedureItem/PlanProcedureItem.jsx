import React, { useState,useEffect } from "react";
import ReactSelect from "react-select";
import { useParams } from "react-router-dom";
import {
    getAssignedUsersProcedurePlans,
    assignUsersToPlanProcedure,
    removeUserFromProcedure,
} from "../../../api/api";

const PlanProcedureItem = ({ procedure, users }) => {
    const [selectedUsers, setSelectedUsers] = useState(null);
    const { id } = useParams();

    
    const handleAssignUserToProcedure = async (e) => {
        setSelectedUsers(e);
        // TODO: Remove console.log and add missing logic
        console.log(e);
    };

    return (
        <div className="py-2">
            <div>
                {procedure.procedureTitle}
            </div>

            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={(e) => handleAssignUserToProcedure(e)}
            />
        </div>
    );
};

export default PlanProcedureItem;
