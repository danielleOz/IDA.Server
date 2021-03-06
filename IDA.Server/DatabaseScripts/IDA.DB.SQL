
use [master] 
IF DB_ID('IDA.DB') IS NOT NULL
    drop database [ida.db]
go

create Database [IDA.DB]
go

Use [IDA.DB]
Go 

CREATE TABLE "User"(
    Id int identity primary key NOT NULL,
    "Email" NVARCHAR(255) NOT NULL,
    "FirstName" NVARCHAR(255) NOT NULL,
    "LastName" NVARCHAR(255) NOT NULL,
    "UserPswd" NVARCHAR(255) NOT NULL,
    "City" NVARCHAR(255) NOT NULL,
    "Street" NVARCHAR(255) NOT NULL,
    "Apartment" NVARCHAR(255) NOT NULL,
    "HouseNumber" NVARCHAR(255) NOT NULL,
    "Birthday" DATETIME NOT NULL,
    "IsWorker" BIT NOT NULL
);

ALTER TABLE
    "User" ADD CONSTRAINT "user_email_unique" UNIQUE ("Email");

CREATE TABLE "Worker"(
    "Id" INT primary key NOT NULL,
    "RadiusKM" FLOAT NOT NULL,
    "AvailbleUntil" DATETIME  NOT NULL


);


CREATE TABLE "Service"(
    "Id" INT IDENTITY(1,1) primary key NOT NULL,
    "Name" NVARCHAR(255) NOT NULL
);

CREATE TABLE "WorkerService"(
    "ServiceId" INT NOT NULL,
    "WorkerId" INT NOT NULL,
);

ALTER TABLE
    "WorkerService" ADD CONSTRAINT "WorkerService_swid_primary" PRIMARY KEY(ServiceId, WorkerId );

CREATE TABLE "JobOffer"(
    "Id" INT IDENTITY(1,1) primary key NOT NULL,
    ServiceId int NOT NULL,
    "ChosenWorkerId" INT NULL,
    "UserId" INT NOT NULL,
    "PublishDate" DATETIME NOT NULL default(GETDATE()),
    "StatusId" INT NOT NULL,
    "Description" NVARCHAR(500) NOT NULL,
    WorkerReviewDescriptipon NVARCHAR(500) NULL,
    WorkerReviewRate INT NULL,
    WorkerReviewDate DATETIME NULL
);

CREATE TABLE "JobOfferStatus"(
    "Id" INT primary key NOT NULL,
    "Description" NVARCHAR(255) NOT NULL
);

INSERT INTO JobOfferStatus VALUES (0, 'Waiting')
INSERT INTO JobOfferStatus VALUES (1, 'Approved')
INSERT INTO JobOfferStatus VALUES (2, 'Finished')



CREATE TABLE ChatMessage (
  Id int identity primary key not null,
  MessageText NVARCHAR(500) not null,
  MessageDate DATETIME NOT NULL default(GETDATE()),
  SenderID int not null,
  RecieverID int not null
);


ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_uid_foreign" FOREIGN KEY("UserId") REFERENCES "User"("Id");
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_wid_foreign" FOREIGN KEY("ChosenWorkerId") REFERENCES "Worker"("Id");
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_sid_foreign" FOREIGN KEY("StatusId") REFERENCES "JobOfferStatus"("Id");
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_serviceid_foreign" FOREIGN KEY("ServiceId") REFERENCES "Service"("Id");

ALTER TABLE
    "Worker" ADD CONSTRAINT "Worker_uid_foreign" FOREIGN KEY("Id") REFERENCES "User"("Id");

ALTER TABLE
    "ChatMessage" ADD CONSTRAINT "ChatMessage_uid_foreign" FOREIGN KEY("SenderId") REFERENCES "User"("Id");
ALTER TABLE
    "ChatMessage" ADD CONSTRAINT "ChatMessage_wid_foreign" FOREIGN KEY("RecieverId") REFERENCES "User"("Id");

ALTER TABLE
    "WorkerService" ADD CONSTRAINT "WorkerService_sid_foreign" FOREIGN KEY("ServiceId") REFERENCES "Service"("Id");
ALTER TABLE
    "WorkerService" ADD CONSTRAINT "WorkerService_wid_foreign" FOREIGN KEY("WorkerId") REFERENCES "Worker"("Id");

GO

INSERT INTO [dbo].[User]
           ([Email]
           ,[FirstName]
           ,[LastName]
           ,[UserPswd]
           ,[City]
           ,[Apartment]
           ,[Street]
           ,[HouseNumber]
           ,[Birthday]
           ,[IsWorker])
     VALUES
           ('danielle.oz@gmail.com'
           ,'danielle'
           ,'oz'
           ,'123456'
           ,'hod hasharon'
           ,'8'
           ,'haBavli'
           ,'1'
           ,'1978-02-21'
           ,'false')
GO


INSERT INTO [dbo].[User]
           ([Email]
           ,[FirstName]
           ,[LastName]
           ,[UserPswd]
           ,[City]
           ,[Apartment]
           ,[Street]
           ,[HouseNumber]
           ,[Birthday]
           ,[IsWorker])
     VALUES
           ('danielle.oz.do@gmail.com'
           ,'www'
           ,'www'
           ,'123456'
           ,'hod hasharon'
           ,'8'
           ,'haBavli'
           ,'19'
           ,'1978-02-21'
           ,'true')
GO

INSERT INTO [dbo].[Worker]
           ([id]
           ,[RadiusKM]
           ,[AvailbleUntil])
 
     VALUES
           (2
           ,2
           ,'1978-02-21')
   
GO



INSERT INTO [dbo].[Service]
           ([Name])
     VALUES
           ('Cleaning'),

		   ('Laundry'),

		   ('Car Washing'),

		   ('Cooking'),
           ('Dog Walking')

GO

INSERT INTO [dbo].[JobOffer]
           (
           [ServiceId],
           [ChosenWorkerId],
           [UserId], 
           [StatusId], 
           [PublishDate],
           [Description],
           [WorkerReviewDescriptipon], 
           [WorkerReviewDate], 
           [WorkerReviewRate]
           )
         
     VALUES
          ( 1,
           2,
           1,
           2,
           '1978-02-21',
           'rrr',
           'was very good',
           '1978-02-22',
           5 )


GO

UPDATE [dbo].[Worker]
           
           Set [AvailbleUntil]='2022-07-29 17:00:00', RadiusKM=2
         where Id=2  
         

GO

INSERT INTO [WorkerService]

(
[WorkerId],
[ServiceId])

VALUES
(
2,
1)

INSERT INTO [dbo].[User]
           ([Email]
           ,[FirstName]
           ,[LastName]
           ,[UserPswd]
           ,[City]
           ,[Apartment]
           ,[Street]
           ,[HouseNumber]
           ,[Birthday]
           ,[IsWorker])
     VALUES
           ('worker@worker.com'
           ,'worker'
           ,'worker'
           ,'123456'
           ,'tel aviv - yafo'
           ,'8'
           ,'Tel Baruch'
           ,'19'
           ,'1978-02-21'
           ,'true')
GO

INSERT INTO [dbo].[Worker]
           ([id]
           ,[RadiusKM]
           ,[AvailbleUntil])
 
     VALUES
           (3
           ,2
           ,'1978-02-21')
   
GO

INSERT INTO [WorkerService]

(
[WorkerId],
[ServiceId])

VALUES
(
3,
2)


INSERT INTO [dbo].[JobOffer]
           (
           [ServiceId],
           [ChosenWorkerId],
           [UserId], 
           [StatusId], 
           [PublishDate],
           [Description],
           [WorkerReviewDescriptipon], 
           [WorkerReviewDate], 
           [WorkerReviewRate]
           )
         
     VALUES
          ( 2,
           3,
           1,
           2,
           '2022-02-21',
           'g',
           ' was ok',
           '2022-02-22',
           4 )


GO



UPDATE [dbo].[Worker]
           
           Set [AvailbleUntil]='2022-07-29 17:00:00', RadiusKM=2
         where Id=3  
         

GO





