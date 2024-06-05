/*��ȡ���б�*/
select name,crdate from sysobjects where type='U' order by name asc

select * from sysobjects

/* ����Ϣ */
SELECT [TableName] = [Tables].name ,
        [TableOwner] = [Schemas].name ,
        [TableCreateDate] = [Tables].create_date ,
        [TableModifyDate] = [Tables].modify_date
FROM    sys.tables AS [Tables]
        INNER JOIN sys.schemas AS [Schemas] ON [Tables].schema_id = [Schemas].schema_id
WHERE   [Tables].name = 'Course'

 /*��ѯ������*/
 SELECT distinct
	TABLE_NAME,
	COLUMN_NAME=stuff((
		SELECT '|'+COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		where TABLE_NAME ='Article'
    FOR XML path('')
    ), 1, 1, '')
FROM
	INFORMATION_SCHEMA.KEY_COLUMN_USAGE
where TABLE_NAME ='Article'


/**����ֶ���Ϣ�������������ͣ�����*/
SELECT name,type_name(xtype) AS type,
--���ȣ����޳�Ϊ-1
length,(type_name(xtype)+'('+CONVERT(varchar,length)+')') as fulltype,
--�Ƿ�ɿգ�0Ϊ�ɿ�
isnullable as nullable
FROM syscolumns
WHERE (id = OBJECT_ID('questions'))
order by name asc

/*��ѯĳ���������͵��ֶ�*/
SELECT 
    TABLE_NAME,
    COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE DATA_TYPE = 'float'