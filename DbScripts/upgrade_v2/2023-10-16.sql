

--�޸Ļ������ֶ�
--������ͳһ������ô��루Unified Social Credit Identifier, USCI��
EXEC sp_rename 'Organization.Org_License', 'Org_USCI', 'COLUMN';
go
EXEC sp_rename 'Organization.Org_Extracode', 'Org_ExtraWeb', 'COLUMN';
EXEC sp_rename 'Organization.Org_LicensePic', 'Org_ExtraMobi', 'COLUMN';
--�����Ӵ����Ϊweb����mobi��
ALTER TABLE Organization ALTER COLUMN Org_ExtraWeb nvarchar(max) null
ALTER TABLE Organization ALTER COLUMN Org_ExtraMobi nvarchar(max) null
go
--ɾ��������ά����ֶΣ����ڲ���js��ǰ��ʵ����
ALTER TABLE Organization DROP COLUMN Org_QrCode;
ALTER TABLE Organization DROP COLUMN Org_QrCodeUrl;

--ͼ��ϵͳ�˵���ͼ����
EXEC sp_rename 'ManageMenu.MM_IcoS', 'MM_IcoCode', 'COLUMN';
ALTER TABLE ManageMenu DROP COLUMN MM_IcoB;
go
alter table [ManageMenu] add MM_IcoSize int NULL
go
update [ManageMenu] set MM_IcoSize=0
go
alter table [ManageMenu] ALTER COLUMN MM_IcoSize int not null

go
alter table [ManageMenu] add MM_IcoColor  nvarchar(100) null
go
ALTER TABLE [ManageMenu] ALTER COLUMN MM_IcoCode nvarchar(50) null

--�������е�flv��Ƶ����Ϊmp4��flv�Ѿ�����ʹ��
 Update Accessory Set as_filename=replace(as_filename,'.flv','.mp4') 
 where as_type='CourseVideo' and as_filename like '%.flv'
 --�γ̹������ӿγ����͵��ֶ�
 alter table [Course] add MM_IcoSize int NULL
go
update [Course] set MM_IcoSize=0
go
alter table [Course] ALTER COLUMN MM_IcoSize int not null