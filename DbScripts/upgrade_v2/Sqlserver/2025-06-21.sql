/*�Ƿ�����AI����,Ĭ��Ϊ�����ã���false*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'Cou_EnabledAI')
BEGIN
    ALTER TABLE Course ADD Cou_EnabledAI bit NOT NULL CONSTRAINT DF_Course_Cou_EnabledAI DEFAULT 0;
END

/*�Ƿ���ù��������ģ��,Ĭ��Ϊ0���������ģ��*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'Cou_AIType')
BEGIN
    ALTER TABLE Course ADD Cou_AIType int NOT NULL CONSTRAINT DF_Course_Cou_AIType DEFAULT 0;
END

/*���������ַ*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'Cou_AIAgent')
BEGIN
    ALTER TABLE Course ADD Cou_AIAgent VARCHAR(2000);
END

/*AI��ͨ��¼�����ӿγ�ID*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LlmRecords') AND name = 'Cou_ID')
BEGIN
    ALTER TABLE LlmRecords ADD Cou_ID bigint NOT NULL DEFAULT 0;
    CREATE INDEX LlmRecords_IX_Cou_ID ON LlmRecords(Cou_ID ASC);
END

/*֪ͨ����������ֶ�*/
ALTER TABLE Notice ADD No_Order int NOT NULL CONSTRAINT DF_Notice_No_Order DEFAULT 0
go
    /*���ù���������*/
    UPDATE t SET No_Order = subq.rn
    FROM Notice t
    INNER JOIN (SELECT No_Id, ROW_NUMBER() OVER (ORDER BY No_Id) AS rn FROM Notice) subq ON t.No_Id = subq.No_Id
go   
CREATE INDEX Notice_IX_No_Order ON Notice(No_Order DESC)
go

/*����ѧԱ�������ţ�֮ǰ��ż�����ظ�*/
UPDATE t SET Sts_Tax = subq.rn
FROM StudentSort t
INNER JOIN (
    SELECT Sts_ID, ROW_NUMBER() OVER (ORDER BY Sts_Tax ASC) AS rn
    FROM StudentSort
) subq ON t.Sts_ID = subq.Sts_ID;


/*�������µ������ֶ�*/
ALTER TABLE Article ADD Art_Order int NOT NULL CONSTRAINT DF_Article_Art_Order DEFAULT 0
go
/*�������µ������*/
    UPDATE t SET Art_Order = subq.rn
    FROM Article t
    INNER JOIN (
        SELECT Art_ID, ROW_NUMBER() OVER (ORDER BY Art_ID) AS rn
        FROM Article
    ) subq ON t.Art_ID = subq.Art_ID
    go
CREATE INDEX Article_IX_Art_Order ON Article(Art_Order DESC)