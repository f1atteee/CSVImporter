CREATE DATABASE trip

IF OBJECT_ID('dbo.Trips', 'U') IS NOT NULL
  DROP TABLE dbo.Trips;

CREATE TABLE [dbo].[Trips]
(
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY, 
    [TpepPickupDatetime] DATETIME2(0) NOT NULL, 
    [TpepDropoffDatetime] DATETIME2(0) NOT NULL, 
    [PassengerCount] SMALLINT NOT NULL,
    [TripDistance] FLOAT NOT NULL,
    [StoreAndFwdFlag] NVARCHAR(3) NOT NULL, 
    [PULocationID] INT NOT NULL,
    [DOLocationID] INT NOT NULL,
    [FareAmount] DECIMAL(10, 2) NOT NULL,
    [TipAmount] DECIMAL(10, 2) NOT NULL,
    [CreatedAt] DATETIME2(0) DEFAULT SYSUTCDATETIME()
);


CREATE NONCLUSTERED INDEX IX_Trips_PULocationID_TipAmount 
ON [dbo].[Trips] ([PULocationID]) 
INCLUDE ([TipAmount]);

CREATE NONCLUSTERED INDEX IX_Trips_TripDistance_DESC
ON [dbo].[Trips] ([TripDistance] DESC);

CREATE NONCLUSTERED INDEX IX_Trips_Dropoff_Pickup
ON [dbo].[Trips] ([TpepDropoffDatetime], [TpepPickupDatetime]);

CREATE NONCLUSTERED INDEX IX_Trips_PULocationID_General
ON [dbo].[Trips] ([PULocationID], [TpepPickupDatetime]);