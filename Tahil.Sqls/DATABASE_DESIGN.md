# Database Design Documentation

## Overview
This document outlines the database design for the Tahil Educational Management System, which manages courses, teachers, students, groups, rooms, and lesson schedules.

## Database Schema

### Core Tables

#### 1. Users Table
```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Password NVARCHAR(255) NOT NULL,
    Role INT NOT NULL, -- UserRole enum
    IsActive BIT NOT NULL DEFAULT 1,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    Image NVARCHAR(255) NULL,
    ImageName NVARCHAR(255) NULL
);
```

#### 2. Tenants Table
```sql
CREATE TABLE Tenants (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
```

#### 3. Courses Table
```sql
CREATE TABLE Courses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);
```

#### 4. Teachers Table
```sql
CREATE TABLE Teachers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Password NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    Image NVARCHAR(255) NULL,
    ImageName NVARCHAR(255) NULL
);
```

#### 5. Students Table
```sql
CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Password NVARCHAR(255) NOT NULL,
    Gender INT NOT NULL, -- Gender enum
    IsActive BIT NOT NULL DEFAULT 1,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    Image NVARCHAR(255) NULL,
    ImageName NVARCHAR(255) NULL
);
```

#### 6. Rooms Table
```sql
CREATE TABLE Rooms (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);
```

#### 7. Groups Table
```sql
CREATE TABLE Groups (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);
```

### Relationship Tables

#### 8. TeacherCourses Table
```sql
CREATE TABLE TeacherCourses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TeacherId INT NOT NULL,
    CourseId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    FOREIGN KEY (TeacherId) REFERENCES Teachers(Id),
    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);
```

#### 9. StudentGroups Table
```sql
CREATE TABLE StudentGroups (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    GroupId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    FOREIGN KEY (StudentId) REFERENCES Students(Id),
    FOREIGN KEY (GroupId) REFERENCES Groups(Id)
);
```

#### 10. ClassSchedules Table
```sql
CREATE TABLE ClassSchedules (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CourseId INT NOT NULL,
    TeacherId INT NOT NULL,
    RoomId INT NOT NULL,
    GroupId INT NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    DayOfWeek INT NOT NULL, -- DayOfWeek enum
    Status INT NOT NULL DEFAULT 0, -- ClassScheduleStatus enum
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    
    FOREIGN KEY (CourseId) REFERENCES Courses(Id),
    FOREIGN KEY (TeacherId) REFERENCES Teachers(Id),
    FOREIGN KEY (RoomId) REFERENCES Rooms(Id),
    FOREIGN KEY (GroupId) REFERENCES Groups(Id)
);
```

#### 11. LessonSessions Table
```sql
CREATE TABLE LessonSessions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClassScheduleId INT NOT NULL,
    CourseId INT NOT NULL,
    TeacherId INT NOT NULL,
    RoomId INT NOT NULL,
    GroupId INT NOT NULL,
    LessonDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    Status INT NOT NULL DEFAULT 0, -- LessonSessionStatus enum
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    
    FOREIGN KEY (ClassScheduleId) REFERENCES ClassSchedules(Id),
    FOREIGN KEY (CourseId) REFERENCES Courses(Id),
    FOREIGN KEY (TeacherId) REFERENCES Teachers(Id),
    FOREIGN KEY (RoomId) REFERENCES Rooms(Id),
    FOREIGN KEY (GroupId) REFERENCES Groups(Id)
);
```

### Attachment Tables

#### 12. Attachments Table
```sql
CREATE TABLE Attachments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileSize BIGINT NOT NULL,
    ContentType NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
```

#### 13. TeacherAttachments Table
```sql
CREATE TABLE TeacherAttachments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TeacherId INT NOT NULL,
    AttachmentId INT NOT NULL,
    DisplayName NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    FOREIGN KEY (TeacherId) REFERENCES Teachers(Id),
    FOREIGN KEY (AttachmentId) REFERENCES Attachments(Id)
);
```

#### 14. StudentAttachments Table
```sql
CREATE TABLE StudentAttachments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    AttachmentId INT NOT NULL,
    DisplayName NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    FOREIGN KEY (StudentId) REFERENCES Students(Id),
    FOREIGN KEY (AttachmentId) REFERENCES Attachments(Id)
);
```

## Enums

### UserRole
- `None = 0`
- `Admin = 1`
- `Employee = 2`
- `Teacher = 3`
- `Student = 4`

### Gender
- `Male = 1`
- `Female = 2`

### ClassScheduleStatus
- `Inactive = 0`
- `Active = 1`

### LessonSessionStatus
- `Scheduled = 0`
- `Completed = 1`
- `Cancelled = 2`

### DayOfWeek
- `Sunday = 0`
- `Monday = 1`
- `Tuesday = 2`
- `Wednesday = 3`
- `Thursday = 4`
- `Friday = 5`
- `Saturday = 6`

## Relationships

### One-to-Many Relationships
- **Tenant → Users**: One tenant can have multiple users
- **Tenant → Teachers**: One tenant can have multiple teachers
- **Tenant → Students**: One tenant can have multiple students
- **Course → ClassSchedules**: One course can have multiple lesson schedules
- **Course → LessonSessions**: One course can have multiple lesson sessions
- **Teacher → ClassSchedules**: One teacher can have multiple lesson schedules
- **Teacher → LessonSessions**: One teacher can have multiple lesson sessions
- **Room → ClassSchedules**: One room can have multiple lesson schedules
- **Room → LessonSessions**: One room can have multiple lesson sessions
- **Group → ClassSchedules**: One group can have multiple lesson schedules
- **Group → LessonSessions**: One group can have multiple lesson sessions
- **ClassSchedule → LessonSessions**: One lesson schedule can have multiple sessions

### Many-to-Many Relationships
- **Teachers ↔ Courses**: Through TeacherCourses table
- **Students ↔ Groups**: Through StudentGroups table
- **Teachers ↔ Attachments**: Through TeacherAttachments table
- **Students ↔ Attachments**: Through StudentAttachments table

## Indexes

### Primary Indexes
- All tables have clustered primary key indexes on their `Id` columns

### Recommended Secondary Indexes
```sql
-- Users table
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_PhoneNumber ON Users(PhoneNumber);
CREATE INDEX IX_Users_TenantId ON Users(TenantId);
CREATE INDEX IX_Users_Role ON Users(Role);

-- Teachers table
CREATE INDEX IX_Teachers_Email ON Teachers(Email);
CREATE INDEX IX_Teachers_PhoneNumber ON Teachers(PhoneNumber);
CREATE INDEX IX_Teachers_TenantId ON Teachers(TenantId);

-- Students table
CREATE INDEX IX_Students_Email ON Students(Email);
CREATE INDEX IX_Students_PhoneNumber ON Students(PhoneNumber);
CREATE INDEX IX_Students_TenantId ON Students(TenantId);

-- ClassSchedules table
CREATE INDEX IX_ClassSchedules_CourseId ON ClassSchedules(CourseId);
CREATE INDEX IX_ClassSchedules_TeacherId ON ClassSchedules(TeacherId);
CREATE INDEX IX_ClassSchedules_RoomId ON ClassSchedules(RoomId);
CREATE INDEX IX_ClassSchedules_GroupId ON ClassSchedules(GroupId);
CREATE INDEX IX_ClassSchedules_DayOfWeek ON ClassSchedules(DayOfWeek);

-- LessonSessions table
CREATE INDEX IX_LessonSessions_ClassScheduleId ON LessonSessions(ClassScheduleId);
CREATE INDEX IX_LessonSessions_CourseId ON LessonSessions(CourseId);
CREATE INDEX IX_LessonSessions_TeacherId ON LessonSessions(TeacherId);
CREATE INDEX IX_LessonSessions_RoomId ON LessonSessions(RoomId);
CREATE INDEX IX_LessonSessions_GroupId ON LessonSessions(GroupId);
CREATE INDEX IX_LessonSessions_LessonDate ON LessonSessions(LessonDate);
```

## Constraints

### Check Constraints
```sql
-- Email format validation
ALTER TABLE Users ADD CONSTRAINT CK_Users_Email CHECK (Email IS NULL OR Email LIKE '%_@_%._%');
ALTER TABLE Teachers ADD CONSTRAINT CK_Teachers_Email CHECK (Email IS NULL OR Email LIKE '%_@_%._%');
ALTER TABLE Students ADD CONSTRAINT CK_Students_Email CHECK (Email IS NULL OR Email LIKE '%_@_%._%');

-- Time validation for lesson schedules
ALTER TABLE ClassSchedules ADD CONSTRAINT CK_ClassSchedules_Time CHECK (StartTime < EndTime);

-- Time validation for lesson sessions
ALTER TABLE LessonSessions ADD CONSTRAINT CK_LessonSessions_Time CHECK (StartTime < EndTime);

-- Day of week validation
ALTER TABLE ClassSchedules ADD CONSTRAINT CK_ClassSchedules_DayOfWeek CHECK (DayOfWeek >= 0 AND DayOfWeek <= 6);
```

### Unique Constraints
```sql
-- Unique email per tenant
ALTER TABLE Users ADD CONSTRAINT UQ_Users_Email_TenantId UNIQUE (Email, TenantId);
ALTER TABLE Teachers ADD CONSTRAINT UQ_Teachers_Email_TenantId UNIQUE (Email, TenantId);
ALTER TABLE Students ADD CONSTRAINT UQ_Students_Email_TenantId UNIQUE (Email, TenantId);

-- Unique phone number per tenant
ALTER TABLE Users ADD CONSTRAINT UQ_Users_PhoneNumber_TenantId UNIQUE (PhoneNumber, TenantId);
ALTER TABLE Teachers ADD CONSTRAINT UQ_Teachers_PhoneNumber_TenantId UNIQUE (PhoneNumber, TenantId);
ALTER TABLE Students ADD CONSTRAINT UQ_Students_PhoneNumber_TenantId UNIQUE (PhoneNumber, TenantId);

-- Unique course names
ALTER TABLE Courses ADD CONSTRAINT UQ_Courses_Name UNIQUE (Name);

-- Unique room names
ALTER TABLE Rooms ADD CONSTRAINT UQ_Rooms_Name UNIQUE (Name);

-- Unique group names
ALTER TABLE Groups ADD CONSTRAINT UQ_Groups_Name UNIQUE (Name);
```

## Data Integrity Rules

### Business Rules
1. **Course Deletion**: A course cannot be deleted if it has:
   - Associated lesson schedules
   - Associated lesson sessions
   - Associated teachers (through TeacherCourses)

2. **Group Deletion**: A group cannot be deleted if it has:
   - Associated students (through StudentGroups)
   - Associated lesson schedules
   - Associated lesson sessions

3. **Teacher Deletion**: A teacher cannot be deleted if they have:
   - Associated lesson schedules
   - Associated lesson sessions

4. **Student Deletion**: A student cannot be deleted if they have:
   - Associated lesson sessions
   - Associated attachments

5. **Room Deletion**: A room cannot be deleted if it has:
   - Associated lesson schedules
   - Associated lesson sessions

### Validation Rules
1. **Course Name**: Must be between 2-100 characters, unique
2. **Email**: Must be valid email format, unique per tenant
3. **Phone Number**: Must be unique per tenant
4. **Lesson Times**: Start time must be before end time
5. **Lesson Dates**: Cannot be in the past for new sessions

## Performance Considerations

### Partitioning Strategy
- Consider partitioning large tables by `TenantId` for multi-tenant scenarios
- Partition `LessonSessions` by `LessonDate` for historical data

### Archiving Strategy
- Archive completed lesson sessions older than 2 years
- Archive inactive users, teachers, and students after 1 year of inactivity

### Backup Strategy
- Full backup: Daily
- Transaction log backup: Every 15 minutes
- Differential backup: Weekly

## Security Considerations

### Data Encryption
- Encrypt sensitive data like passwords using bcrypt
- Consider encrypting email addresses and phone numbers at rest

### Access Control
- Implement row-level security based on `TenantId`
- Use database roles for different user types
- Audit all data modifications

### Compliance
- Ensure GDPR compliance for personal data
- Implement data retention policies
- Provide data export and deletion capabilities

## Migration Strategy

### Version Control
- Use Entity Framework migrations for schema changes
- Maintain migration history in source control
- Test migrations on staging environment before production

### Data Migration
- Plan for data migration when schema changes
- Maintain backward compatibility during transitions
- Use feature flags for gradual rollouts

---

*This document should be updated whenever the database schema changes.* 