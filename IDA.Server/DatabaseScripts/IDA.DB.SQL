use [master] 
create Database [IDA.DB]

Use [IDA.DB]
Go

CREATE TABLE "User"(
    "UserName" NVARCHAR(255) NOT NULL,
    "Email" NVARCHAR(255) NOT NULL,
    "FirstName" NVARCHAR(255) NOT NULL,
    "LastName" NVARCHAR(255) NOT NULL,
    "UserPswd" NVARCHAR(255) NOT NULL,
    "Adress" NVARCHAR(255) NOT NULL,
    "Birthday" DATETIME NOT NULL,
    "IsWorker" BIT NOT NULL
   
);
ALTER TABLE
    "User" ADD CONSTRAINT "user_username_primary" PRIMARY KEY("UserName");

CREATE TABLE "Customer"(
    "CId" INT IDENTITY(1,1) NOT NULL,
    "UserName" NVARCHAR(255) NOT NULL,
  
);
ALTER TABLE
    "Customer" ADD CONSTRAINT "Customer_cid_primary" PRIMARY KEY("CId");
CREATE TABLE "Worker"(
    "WId" INT IDENTITY(1,1) NOT NULL,
    "UserName" NVARCHAR(255) NOT NULL,
    "LId" INT NOT NULL,
    "Service" NVARCHAR (255) NOT NULL,
    "Location" FLOAT NOT NULL,
     "Availble" BIT NOT NULL
);
ALTER TABLE
    "Worker" ADD CONSTRAINT "Worker_wid_primary" PRIMARY KEY("WId");
    
CREATE TABLE "Service"(
    "SId" INT IDENTITY(1,1) NOT NULL,
    "Name" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Service" ADD CONSTRAINT "Service_sid_primary" PRIMARY KEY("SId");
CREATE TABLE "WorkerService"(
    "SWId" INT IDENTITY(1,1) NOT NULL,
    "SId" INT NOT NULL,
    "WId" INT NOT NULL,
    "Price" FLOAT NOT NULL
);
ALTER TABLE
    "WorkerService" ADD CONSTRAINT "WorkerService_swid_primary" PRIMARY KEY("SWId");
CREATE TABLE "JobOffer"(
    "JId" INT IDENTITY(1,1) NOT NULL,
    "WId" INT NOT NULL,
    "CId" INT NOT NULL,
    "SWId" INT NOT NULL,
    "Time" INT NOT NULL,
    "Status" INT NOT NULL,
    "RId" INT NOT NULL
);
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_jid_primary" PRIMARY KEY("JId");
CREATE TABLE "Review"(
    "RId" INT IDENTITY(1,1) NOT NULL,
    "SId" INT NOT NULL,
    "SName" NVARCHAR(255) NOT NULL,
    "Date" DATE NOT NULL,
    "Rate" INT NOT NULL,
    "CId" INT NOT NULL,
    "WId" INT NOT NULL,
    "JId" INT NOT NULL
);
ALTER TABLE
    "Review" ADD CONSTRAINT "Review_rid_primary" PRIMARY KEY("RId");
CREATE TABLE "Location"(
    "LId" INT IDENTITY(1,1) NOT NULL,
    "City" NVARCHAR(255) NOT NULL,
    "Adress" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Location" ADD CONSTRAINT "location_lid_primary" PRIMARY KEY("LId");
CREATE TABLE "Status"(
    "StatusId" INT NOT NULL,
    "Description" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Status" ADD CONSTRAINT "status_statusid_primary" PRIMARY KEY("StatusId");
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_cid_foreign" FOREIGN KEY("CId") REFERENCES "Customer"("CId");
ALTER TABLE
    "Review" ADD CONSTRAINT "Review_cid_foreign" FOREIGN KEY("CId") REFERENCES "Customer"("CId");
ALTER TABLE
    "Customer" ADD CONSTRAINT "Customer_username_foreign" FOREIGN KEY("UserName") REFERENCES "User"("UserName");
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_wid_foreign" FOREIGN KEY("WId") REFERENCES "Worker"("WId");
ALTER TABLE
    "Worker" ADD CONSTRAINT "Worker_username_foreign" FOREIGN KEY("UserName") REFERENCES "User"("UserName");
ALTER TABLE
    "WorkerService" ADD CONSTRAINT "WorkerService_sid_foreign" FOREIGN KEY("SId") REFERENCES "Service"("SId");
ALTER TABLE
    "WorkerService" ADD CONSTRAINT "WorkerService_wid_foreign" FOREIGN KEY("WId") REFERENCES "Worker"("WId");
ALTER TABLE
    "Review" ADD CONSTRAINT "Review_wid_foreign" FOREIGN KEY("WId") REFERENCES "Worker"("WId");
ALTER TABLE
    "Review" ADD CONSTRAINT "Review_jid_foreign" FOREIGN KEY("JId") REFERENCES "JobOffer"("JId");
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_swid_foreign" FOREIGN KEY("SWId") REFERENCES "WorkerService"("SWId");
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_rid_foreign" FOREIGN KEY("RId") REFERENCES "Review"("RId");
ALTER TABLE
    "Worker" ADD CONSTRAINT "Worker_lid_foreign" FOREIGN KEY("LId") REFERENCES "Location"("LId");
ALTER TABLE
    "JobOffer" ADD CONSTRAINT "joboffer_status_foreign" FOREIGN KEY("Status") REFERENCES "Status"("StatusId");


    USE [IDA.DB]
GO

INSERT INTO [dbo].[User]
           ([UserName]
           ,[Email]
           ,[FirstName]
           ,[LastName]
           ,[UserPswd]
           ,[Adress]
           ,[Birthday]
           ,[IsWorker])
     VALUES
           ('danielle3'
           ,'danielle.oz@gmail.com'
           ,'danielle'
           ,'oz'
           ,'1234'
           ,'telaviv'
           ,'1978-02-21'
           ,'false')
GO

INSERT INTO [dbo].[Service]
           ([Name])
     VALUES
           ('cleaning'),

		   ('laundry'),

		   ('car Washing'),

		   ('cooking')

GO



