/*
	��Sqlserver�����������ɵ�PostgreSQL
	
	��д˼·��
	�����α����Sqlserver�е�������������PostgreSQL�п�ִ�еĴ���������sql�ű�
	
	�������ۣ�
	1����Sqlserver��ִ������Sql�ű�
	2���ڡ���Ϣ�����и��ƴ���������Sql�ű�
	3�������������Ľű���ճ����PostgreSql��ִ�м���
	
*/

-- ����Postgresql�������������
CREATE FUNCTION dbo.get_index_sql
(
    @tablename nvarchar(100),		--����
    @indexname nvarchar(200),		--������
    @indexcount int					--ͬ�����м�������������Ǹ������������1
)
RETURNS nvarchar(1000)
AS
BEGIN
    DECLARE @result nvarchar(1000);
    DECLARE @descending int;	--����ʽ,0Ϊ����1Ϊ����
    DECLARE @columnName nvarchar(100);	--��������
    DECLARE @descending_key NVARCHAR(10);
	DECLARE @num int;		--��ʱ����
    --set @result=@indexname +' : '+ CONVERT(nvarchar(20),@indexcount)
    set @num=0;
    
    if @indexcount<=1
    BEGIN	--����ǵ���������
		SELECT  @descending=ic.is_descending_key,
				@columnName=c.name
					FROM 
						sys.indexes i
					JOIN 
						sys.index_columns ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id
					JOIN 
						sys.columns c ON ic.column_id = c.column_id AND ic.object_id = c.object_id
					WHERE 
						i.type_desc='NONCLUSTERED'  and OBJECT_NAME(i.object_id) = @tablename and i.name=@indexname
		IF @descending=1
		BEGIN
			set @descending_key='ASC'					
		END ELSE IF @descending=0
		BEGIN
			set @descending_key='DESC'
		END
		set @result='CREATE INDEX IF NOT EXISTS "'+@tablename+'_'+@indexname+'" ON "'+@tablename+'" ("'+@columnName+'" '+@descending_key+');'
	END ELSE	
	BEGIN  --�����Ǹ�������
		
			set @result='CREATE INDEX IF NOT EXISTS "'+@tablename+'_'+@indexname+'" ON "'+@tablename+'" (';			
			
			DECLARE inner_cursor CURSOR FOR
			select ic.is_descending_key, c.name FROM 
						sys.indexes i
					JOIN 
						sys.index_columns ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id
					JOIN 
						sys.columns c ON ic.column_id = c.column_id AND ic.object_id = c.object_id
					WHERE 
						i.type_desc='NONCLUSTERED'  and OBJECT_NAME(i.object_id) = @tablename and i.name=@indexname
			OPEN inner_cursor;
			--DECLARE @indexname NVARCHAR(160),@indexCount int
			FETCH NEXT FROM inner_cursor INTO @descending, @columnName
			while @@fetch_status=0  
				begin    
					set @num=@num+1;					
					IF @descending=1
					BEGIN
						set @descending_key='ASC'					
					END ELSE IF @descending=0
					BEGIN
						set @descending_key='DESC'
					END
					set @result=@result+'"'+@columnName +'" '+@descending_key
					IF @num<@indexcount
					BEGIN
						set @result=@result +','				
					END
					fetch next from inner_cursor into  @descending, @columnName
				end
			close inner_cursor			
			deallocate inner_cursor	
			set @result=@result +');'
	END
    RETURN @result;
END
go

--��������������PostgreSql�Ĵ��������Ľű��������˸�������
DECLARE  cursor_obj CURSOR SCROLL 
	FOR select name from sysobjects where type='U' order by name
open cursor_obj
DECLARE  @tablename nvarchar(500),@count int,@tatol int,@sql nvarchar(1000)
select @count=0,@tatol=0
FETCH First from cursor_obj into @tablename
WHILE @@fetch_status=0  
 BEGIN     
	 set @count=@count+1;
     print '-- '+CONVERT(nvarchar(20),@count) + ' . '+ @tablename
		--Ƕ���α� -- start
		DECLARE inner_cursor CURSOR FOR
					SELECT 
						i.name AS IndexName,COUNT(ic.index_column_id) as 'count'
					FROM 
						sys.indexes i
					JOIN 
						sys.index_columns ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id
					JOIN 
						sys.columns c ON ic.column_id = c.column_id AND ic.object_id = c.object_id
					WHERE 
						i.type_desc='NONCLUSTERED'  and OBJECT_NAME(i.object_id) =  @tablename
					GROUP BY 
						i.object_id, i.name, i.type_desc
		OPEN inner_cursor;
		DECLARE @indexName NVARCHAR(160),@indexCount int
		FETCH NEXT FROM inner_cursor INTO @indexName, @indexCount
		while @@fetch_status=0  
			begin    
				set @tatol=@tatol+1;
				select @sql= dbo.get_index_sql(@tablename,@indexName,@indexCount);
				print @sql
				
				fetch next from inner_cursor into  @indexName,@indexCount
			end
		close inner_cursor
		deallocate inner_cursor					
		--Ƕ���α� --end
   
   FETCH next from cursor_obj into @tablename  --�ƶ��α�
 END   
 print '--�ܹ������У�'+CONVERT(nvarchar(20),@tatol) 
--�رղ��ͷ��α�
CLOSE cursor_obj
DEALLOCATE cursor_obj

go
DROP FUNCTION dbo.get_index_sql;
