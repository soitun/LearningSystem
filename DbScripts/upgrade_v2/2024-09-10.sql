 
 
/* ѧϰ������������ⳤ�ȱ��Ϊ200��ԭ����500��Ϊ�˷������� */

--SQLserver�ű�
alter table "LearningCardSet" ALTER COLUMN Lcs_Theme [nvarchar](200) NULL
go

--PostgreSQL�ű�
alter table "Student_Course" add "Stc_ResultScore" real NULL;
update "Student_Course" set "Stc_ResultScore"=0;
alter table "Student_Course" ALTER COLUMN "Stc_ResultScore" set NOT NULL;