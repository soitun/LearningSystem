/*将原有超管super，移到默认机构的管理岗*/
UPDATE "EmpAccount" set "Posi_Id"= (SELECT "Posi_Id" FROM "Position" WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsRoot"=true) and "Posi_IsAdmin"=TRUE),
"Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsDefault"=true)
WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsRoot"=true) OR "Acc_AccName"='super';

/*将默认机构设置为根机构*/
UPDATE "Organization" set "Org_IsRoot"=TRUE WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsDefault"=true);
UPDATE "Organization" set "Org_IsRoot"=false WHERE "Org_ID"!=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsDefault"=true);

--SELECT * from "EmpAccount" WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsDefault"=true);
--SELECT * from "EmpAccount" WHERE "Acc_AccName"='super';
--SELECT * FROM "Position" WHERE "Org_ID"=(SELECT "Org_ID" FROM "Organization" WHERE "Org_IsRoot"=true) and "Posi_IsAdmin"=TRUE;

/*试卷增加是否需要批阅的字段*/

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


/*考试场次的记录中没有Org_ID,只有考试主题有，这里是同步数据*/
DO $$
DECLARE exam_record RECORD;
BEGIN
    -- 使用 FOR 循环直接遍历查询结果
    FOR exam_record IN SELECT "Exam_UID", "Org_ID" FROM "Examination" WHERE "Exam_IsTheme"=TRUE
    LOOP
        -- 在此处对每一行数据进行处理
        RAISE NOTICE 'Exam_UID: %, Org_ID: %', exam_record."Exam_UID", exam_record."Org_ID";
        UPDATE "Examination" set "Org_ID"=exam_record."Org_ID" WHERE "Exam_IsTheme"=FALSE and "Exam_UID"=exam_record."Exam_UID";
    END LOOP;
END $$;

/*修订一些字段，之前命名不规范*/

/*修改排序字段*/
ALTER TABLE "CoursePrice" RENAME COLUMN "CP_Tax" TO "CP_Order";
ALTER TABLE "EmpTitle" RENAME COLUMN "Title_Tax" TO "Title_Order";
ALTER TABLE "Links" RENAME COLUMN "Lk_Tax" TO "Lk_Order";
ALTER TABLE "LinksSort" RENAME COLUMN "Ls_Tax" TO "Ls_Order";
ALTER TABLE "QuesTypes" RENAME COLUMN "Qt_Tax" TO "Qt_Order";

ALTER TABLE "Subject" RENAME COLUMN "Sbj_Tax" TO "Sbj_Order";
ALTER TABLE "Teacher" RENAME COLUMN "Th_Tax" TO "Th_Order";
ALTER TABLE "TeacherSort" RENAME COLUMN "Ths_Tax" TO "Ths_Order";
ALTER TABLE "StudentSort" RENAME COLUMN "Sts_Tax" TO "Sts_Order";
ALTER TABLE "Special" RENAME COLUMN "Sp_Tax" TO "Sp_Order";

ALTER TABLE "Columns" RENAME COLUMN "Col_Tax" TO "Col_Order";
ALTER TABLE "Course" RENAME COLUMN "Cou_Tax" TO "Cou_Order";
ALTER TABLE "Depart" RENAME COLUMN "Dep_Tax" TO "Dep_Order";
ALTER TABLE "EmpGroup" RENAME COLUMN "EGrp_Tax" TO "EGrp_Order";
ALTER TABLE "ManageMenu" RENAME COLUMN "MM_Tax" TO "MM_Order";

ALTER TABLE "Examination" RENAME COLUMN "Exam_Tax" TO "Exam_Order";
ALTER TABLE "GuideColumns" RENAME COLUMN "Gc_Tax" TO "Gc_Order";
ALTER TABLE "KnowledgeSort" RENAME COLUMN "Kns_Tax" TO "Kns_Order";
ALTER TABLE "Navigation" RENAME COLUMN "Nav_Tax" TO "Nav_Order";
ALTER TABLE "OrganLevel" RENAME COLUMN "Olv_Tax" TO "Olv_Order";

ALTER TABLE "Outline" RENAME COLUMN "Ol_Tax" TO "Ol_Order";
ALTER TABLE "PayInterface" RENAME COLUMN "Pai_Tax" TO "Pai_Order";
ALTER TABLE "Position" RENAME COLUMN "Posi_Tax" TO "Posi_Order";
ALTER TABLE "Questions" RENAME COLUMN "Qus_Tax" TO "Qus_Order";
ALTER TABLE "ShowPicture" RENAME COLUMN "Shp_Tax" TO "Shp_Order";
ALTER TABLE "ThirdpartyLogin" RENAME COLUMN "Tl_Tax" TO "Tl_Order";

/*修改性别字段*/
ALTER TABLE "Accounts" RENAME COLUMN "Ac_Sex" TO "Ac_Gender";
ALTER TABLE "EmpAccount" RENAME COLUMN "Acc_Sex" TO "Acc_Gender";
ALTER TABLE "ExamResults" RENAME COLUMN "Ac_Sex" TO "Ac_Gender";
ALTER TABLE "Teacher" RENAME COLUMN "Th_Sex" TO "Th_Gender";
ALTER TABLE "TestResults" RENAME COLUMN "St_Sex" TO "Ac_Gender";


