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

/*在线考试中的试题相关*/
-- 创建试题分类表
CREATE TABLE "QuesPart" (
    "Qp_ID" BIGINT PRIMARY KEY,           -- 主键，雪花ID
    "Qp_PID" BIGINT NOT NULL,             -- 父级ID，雪花ID
    "Qp_Name" VARCHAR(1000) NOT NULL,     -- 分类名称
    "Org_ID" INT NOT NULL,                -- 组织机构ID
    "QP_Count" INT NOT null DEFAULT 0,             -- 试题数量
    "Qp_Order" INT NOT null DEFAULT 0,             -- 排序号
    "Qp_Intro" TEXT,                      -- 说明或介绍
    "Qp_IsUse" BOOLEAN NOT null  DEFAULT TRUE,      -- 是否启用
    "Qp_CrtTime" TIMESTAMP NOT null  DEFAULT CURRENT_TIMESTAMP, -- 创建时间
    "Qp_UpdateTime" TIMESTAMP NOT null  DEFAULT CURRENT_TIMESTAMP, -- 修改时间    
    -- 索引定义
    CONSTRAINT fk_parent FOREIGN KEY (Qk_PID) REFERENCES QuesKnowledge(Qk_ID) ON DELETE CASCADE
);

-- 创建索引
CREATE INDEX "QuesPart_IX_Qp_PID" ON "QuesPart"("Qp_PID");
CREATE INDEX "QuesPart_IX_Qp_Name" ON "QuesPart"("Qp_Name");
CREATE INDEX "QuesPart_IX_Qp_Order" ON "QuesPart"("Qp_Order");
CREATE INDEX "QuesPart_IX_Qp_IsUse" ON "QuesPart"("Qp_IsUse");
CREATE INDEX "QuesPart_IX_Org_ID" ON "QuesPart"("Org_ID");

-- 创建试题与分类关联表
CREATE TABLE "Questions_QPart" (
    "Qqp_ID" BIGINT PRIMARY KEY,          -- 主键，雪花ID
    "Ques_ID" BIGINT NOT NULL,            -- 试题ID
    "Qp_ID" BIGINT NOT NULL               -- 分类ID
);

-- 创建关联表索引
CREATE INDEX "Questions_QPart_IX_Ques_ID" ON "Questions_QPart"("Ques_ID");
CREATE INDEX "Questions_QPart_IX_Qp_ID" ON "Questions_QPart"("Qp_ID");
CREATE INDEX "Questions_QPart_IX_QuesQp" ON "Questions_QPart"("Ques_ID", "Qp_ID");


/*创建试题知识点*/
CREATE TABLE "QuesKnowledge" (
    "Qk_ID" BIGINT PRIMARY KEY DEFAULT 0,     -- 主键，雪花ID
    "Qk_PID" BIGINT NOT NULL DEFAULT 0,   -- 父级ID，雪花ID
    "Qk_Name" VARCHAR(1000) NOT NULL,
    "Org_ID" INT NOT NULL,             -- 组织机构ID
    "Qk_Order" INT NOT NULL DEFAULT 0,     -- 排序号
    "Qk_Weight" INT NOT NULL DEFAULT 0,     -- 权重值
    "Qk_IsUse" BOOLEAN NOT NULL DEFAULT TRUE,
    "Qk_Count" INT NOT NULL DEFAULT 0,       -- 试题数量
    "Qk_CrtTime" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,    -- 创建时间
    "Qk_UpdateTime" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- 修改时间
    "Qk_Intro" TEXT,                        --简述
    "Qk_Details" TEXT
);

-- 创建索引
CREATE INDEX "QuesKnowledge_IX_PID" ON "QuesKnowledge"("Qk_PID");
CREATE INDEX "QuesKnowledge_IX_Name" ON "QuesKnowledge"("Qk_Name");
CREATE INDEX "QuesKnowledge_IX_Order" ON "QuesKnowledge"("Qk_Order");
CREATE INDEX "QuesKnowledge_IX_IsUse" ON "QuesKnowledge"("Qk_IsUse");
CREATE INDEX "QuesKnowledge_IX_OrgID" ON "QuesKnowledge"("Org_ID");
CREATE INDEX "QuesKnowledge_IX_PID_Order" ON "QuesKnowledge"("Qk_PID", "Qk_Order");
CREATE INDEX "QuesKnowledge_IX_Org_ID" ON "QuesKnowledge"("Org_ID");
CREATE INDEX "QuesKnowledge_IX_Qk_Count" ON "QuesKnowledge"("Qk_Count");

/*试题与知识点的关联表*/
CREATE TABLE "Questions_QKnl" (
    "Qqk_ID" BIGINT PRIMARY KEY DEFAULT 0,
    "Ques_ID" BIGINT NOT NULL,
    "Qtag_ID" BIGINT NOT NULL
);
-- 创建所有字段的索引
CREATE INDEX "Questions_QKnl_IX_ID" ON "Questions_QKnl"("Qqk_ID");
CREATE INDEX "Questions_QKnl_IX_QuesID" ON "Questions_QKnl"("Ques_ID");
CREATE INDEX "Questions_QKnl_IX_TagID" ON "Questions_QKnl"("Qtag_ID");
-- 复合索引
CREATE INDEX "Questions_QKnl_IX_QuesID_TagID" ON "Questions_QKnl"("Ques_ID", "Qtag_ID");
CREATE INDEX "Questions_QKnl_IX_TagID_QuesID" ON "Questions_QKnl"("Qtag_ID", "Ques_ID");


--创建试题标签
CREATE TABLE "QuesTags" (
    "Qtag_ID" BIGINT PRIMARY KEY DEFAULT 0,
    "Qtag_PID" BIGINT NOT NULL DEFAULT 0,
    "Org_ID" INT NOT NULL,
    "Qtag_Name" VARCHAR(255) NOT NULL,
    "Cou_ID" BIGINT NOT NULL,
    "Qtag_Order" INT NOT NULL DEFAULT 0,
    "Qtag_Weight" INT NOT NULL DEFAULT 0,             --权重
    "Qtag_CrtTime" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,    -- 创建时间
    "Qtag_UpdateTime" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP -- 修改时间
);
-- 创建索引
CREATE INDEX "QuesTags_IX_PID" ON "QuesTags"("Qtag_PID");
CREATE INDEX "QuesTags_IX_OrgID" ON "QuesTags"("Org_ID");
CREATE INDEX "QuesTags_IX_Name" ON "QuesTags"("Qtag_Name");
CREATE INDEX "QuesTags_IX_CouID" ON "QuesTags"("Cou_ID");
CREATE INDEX "QuesTags_IX_Order" ON "QuesTags"("Qtag_Order");
CREATE INDEX "QuesTags_IX_Weight" ON "QuesTags"("Qtag_Weight");

--创建试题与标签的关联表
CREATE TABLE "Questions_QTags" (
    "Qqt_ID" BIGINT PRIMARY KEY DEFAULT 0,
    "Ques_ID" BIGINT NOT NULL,
    "Qtag_ID" BIGINT NOT NULL
);
-- 创建所有字段的索引
CREATE INDEX "Questions_QTags_IX_QuesID" ON "Questions_QTags"("Ques_ID");
CREATE INDEX "Questions_QTags_IX_TagID" ON "Questions_QTags"("Qtag_ID");
-- 复合索引
CREATE INDEX "Questions_QTags_IX_QuesID_TagID" ON "Questions_QTags"("Ques_ID", "Qtag_ID");
CREATE INDEX "Questions_QTags_IX_TagID_QuesID" ON "Questions_QTags"("Qtag_ID", "Ques_ID");


--创建操作日志的记录表
CREATE TABLE "DataOperateLog" (
    -- 主键
    "Dlog_ID" BIGINT PRIMARY KEY DEFAULT 0,    
    -- 操作对象的记录
    "Dlog_Entity" VARCHAR(100) NOT NULL,    --记录名，即表结构名称
    "Dlog_KeyID" BIGINT NOT NULL,   --被操作表的主键值
    "Dlog_Type" SMALLINT NOT NULL, -- 1新增, 2修改, 3删除    
    -- 时间信息
    "Dlog_CrtTime" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,  --创建时间
    "Dlog_Timespan" INTEGER, -- 用时（毫秒）    
    -- 客户端信息
    "Dlog_IP" VARCHAR(50),
    "Dlog_Browser" VARCHAR(100),
    "Dlog_OS" VARCHAR(100),
    "Dlog_BrwUa" VARCHAR(1000),    
    -- 外部ID
    "Acc_ID" BIGINT, -- 管理员ID
    "Th_ID" BIGINT, -- 教师ID
    "Ac_ID" BIGINT, -- 基础账号ID
    "Org_ID" BIGINT, -- 组织机构ID    
    -- JSON数据
    "Dlog_OldData" TEXT,  -- 历史记录
    "Dlog_NewData" TEXT,    --修改后的记录
    "Dlog_Fields" TEXT, -- 修改的字段    
    -- 操作上下文
    "Dlog_Module" VARCHAR(200), -- 业务模块
    "MM_Link" VARCHAR(1000), -- 菜单路径
    "MM_UID" VARCHAR(100), -- 菜单UID
    "Dlog_Mark" VARCHAR(500), -- 操作描述
    "Dlog_API" VARCHAR(500) -- RESTful API名称
);

-- 创建索引
CREATE INDEX "DataOperateLog_IX_ID" ON "DataOperateLog"("Dlog_ID");
CREATE INDEX "DataOperateLog_IX_Entity" ON "DataOperateLog"("Dlog_Entity");
CREATE INDEX "DataOperateLog_IX_KeyID" ON "DataOperateLog"("Dlog_KeyID");
CREATE INDEX "DataOperateLog_IX_Type" ON "DataOperateLog"("Dlog_Type");
CREATE INDEX "DataOperateLog_IX_CrtTime" ON "DataOperateLog"("Dlog_CrtTime");
CREATE INDEX "DataOperateLog_IX_IP" ON "DataOperateLog"("Dlog_IP");
CREATE INDEX "DataOperateLog_IX_AccID" ON "DataOperateLog"("Acc_ID");
CREATE INDEX "DataOperateLog_IX_ThID" ON "DataOperateLog"("Th_ID");
CREATE INDEX "DataOperateLog_IX_AcID" ON "DataOperateLog"("Ac_ID");
CREATE INDEX "DataOperateLog_IX_OrgID" ON "DataOperateLog"("Org_ID");
CREATE INDEX "DataOperateLog_IX_Module" ON "DataOperateLog"("Dlog_Module");
CREATE INDEX "DataOperateLog_IX_MMUID" ON "DataOperateLog"("MM_UID");
CREATE INDEX "DataOperateLog_IX_API" ON "DataOperateLog"("Dlog_API");

--创建操作日志的档案表
CREATE TABLE "DataOperateLogArchive" (
    -- 主键
    "Dlog_ID" BIGINT PRIMARY KEY DEFAULT 0,    
    -- 操作对象的记录
    "Dlog_Entity" VARCHAR(100) NOT NULL,    --记录名，即表结构名称
    "Dlog_KeyID" BIGINT NOT NULL,   --被操作表的主键值
    "Dlog_Type" SMALLINT NOT NULL, -- 1新增, 2修改, 3删除    
    -- 时间信息
    "Dlog_CrtTime" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,  --创建时间
    "Dlog_Timespan" INTEGER, -- 用时（毫秒）    
    -- 客户端信息
    "Dlog_IP" VARCHAR(50),
    "Dlog_Browser" VARCHAR(100),
    "Dlog_OS" VARCHAR(100),
    "Dlog_BrwUa" VARCHAR(1000),    
    -- 外部ID
    "Acc_ID" BIGINT, -- 管理员ID
    "Th_ID" BIGINT, -- 教师ID
    "Ac_ID" BIGINT, -- 基础账号ID
    "Org_ID" BIGINT, -- 组织机构ID    
    -- JSON数据
    "Dlog_OldData" TEXT,  -- 历史记录
    "Dlog_NewData" TEXT,    --修改后的记录
    "Dlog_Fields" TEXT, -- 修改的字段    
    -- 操作上下文
    "Dlog_Module" VARCHAR(200), -- 业务模块
    "MM_Link" VARCHAR(1000), -- 菜单路径
    "MM_UID" VARCHAR(100), -- 菜单UID
    "Dlog_Mark" VARCHAR(500), -- 操作描述
    "Dlog_API" VARCHAR(500) -- RESTful API名称
);

-- 创建索引
CREATE INDEX "DataOperateLogArchive_IX_ID" ON "DataOperateLogArchive"("Dlog_ID");
CREATE INDEX "DataOperateLogArchive_IX_Entity" ON "DataOperateLogArchive"("Dlog_Entity");
CREATE INDEX "DataOperateLogArchive_IX_KeyID" ON "DataOperateLogArchive"("Dlog_KeyID");
CREATE INDEX "DataOperateLogArchive_IX_Type" ON "DataOperateLogArchive"("Dlog_Type");
CREATE INDEX "DataOperateLogArchive_IX_CrtTime" ON "DataOperateLogArchive"("Dlog_CrtTime");
CREATE INDEX "DataOperateLogArchive_IX_IP" ON "DataOperateLogArchive"("Dlog_IP");
CREATE INDEX "DataOperateLogArchive_IX_AccID" ON "DataOperateLogArchive"("Acc_ID");
CREATE INDEX "DataOperateLogArchive_IX_ThID" ON "DataOperateLogArchive"("Th_ID");
CREATE INDEX "DataOperateLogArchive_IX_AcID" ON "DataOperateLogArchive"("Ac_ID");
CREATE INDEX "DataOperateLogArchive_IX_OrgID" ON "DataOperateLogArchive"("Org_ID");
CREATE INDEX "DataOperateLogArchive_IX_Module" ON "DataOperateLogArchive"("Dlog_Module");
CREATE INDEX "DataOperateLogArchive_IX_MMUID" ON "DataOperateLogArchive"("MM_UID");
CREATE INDEX "DataOperateLogArchive_IX_API" ON "DataOperateLogArchive"("Dlog_API");

--创建题库收藏的表，这里是收藏管理员的试题
CREATE TABLE "QuesCollect" (
    "Qcl_ID" BIGINT PRIMARY KEY DEFAULT 0,
    "Acc_ID" BIGINT NOT NULL DEFAULT 0,
    "Ques_ID" BIGINT NOT NULL DEFAULT 0,
    "Qcl_CrtTime" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
-- 创建索引
CREATE INDEX "QuesCollect_IX_AccID" ON "QuesCollect"("Acc_ID");
CREATE INDEX "QuesCollect_IX_QuesID" ON "QuesCollect"("Ques_ID");
CREATE INDEX "QuesCollect_IX_CrtTime" ON "QuesCollect"("Qcl_CrtTime");
