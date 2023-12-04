
declare @snow_int_id bigint
set @snow_int_id=108326701601394689

/*��רҵ�������е�Sbj_IDתΪѩ��id*/
declare cursor_obj  cursor scroll
for SELECT Sbj_ID FROM [Subject] where Sbj_ID<5000000 order by Sbj_ID
open cursor_obj
declare @sbjid bigint, @snowsbj bigint
set @snowsbj=@snow_int_id + 1000000
fetch First from cursor_obj into @sbjid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
	set @snowsbj=@snowsbj+1
	Update [Subject] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid	
	Update [Subject] Set Sbj_PID=@snowsbj Where Sbj_PID=@sbjid
	Update [TestResults] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [TestPaper] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [ExamResults] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Examination] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Student_Ques] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Student_Collect] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Course] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Questions] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Outline] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid

	fetch next from cursor_obj into @sbjid
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj


/*���γ̹������е�Cou_IDתΪѩ��id*/
declare cursor_obj  cursor scroll
for SELECT Cou_ID FROM Course where Cou_ID<5000000 order by Cou_ID
open cursor_obj
declare @couid bigint, @snowid bigint
set @snowid=@snow_int_id + 2000000
fetch First from cursor_obj into @couid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
	set @snowid=@snowid+1
	---print @snowid
	Update Course Set Cou_ID=@snowid Where Cou_ID=@couid
	
	Update [TestResults] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [TestPaper] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [KnowledgeSort] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Knowledge] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [GuideColumns] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [Guide] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Teacher_Course] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Student_Ques] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Student_Notes] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Student_Course] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [Student_Collect] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [CoursePrice] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [QuesTypes] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Questions] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [OutlineEvent] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [Outline] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [MessageBoard] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Message] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [LogForStudentStudy] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [LogForStudentQuestions] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [LogForStudentExercise] Set Cou_ID=@snowid Where Cou_ID=@couid	
	Update [StudentSort_Course] Set Cou_ID=@snowid Where Cou_ID=@couid
	
	fetch next from cursor_obj into @couid
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj



/*���½ڹ������е�Ol_IDתΪѩ��id*/
declare cursor_obj  cursor scroll
for SELECT Ol_ID FROM outline where Ol_ID<5000000 order by Ol_ID
open cursor_obj
declare @olid bigint, @snowol bigint
set @snowol=@snow_int_id + 3000000
fetch First from cursor_obj into @olid
while @@fetch_status=0  --��ȡ�ɹ���������һ�����ݵ���ȡ���� 
 begin
	set @snowol=@snowol+1
	---print @snowid
	Update outline Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update outline Set Ol_PID=@snowol Where Ol_PID=@olid	
	Update [TestPaperItem] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [Questions] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [OutlineEvent] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [Message] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [LogForStudentStudy] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [LogForStudentQuestions] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [LogForStudentExercise] Set Ol_ID=@snowol Where Ol_ID=@olid	
	fetch next from cursor_obj into @olid
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj




/*���Ծ�������е�Tp_IdתΪѩ��id*/
declare cursor_obj  cursor scroll
for SELECT Tp_Id FROM TestPaper where Tp_Id<5000000 order by Tp_Id
open cursor_obj
declare @tpid bigint, @snowtp bigint
set @snowtp=@snow_int_id + 4000000
fetch First from cursor_obj into @tpid
while @@fetch_status=0 
 begin
	set @snowtp=@snowtp+1
	Update TestPaper Set Tp_Id=@snowtp Where Tp_Id=@tpid
	Update [TestPaperQues] Set Tp_Id=@snowtp Where Tp_Id=@tpid
	Update [TestResults] Set Tp_Id=@snowtp Where Tp_Id=@tpid
	Update [ExamResults] Set Tp_Id=@snowtp Where Tp_Id=@tpid
	Update [Examination] Set Tp_Id=@snowtp Where Tp_Id=@tpid
	
	fetch next from cursor_obj into @tpid
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj


/*���ⲿ�ֲ�Ҫִ�У����֮ǰ�Ŀ��Գɼ��ع˻�������

/*����������е��ֶ�Qus_IDתΪѩ��id*/
declare cursor_obj  cursor scroll
for SELECT Qus_ID FROM Questions where Qus_ID<5000000 order by Qus_ID
open cursor_obj
declare @qid bigint, @snowqs bigint
set @snowqs=@snow_int_id + 5000000
fetch First from cursor_obj into @qid
while @@fetch_status=0 
 begin
	set @snowqs=@snowqs+1
	Update Questions Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update Student_Ques Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update [Student_Notes] Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update [Student_Collect] Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update [QuesAnswer] Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update [LogForStudentQuestions] Set Qus_ID=@snowqs Where Qus_ID=@qid
	
	fetch next from cursor_obj into @qid
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj
*/

/*ѧԱ����ֶ�Sts_IDתΪѩ��id*/
declare cursor_obj  cursor scroll
for SELECT Sts_ID FROM [StudentSort] where Sts_ID<5000000 order by Sts_ID
open cursor_obj
declare @stsid bigint, @snowsts bigint
set @snowsts=@snow_int_id + 6000000
fetch First from cursor_obj into @stsid
while @@fetch_status=0 
 begin
	set @snowsts=@snowsts+1
	Update [StudentSort] Set Sts_ID=@snowsts Where Sts_ID=@stsid
	Update [TestResults] Set Sts_ID=@snowsts Where Sts_ID=@stsid
	Update [ExamResults] Set Sts_ID=@snowsts Where Sts_ID=@stsid
	Update [ExamGroup] Set Sts_ID=@snowsts Where Sts_ID=@stsid	
	Update [Accounts] Set Sts_ID=@snowsts Where Sts_ID=@stsid	
	fetch next from cursor_obj into @stsid
 end   
--�رղ��ͷ��α�
close cursor_obj
deallocate cursor_obj


