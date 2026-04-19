/*将考试的ID转为雪花ID*/
ALTER TABLE "ExamResults" ALTER COLUMN "Exam_ID" TYPE BIGINT;
ALTER TABLE "Examination"  ADD COLUMN "new_id" BIGINT;
UPDATE "Examination"  set "new_id"="Exam_ID";
ALTER TABLE "Examination" ALTER COLUMN "Exam_ID" DROP DEFAULT;
DROP SEQUENCE IF EXISTS "Examination_Exam_ID_seq";
ALTER TABLE "Examination" DROP CONSTRAINT IF EXISTS "key_examination";
ALTER TABLE "Examination" ALTER COLUMN "Exam_ID" DROP IDENTITY IF EXISTS;
-- 删除旧的 Exam_ID 列
ALTER TABLE "Examination" DROP COLUMN "Exam_ID";
-- 将 new_id 重命名为 Exam_ID
ALTER TABLE "Examination" RENAME COLUMN "new_id" TO "Exam_ID";
-- 生新设置Exam_ID为主键
ALTER TABLE "Examination" ADD PRIMARY KEY ("Exam_ID");