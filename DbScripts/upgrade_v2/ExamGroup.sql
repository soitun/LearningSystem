/*
 Navicat Premium Dump SQL

 Source Server         : postgreSQL
 Source Server Type    : PostgreSQL
 Source Server Version : 160003 (160003)
 Source Host           : localhost:5432
 Source Catalog        : gxmk
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 160003 (160003)
 File Encoding         : 65001

 Date: 24/12/2025 21:17:58
*/


-- ----------------------------
-- Table structure for ExamGroup
-- ----------------------------
DROP TABLE IF EXISTS "public"."ExamGroup";
CREATE TABLE "public"."ExamGroup" (
  "Eg_ID" int4 NOT NULL DEFAULT nextval('"ExamGroup_Eg_ID_seq"'::regclass),
  "Eg_Type" int4,
  "Exam_UID" varchar(255) COLLATE "pg_catalog"."default",
  "Org_ID" int4 NOT NULL,
  "Sts_ID" int8 NOT NULL
)
;

-- ----------------------------
-- Indexes structure for table ExamGroup
-- ----------------------------
CREATE INDEX "ExamGroup_IX_Exam_UID" ON "public"."ExamGroup" USING btree (
  "Exam_UID" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "ExamGroup_IX_Sts_ID" ON "public"."ExamGroup" USING btree (
  "Sts_ID" "pg_catalog"."int8_ops" ASC NULLS LAST
);
CREATE INDEX "aaaaaExamGroup_PK" ON "public"."ExamGroup" USING btree (
  "Eg_ID" "pg_catalog"."int4_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table ExamGroup
-- ----------------------------
ALTER TABLE "public"."ExamGroup" ADD CONSTRAINT "key_examgroup" PRIMARY KEY ("Eg_ID");
