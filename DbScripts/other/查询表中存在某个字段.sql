declare cursor_obj  cursor scroll
for select name from sysobjects where type='U'
open cursor_obj
declare @name nvarchar(500),@count int,@tatol int 
select @count=0,@tatol=0
fetch First from cursor_obj into @name
while @@fetch_status=0  
 begin    
    --select @count=COUNT(*) from syscolumns Where ID=OBJECT_ID(@name) and name='Cou_UID'
    select @count=COUNT(*) from syscolumns Where ID=OBJECT_ID(@name) and name like '%Qus_Number%'
    if @count>0
	   begin
		 set @tatol=@tatol+1;
		 print @name
	   end
   fetch next from cursor_obj into @name  --�ƶ��α�
 end   
 print @tatol
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj

/*
/*��ѯ���*/
SELECT idx.*,idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) 
where tab.name='TestPaper'

/*��ѯԼ�������*/
SELECT idx.* FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
where tab.name='TestPaper'

SELECT idx.name as 'df',tab.name as 'tb' FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
where idx.type='d'

SELECT * FROM dbo.sysobjects WHERE type = 'D' and id='213575799'

select * from sys.tables 

SELECT * FROM dbo.sysobjects where parent_obj='213575799'

*/