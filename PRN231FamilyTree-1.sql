
-- Tạo cơ sở dữ liệu
USE [master]
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'PRN231FamilyTree')
BEGIN
    ALTER DATABASE [PRN231FamilyTree] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE [PRN231FamilyTree]
END
GO

CREATE DATABASE [PRN231FamilyTree]
GO

USE [PRN231FamilyTree]
GO

-- Tạo bảng Families
CREATE TABLE [Families](
    ID int IDENTITY PRIMARY KEY,
    FamilyName nvarchar(200) NOT NULL,
    NumberOfMembers int NOT NULL,
    [Description] nvarchar(1000)
)
GO

-- Tạo bảng FamilyMembers
CREATE TABLE [FamilyMembers](
    ID int IDENTITY PRIMARY KEY,
    FullName nvarchar(30) NOT NULL,
    Gender int NOT NULL,
    DOB DATE NOT NULL,
    Phone nvarchar(12) NOT NULL,
    Email nvarchar(100),
    [Address] nvarchar(200),
    FamilyID int FOREIGN KEY REFERENCES Families(ID)
)
GO

-- Tạo bảng Relationships
CREATE TABLE [Relationships](
    ID int IDENTITY PRIMARY KEY,
    RelationType nvarchar(20) NOT NULL
)
GO

-- Tạo bảng Relatives
CREATE TABLE [Relatives](
    ID int IDENTITY PRIMARY KEY,
    MemberID int FOREIGN KEY REFERENCES FamilyMembers(ID),
    RelationID int FOREIGN KEY REFERENCES Relationships(ID),
    MemberRelativeID int NOT NULL,
    FamilyID int FOREIGN KEY REFERENCES Families(ID)
)
GO

-- Tạo bảng Accounts
CREATE TABLE [Accounts](
    ID int IDENTITY PRIMARY KEY,
    FullName nvarchar(30) NOT NULL,
    Email nvarchar(50) NOT NULL,
    [Password] nvarchar(50),
    [Role] int NOT NULL,
    MemberID int FOREIGN KEY REFERENCES FamilyMembers(ID)
)
GO

-- Tạo bảng Activities
CREATE TABLE [Activities](
    ID int IDENTITY PRIMARY KEY,
    FamilyID int FOREIGN KEY REFERENCES Families(ID),
    ActivityName nvarchar(200) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    [Description] nvarchar(1000)
)
GO

-- Tạo bảng StudyPromotions
CREATE TABLE [StudyPromotions](
    ID int IDENTITY PRIMARY KEY,
    FamilyID int FOREIGN KEY REFERENCES Families(ID),
    PromotionName nvarchar(200) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    [Description] nvarchar(1000)
)
GO

-- Tạo bảng Albums
CREATE TABLE [Albums](
    ID int IDENTITY PRIMARY KEY,
    FamilyID int FOREIGN KEY REFERENCES Families(ID),
    AlbumName nvarchar(200),
    UrlAlbum nvarchar(300) NOT NULL,
    [Description] nvarchar(1000)
)
GO

-- Thêm dữ liệu vào bảng Families
INSERT INTO [Families] (FamilyName, NumberOfMembers, [Description])
VALUES ('Smith', 4, 'The Smith family from New York'),
       ('Johnson', 5, 'The Johnson family from California'),
       ('Brown', 3, 'The Brown family from Texas')
GO

-- Thêm dữ liệu vào bảng FamilyMembers
INSERT INTO [FamilyMembers] (FullName, Gender, DOB, Phone, Email, [Address], FamilyID)
VALUES ('John Smith', 1, '1990-05-15', '1234567890', 'johnsmith@example.com', '123 Main St, New York', 1),
       ('Emma Johnson', 0, '1995-09-20', '9876543210', 'emmajohnson@example.com', '456 Oak Ave, California', 2),
       ('Michael Brown', 1, '1985-07-10', '5555555555', 'michaelbrown@example.com', '789 Elm St, Texas', 3),
       ('Sarah Smith', 0, '1992-03-28', '7894561230', 'sarahsmith@example.com', '456 Pine St, New York', 1),
       ('Emily Johnson', 0, '2000-12-10', '3698521470', 'emilyjohnson@example.com', '789 Maple Ave, California', 2),
       ('Daniel Brown', 1, '1978-09-03', '9876543219', 'danielbrown@example.com', '123 Oak St, Texas', 3)
GO

-- Thêm dữ liệu vào bảng Relationships
INSERT INTO [Relationships] (RelationType)
VALUES ('Parent'),
       ('Sibling'),
       ('Spouse'),
       ('Child')
GO

-- Thêm dữ liệu vào bảng Relatives
INSERT INTO [Relatives] (MemberID, RelationID, MemberRelativeID, FamilyID)
VALUES (1, 3, 2, 1),
       (2, 4, 1, 2),
       (2, 2, 3, 2),
       (3, 1, 2, 3),
       (4, 2, 1, 1),
       (4, 4, 3, 1),
       (5, 2, 6, 2),
       (6, 1, 5, 3);
GO

-- Thêm dữ liệu vào bảng Accounts
INSERT INTO [Accounts] (FullName, Email, [Password], [Role], MemberID)
VALUES ('John Smith', 'johnsmith@example.com', 'password123', 1, 1),
       ('Emma Johnson', 'emmajohnson@example.com', 'password456', 2, 2),
       ('Michael Brown', 'michaelbrown@example.com', 'password789', 2, 3),
       ('Sarah Smith', 'sarahsmith@example.com', 'password321', 2, 4),
       ('Emily Johnson', 'emilyjohnson@example.com', 'password654', 2, 5),
       ('Daniel Brown', 'danielbrown@example.com', 'password987', 1, 6)
GO

-- Thêm dữ liệu vào bảng Activities
INSERT INTO [Activities] (FamilyID, ActivityName, StartDate, EndDate, [Description])
VALUES (1, 'Family Picnic', '2023-06-20 10:00:00', '2023-06-20 16:00:00', 'Enjoy a day of outdoor activities'),
       (2, 'Movie Night', '2023-06-25 19:00:00', '2023-06-25 22:00:00', 'Watch the latest movies together'),
       (3, 'Hiking Trip', '2023-07-01 08:00:00', '2023-07-01 18:00:00', 'Explore scenic trails'),
       (1, 'Beach Day', '2023-07-10 10:00:00', '2023-07-10 16:00:00', 'Relax and have fun at the beach')
GO

-- Thêm dữ liệu vào bảng StudyPromotions
INSERT INTO [StudyPromotions] (FamilyID, PromotionName, StartDate, EndDate, [Description])
VALUES (1, 'Summer Reading Challenge', '2023-06-15', '2023-08-15', 'Read and earn rewards'),
       (2, 'Math Tutoring Program', '2023-06-20', '2023-08-20', 'Improve math skills'),
       (3, 'Science Fair Competition', '2023-07-01', '2023-09-01', 'Showcase scientific projects')
GO

-- Thêm dữ liệu vào bảng Albums
INSERT INTO [Albums] (FamilyID, AlbumName, UrlAlbum, [Description])
VALUES (1, 'Family Vacation', 'https://example.com/album1', 'Memories from our summer vacation'),
       (2, 'Holiday Celebrations', 'https://example.com/album2', 'Photos of our holiday gatherings'),
       (3, 'Family Reunion', 'https://example.com/album3', 'Reconnecting with relatives')
GO

