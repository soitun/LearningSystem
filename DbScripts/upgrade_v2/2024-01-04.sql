
/*����ѧԱ���߼�¼��صĵ�����Ϣ*/
--ʡ���У����أ�
alter table [LogForStudentOnline] add Lso_Province [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_City [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_District [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_Source [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_Info [nvarchar](255) NULL
go
--������������
alter table [LogForStudentOnline] add Lso_Code int NULL
go
update [LogForStudentOnline] set Lso_Code=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Code int NOT NULL
go
--����
alter table [LogForStudentOnline] add Lso_Longitude decimal(20, 15) NULL
go
update [LogForStudentOnline] set Lso_Longitude=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Longitude decimal(20, 15) NOT NULL
go
--γ��
alter table [LogForStudentOnline] add Lso_Latitude decimal(20, 15) NULL
go
update [LogForStudentOnline] set Lso_Latitude=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Latitude decimal(20, 15) NOT NULL
go
--���ݻ�ȡ��ʽ��GPS��IP����Ĭ��Ϊ0��IP��ʽΪ1
alter table [LogForStudentOnline] add Lso_GeogType [int] NULL
go
update [LogForStudentOnline] set Lso_GeogType=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_GeogType [int] NOT NULL



/*������ϰ��¼�����ӵ�����Ϣ�ֶ�*/
alter table LogForStudentExercise drop column Lse_UID
go
alter table LogForStudentExercise add Lse_GeogData nvarchar(max) NULL
go
/*��Ƶѧϰ��¼�����ӵ�����Ϣ�ֶ�*/
alter table LogForStudentStudy add Lss_GeogData nvarchar(max) NULL
go

alter table [Columns] ALTER COLUMN Col_IsChildren [bit] NOT NULL