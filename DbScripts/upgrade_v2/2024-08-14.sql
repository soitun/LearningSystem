 
 
/* ѧϰ��¼�е��ۺϳɼ� */

--SQLserver�ű�
alter table "Student_Course" add "Stc_ResultScore" float NULL
go
update "Student_Course" set "Stc_ResultScore"=0 where "Stc_ResultScore" is null
go
alter table "Student_Course" ALTER COLUMN "Stc_ResultScore" float NOT NULL
go

--PostgreSQL�ű�
alter table "Student_Course" add "Stc_ResultScore" real NULL;
update "Student_Course" set "Stc_ResultScore"=0;
alter table "Student_Course" ALTER COLUMN "Stc_ResultScore" set NOT NULL;