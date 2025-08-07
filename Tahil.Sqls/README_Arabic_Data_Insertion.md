# Arabic Data Insertion Scripts for Tahil Educational System

This directory contains SQL scripts to insert sample Arabic data into the Tahil Educational Management System database.

## Files Description

### 1. `insert_arabic_data.sql`
**Main insertion script with verification queries**
- Inserts 5 rooms in Arabic
- Inserts 5 courses in Arabic  
- Inserts 5 teachers in Arabic (with user accounts)
- Inserts 5 students in Arabic (with user accounts)
- Inserts 5 groups in Arabic
- Creates 10 student-group relationships
- Includes verification queries to confirm data insertion

### 2. `insert_arabic_data_simple.sql`
**Simplified insertion script**
- Same data as the main script but without verification queries
- Cleaner and easier to execute
- Recommended for production use

### 3. `rollback_arabic_data.sql`
**Rollback script to remove inserted data**
- Safely removes all Arabic data inserted by the insertion scripts
- Executes deletions in the correct order to maintain referential integrity
- Includes verification queries to confirm data removal

## Data Overview

### Rooms (5 rooms)
1. **قاعة المحاضرات الرئيسية** - Main Lecture Hall (Capacity: 50)
2. **مختبر الحاسوب الأول** - Computer Lab 1 (Capacity: 25)
3. **قاعة الدراسة الصغيرة** - Small Study Room (Capacity: 15)
4. **مختبر العلوم** - Science Lab (Capacity: 30)
5. **قاعة الفنون والرسم** - Arts and Drawing Room (Capacity: 20)

### Courses (5 courses)
1. **القرآن الكريم** - Quran Studies (Islamic studies and memorization)
2. **اللغة العربية** - Arabic Language (Grammar and syntax)
3. **الرياضيات** - Mathematics (Algebra, geometry, statistics)
4. **العلوم** - Sciences (Physics, chemistry, biology)
5. **الحاسوب** - Computer Science (Programming and applications)

### Teachers (5 teachers)
1. **أحمد محمد علي** - PhD in Islamic Studies (10 years experience)
2. **فاطمة عبدالله حسن** - Master's in Arabic Language (8 years experience)
3. **محمد سعيد الكاظمي** - PhD in Mathematics (12 years experience)
4. **زينب محمود أحمد** - Master's in Sciences (7 years experience)
5. **علي رضا الموسوي** - Bachelor's in Computer Science (5 years experience)

### Students (5 students)
1. **حسن أحمد محمد** - 3rd Year Secondary Student
2. **مريم علي حسن** - 2nd Year Secondary Student
3. **عبدالله محمد رضا** - 1st Year Secondary Student
4. **زهرة أحمد علي** - 3rd Year Secondary Student
5. **محمد علي كاظم** - 2nd Year Secondary Student

### Groups (5 groups)
1. **مجموعة القرآن الكريم الأولى** - First Quran Group (Teacher: أحمد محمد علي)
2. **مجموعة اللغة العربية المتقدمة** - Advanced Arabic Group (Teacher: فاطمة عبدالله حسن)
3. **مجموعة الرياضيات الأساسية** - Basic Mathematics Group (Teacher: محمد سعيد الكاظمي)
4. **مجموعة العلوم التجريبية** - Experimental Sciences Group (Teacher: زينب محمود أحمد)
5. **مجموعة الحاسوب للمبتدئين** - Computer Science for Beginners (Teacher: علي رضا الموسوي)

### Student-Group Relationships (10 relationships)
Each student is enrolled in 2 different groups:
- **حسن**: Quran Group + Mathematics Group
- **مريم**: Arabic Group + Sciences Group
- **عبدالله**: Quran Group + Computer Group
- **زهرة**: Arabic Group + Mathematics Group
- **محمد**: Sciences Group + Computer Group

## Prerequisites

Before running these scripts, ensure:

1. **Database exists** and is properly configured
2. **Tenant exists** with ID: `6AF39530-F6E0-4298-A890-FB5C50310C7C`
3. **Database schema** matches the expected structure
4. **Proper permissions** to execute INSERT/DELETE operations

## Usage Instructions

### To Insert Data:

1. **Backup your database** (recommended)
2. **Connect to your SQL Server database**
3. **Execute the insertion script**:
   ```sql
   -- For main script with verification
   EXECUTE insert_arabic_data.sql
   
   -- OR for simplified script
   EXECUTE insert_arabic_data_simple.sql
   ```

### To Remove Data:

1. **Execute the rollback script**:
   ```sql
   EXECUTE rollback_arabic_data.sql
   ```

## Important Notes

### Security
- All users have the same hashed password: `$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi`
- This corresponds to the plain text password: `password`
- **Change passwords** after insertion for security

### Data Integrity
- Scripts maintain referential integrity
- Foreign key relationships are properly established
- All data is associated with the existing tenant

### Character Encoding
- Scripts use proper Unicode encoding for Arabic text
- Ensure your database supports UTF-8 or Unicode encoding

## Verification Queries

After running the insertion script, you can verify the data with these queries:

```sql
-- Count all inserted records
SELECT 'Rooms' as TableName, COUNT(*) as Count FROM room WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C'
UNION ALL
SELECT 'Courses', COUNT(*) FROM course WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C'
UNION ALL
SELECT 'Teachers', COUNT(*) FROM teacher
UNION ALL
SELECT 'Students', COUNT(*) FROM student
UNION ALL
SELECT 'Groups', COUNT(*) FROM [group] WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C'
UNION ALL
SELECT 'Student-Group Relationships', COUNT(*) FROM student_group;

-- View student-group assignments
SELECT 
    u.name as StudentName,
    g.name as GroupName,
    c.name as CourseName
FROM student_group sg
JOIN student s ON sg.student_id = s.id
JOIN [user] u ON s.user_id = u.id
JOIN [group] g ON sg.group_id = g.id
JOIN course c ON g.course_id = c.id
ORDER BY u.name, g.name;
```

## Troubleshooting

### Common Issues:

1. **Foreign Key Violations**: Ensure the tenant exists before running scripts
2. **Character Encoding Issues**: Check database collation settings
3. **Permission Errors**: Ensure proper database permissions
4. **Duplicate Key Errors**: Run rollback script first if data already exists

### Error Recovery:

If you encounter errors during insertion:
1. **Stop the script execution**
2. **Run the rollback script** to clean up partial data
3. **Check the error messages** and fix the underlying issue
4. **Re-run the insertion script**

## Support

For issues or questions regarding these scripts, please refer to:
- Database schema documentation
- Entity Framework migrations
- Application configuration files 