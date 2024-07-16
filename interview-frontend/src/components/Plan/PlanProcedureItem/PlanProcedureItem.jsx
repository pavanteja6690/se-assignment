import React, { useState, useEffect } from "react";
import ReactSelect from "react-select";
import { useParams } from "react-router-dom";
import {
  getPlanProcedureUsers,
  addUserToProcedure,
}from "../../../api/api"

const PlanProcedureItem = ({ procedure, users }) => {
    let { id } = useParams();
    const [selectedUsers, setSelectedUsers] = useState(null);

    useEffect(() => {
      (async () => {
        var getselectedUsers = await getPlanProcedureUsers(id, procedure.procedureId);
        const initialSelectedUsers = getselectedUsers.map((user) => ({
          label: user.user.name,
          value: user.user.userId,
          }));
        console.log(initialSelectedUsers);
        setSelectedUsers(initialSelectedUsers);
      })();
    }, [procedure]);



    const handleAssignUserToProcedure = async (e) => {
        setSelectedUsers(e);
        await addUserToProcedure(id, procedure.procedureId, e[e.length - 1].value);
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
