-- PCM Database Initialization Script
-- This script runs when the SQL Server container starts for the first time

-- Create Database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PCM_DB')
BEGIN
    CREATE DATABASE PCM_DB;
    PRINT 'Database PCM_DB created successfully.';
END
GO

USE PCM_DB;
GO

-- Note: The actual schema and seed data will be created by Entity Framework Core migrations
-- when the backend container starts. This script is for initial database creation only.

PRINT 'Database initialization completed. EF Core migrations will handle schema creation.';
GO
