/*将原有超管super，移到默认机构的管理岗*/
UPDATE "EmpAccount" set "Posi_Id"= (SELECT "Posi_Id" FROM "Position" WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsRoot"=true) and "Posi_IsAdmin"=TRUE),
"Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsDefault"=true)
WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsRoot"=true) OR "Acc_AccName"='super';


/*将默认机构设置为根机构*/

UPDATE "Organization" set "Org_IsRoot"=TRUE WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsDefault"=true);
UPDATE "Organization" set "Org_IsRoot"=false WHERE "Org_ID"!=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsDefault"=true);

--SELECT * from "EmpAccount" WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsDefault"=true)

--SELECT * from "EmpAccount" WHERE "Acc_AccName"='super'

--SELECT * FROM "Position" WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsRoot"=true) and "Posi_IsAdmin"=TRUE




