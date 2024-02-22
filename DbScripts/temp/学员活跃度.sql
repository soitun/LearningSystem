

--select *, ROW_NUMBER() OVER( ORDER BY acc.ac_id desc ) AS rowid from Accounts 
--where 1=1 and 

select *  from (
	select acc.Ac_ID,Ac_Name,Ac_AccName,Ac_Photo,Ac_IDCardNumber,Ac_MobiTel1,Ac_LastTime,Sts_ID,Sts_Name
		,logincount,logintime
		,coursecount,rechargecount,laststudy,lastexrcise,lasttest,lastexam
		,ROW_NUMBER() OVER( ORDER BY coursecount desc ) AS rowid from Accounts as acc
	left join  --��¼����������¼ʱ��
	(select Ac_id, COUNT(*) as 'logincount', max(Lso_CrtTime) as 'logintime' from LogForStudentOnline group by Ac_ID) as ol
		on acc.Ac_ID=ol.Ac_id
	left join --�γ̹������
	(select Ac_id, COUNT(*) as 'coursecount' from Student_Course group by Ac_ID) as buy
		on acc.Ac_ID=buy.Ac_id
	left join ----�ʽ���
	(select Ac_id, COUNT(*) as 'rechargecount',max(Ma_CrtTime) as 'lastrecharge'  from MoneyAccount group by Ac_ID) as recharge
		on acc.Ac_ID=recharge.Ac_ID			
	left join --��Ƶѧϰ��¼
	(select Ac_id, max(Lss_LastTime) as 'laststudy' from LogForStudentStudy group by Ac_ID) as video
		on acc.Ac_ID=video.Ac_ID
	left join --������ϰ��¼
	(select Ac_id, max(Lse_LastTime) as 'lastexrcise' from LogForStudentExercise group by Ac_ID) as ques
		on acc.Ac_ID=ques.Ac_ID
	left join --���Գɼ�
	(select Ac_id, max(Tr_CrtTime) as 'lasttest' from TestResults group by Ac_ID) as test
		on acc.Ac_ID=test.Ac_ID
	left join --���Գɼ�
	(select Ac_id, max(Exr_CrtTime) as 'lastexam' from ExamResults group by Ac_ID) as exam
		on acc.Ac_ID=exam.Ac_ID
	--��ѯ����
	where Ac_AccName like '%41%' and Ac_Name like '%��%' and Sts_ID=3058
	and Ac_MobiTel1 like '%1%'  and Ac_IDCardNumber like '%41%'
	--and Ac_CodeNumber like '%41%'  	
) as res where rowid BETWEEN 1 AND 20

--order by coursecount desc
--select * from LogForStudentOnline