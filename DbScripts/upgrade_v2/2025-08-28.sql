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

/*修订TestPaperItem表,删除一些冗余，添加试卷ID */
ALTER TABLE IF EXISTS "TestPaperItem" DROP COLUMN IF EXISTS "Org_Name" CASCADE;
ALTER TABLE IF EXISTS "TestPaperItem" DROP COLUMN IF EXISTS "Tp_UID" CASCADE;
ALTER TABLE  IF EXISTS "TestPaperItem" ADD COLUMN "Tp_Id" int8 NOT NULL  DEFAULT 0;

/*修订，增加人工批阅的字段*/
ALTER TABLE IF EXISTS  "Examination" ADD COLUMN "Exam_IsManual" BOOLEAN  NOT NULL DEFAULT FALSE;
ALTER TABLE IF EXISTS  "TestPaper" ADD COLUMN "Tp_IsManual" BOOLEAN  NOT NULL DEFAULT FALSE;
CREATE INDEX IF NOT EXISTS "Examination_Exam_IsManual" ON "Examination" ("Exam_IsManual" ASC);
CREATE INDEX IF NOT EXISTS "TestPaper_Tp_IsManual" ON "TestPaper" ("Tp_IsManual" ASC);

/*修订一些其它字段，也许不用修订*/
UPDATE "SingleSignOn" set "SSO_IsAddSort"=FALSE WHERE "SSO_IsAddSort" ISNULL;
ALTER TABLE "SingleSignOn" ALTER COLUMN "SSO_IsAddSort" SET NOT NULL;
UPDATE "TestResults" set "Tr_Score"=0 WHERE "Tr_Score" ISNULL;
ALTER TABLE "TestResults" ALTER COLUMN "Tr_Score" SET NOT NULL;


/*有简答题的试卷，以及引用该试卷的考试，设置人工批阅*/
--SELECT "Tp_Id" FROM "TestPaper" WHERE (xpath('/Config/All/Items/TestPaperItem/TPI_Count/text()', "Tp_FromConfig"::xml ))[4]::text!='0';
--SELECT "Tp_Id" FROM "TestPaper" WHERE (xpath('/Config/Outline/Percent/TestPaperItem/TPI_Count/text()', "Tp_FromConfig"::xml ))[4]::text!='0';
UPDATE "TestPaper" set "Tp_IsManual"=FALSE;
UPDATE "Examination" set "Exam_IsManual"=FALSE;
UPDATE "TestPaper" set "Tp_IsManual"=true  WHERE (xpath('/Config/All/Items/TestPaperItem/TPI_Count/text()', "Tp_FromConfig"::xml ))[4]::text!='0';
UPDATE "TestPaper" set "Tp_IsManual"=true  WHERE (xpath('/Config/Outline/Percent/TestPaperItem/TPI_Count/text()', "Tp_FromConfig"::xml ))[4]::text!='0';

UPDATE "Examination" set "Exam_IsManual"=true WHERE "Tp_Id" in (SELECT "Tp_Id" FROM "TestPaper" WHERE "Tp_IsManual"=true);
