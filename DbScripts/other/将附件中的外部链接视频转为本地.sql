declare cur cursor scroll
for select As_Id, As_FileName from Accessory where As_Type='CourseVideo' and As_FileName like 'http%'
open cur
declare @asid int,@file nvarchar(3000),@lastIndex int;
--set @orgid=0
fetch First from cur into @asid,@file
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
   SELECT @lastIndex = CHARINDEX('/', REVERSE(@file));
   SELECT @file=SUBSTRING(@file, LEN(@file) - @lastIndex + 2, LEN(@file))
   print @file
   --print @orgid
   fetch next from cur into @asid,@file   --�ƶ��α�
 end   
--�رղ��ͷ��α�
close cur
deallocate cur