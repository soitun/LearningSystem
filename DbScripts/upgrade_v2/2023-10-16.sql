

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