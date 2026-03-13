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
