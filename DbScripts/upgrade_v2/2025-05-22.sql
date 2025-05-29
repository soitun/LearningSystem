
/*������Postgresql���ݿ������ű�*/

/*����רҵ��������������ǰ�㼶���������¼�*/
UPDATE "Subject"
SET "Sbj_QuesCount" = q.count
FROM (
    SELECT "Sbj_ID", COUNT(*) as count
    FROM "Questions"
    GROUP BY "Sbj_ID"
) as q
WHERE q."Sbj_ID" = "Subject"."Sbj_ID";

/*����רҵ�Ŀγ���������ǰ�㼶���������¼�*/
UPDATE "Subject"
SET "Sbj_CourseCount" = q.count
FROM (
    SELECT "Sbj_ID", COUNT(*) as count
    FROM "Course"
    GROUP BY "Sbj_ID"
) as q
WHERE q."Sbj_ID" = "Subject"."Sbj_ID";

/*����רҵ���Ծ���������ǰ�㼶���������¼�*/
UPDATE "Subject"
SET "Sbj_TestCount" = q.count
FROM (
    SELECT "Sbj_ID", COUNT(*) as count
    FROM "TestPaper"
    GROUP BY "Sbj_ID"
) as q
WHERE q."Sbj_ID" = "Subject"."Sbj_ID";



/*�����½ڵ�������������ǰ�㼶���������¼�*/
UPDATE "Outline"
SET "Ol_QuesCount" = q.count
FROM (
    SELECT "Ol_ID", COUNT(*) as count
    FROM "Questions"
    GROUP BY "Ol_ID"
) as q
WHERE q."Ol_ID" = "Outline"."Ol_ID";

/*����γ̵�������*/
UPDATE "Course"
SET "Cou_QuesCount" = q.count
FROM (
    SELECT "Cou_ID", COUNT(*) as count
    FROM "Questions"
    GROUP BY "Cou_ID"
) as q
WHERE q."Cou_ID" = "Course"."Cou_ID";


/* ����������Ѷȵȼ�����֪��Ϊʲô�����ѶȻ���ִ���5����� */
UPDATE "Questions" SET "Qus_Diff"=5  WHERE "Qus_Diff">=5;
UPDATE "Questions" SET "Qus_Diff"=1  WHERE "Qus_Diff"<=1;




/*������Sqlserver�����ű�*/

-- ����רҵ��������������ǰ�㼶���������¼�
UPDATE s
SET s.Sbj_QuesCount = q.count
FROM Subject s
INNER JOIN (
    SELECT Sbj_ID, COUNT(*) as count
    FROM Questions
    GROUP BY Sbj_ID
) q ON q.Sbj_ID = s.Sbj_ID;

-- ����רҵ�Ŀγ���������ǰ�㼶���������¼�
UPDATE s
SET s.Sbj_CourseCount = q.count
FROM Subject s
INNER JOIN (
    SELECT Sbj_ID, COUNT(*) as count
    FROM Course
    GROUP BY Sbj_ID
) q ON q.Sbj_ID = s.Sbj_ID;

-- ����רҵ���Ծ���������ǰ�㼶���������¼�
UPDATE s
SET s.Sbj_TestCount = q.count
FROM Subject s
INNER JOIN (
    SELECT Sbj_ID, COUNT(*) as count
    FROM TestPaper
    GROUP BY Sbj_ID
) q ON q.Sbj_ID = s.Sbj_ID;

-- �����½ڵ�������������ǰ�㼶���������¼�
UPDATE o
SET o.Ol_QuesCount = q.count
FROM Outline o
INNER JOIN (
    SELECT Ol_ID, COUNT(*) as count
    FROM Questions
    GROUP BY Ol_ID
) q ON q.Ol_ID = o.Ol_ID;

-- ����γ̵�������
UPDATE c
SET c.Cou_QuesCount = q.count
FROM Course c
INNER JOIN (
    SELECT Cou_ID, COUNT(*) as count
    FROM Questions
    GROUP BY Cou_ID
) q ON q.Cou_ID = c.Cou_ID;

-- ����������Ѷȵȼ�����֪��Ϊʲô�����ѶȻ���ִ���5�����
UPDATE Questions SET Qus_Diff = 5 WHERE Qus_Diff >= 5;
UPDATE Questions SET Qus_Diff = 1 WHERE Qus_Diff <= 1;
