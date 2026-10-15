/* Run this script while the selected database is model. */
IF OBJECT_ID('dbo.Groups', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Groups (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Groups PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL CONSTRAINT UQ_Groups_Name UNIQUE
    );
END;

IF OBJECT_ID('dbo.Students', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Students PRIMARY KEY,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Age INT NOT NULL CONSTRAINT CK_Students_Age CHECK (Age BETWEEN 1 AND 120),
        GroupId INT NOT NULL CONSTRAINT FK_Students_Groups FOREIGN KEY REFERENCES dbo.Groups(Id)
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Groups WHERE Name = N'ИТ-101')
    INSERT dbo.Groups(Name) VALUES (N'ИТ-101');
IF NOT EXISTS (SELECT 1 FROM dbo.Groups WHERE Name = N'ИТ-102')
    INSERT dbo.Groups(Name) VALUES (N'ИТ-102');
