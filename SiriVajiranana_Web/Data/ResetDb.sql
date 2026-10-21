-- Database Reset Script for Dhamma School Management System
-- Run against Database1.mdf


DROP TABLE IF EXISTS Teachers;
DROP TABLE IF EXISTS Student_Registration;
DROP TABLE IF EXISTS Login;
GO

CREATE TABLE Login (
    AdminID int PRIMARY KEY,
    UserName varchar(50) NOT NULL,
    Password varchar(50) NOT NULL,
    Admin_Name nvarchar(255) NOT NULL,
    IsApproved bit DEFAULT 0
);
GO

CREATE TABLE Student_Registration (
    Registration_No int PRIMARY KEY,
    Name varchar(255),
    Gender varchar(20),
    Address varchar(255),
    Telephone_No varchar(20),
    Other_Skills nvarchar(MAX),
    Year_of_Admission varchar(50),
    Grade05_Buddhism varchar(255),
    Grade05_Abhidhamma varchar(255),
    Grade05_Pali varchar(255),
    Grade06_Buddhism varchar(255),
    Grade06_Abhidhamma varchar(255),
    Grade06_Pali varchar(255),
    Grade07_Buddhism varchar(255),
    Grade07_Abhidhamma varchar(255),
    Grade07_Pali varchar(255),
    Grade08_Buddhism varchar(255),
    Grade08_Abhidhamma varchar(255),
    Grade08_Pali varchar(255),
    Grade09_Buddhism varchar(255),
    Grade09_Abhidhamma varchar(255),
    Grade09_Pali varchar(255),
    Grade10_Buddhism varchar(255),
    Grade10_Abhidhamma varchar(255),
    Grade10_Pali varchar(255),
    Grade11_Buddhism varchar(255),
    Grade11_Abhidhamma varchar(255),
    Grade11_Pali varchar(255)
);
GO

CREATE TABLE Teachers (
    TeacherID int PRIMARY KEY,
    Name nvarchar(255) NOT NULL,
    Gender varchar(20) NOT NULL,
    NIC varchar(20) NOT NULL,
    Telephone_No varchar(20) NOT NULL,
    Address nvarchar(MAX) NOT NULL,
    Email nvarchar(100),
    Year_of_Appointment varchar(20) NOT NULL,
    Subject nvarchar(100) NOT NULL
);
GO

-- Seed default Super Admin
INSERT INTO Login (AdminID, UserName, Password, Admin_Name, IsApproved)
VALUES (1, 'Super Admin', 'Super Admin', 'Super Admin', 1);
GO
