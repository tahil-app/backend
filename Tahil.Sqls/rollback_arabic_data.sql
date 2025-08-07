-- Rollback Script to Remove Arabic Data from Tahil Educational System
-- This script removes the data inserted by insert_arabic_data.sql

-- =====================================================
-- WARNING: This script will delete all the Arabic data
-- Make sure you have a backup before running this script
-- =====================================================

-- =====================================================
-- 1. DELETE STUDENT-GROUP RELATIONSHIPS
-- =====================================================
DELETE FROM student_group 
WHERE student_id IN (
    SELECT s.id 
    FROM student s 
    JOIN [user] u ON s.user_id = u.id 
    WHERE u.name IN (
        'حسن أحمد محمد',
        'مريم علي حسن', 
        'عبدالله محمد رضا',
        'زهرة أحمد علي',
        'محمد علي كاظم'
    )
);

-- =====================================================
-- 2. DELETE GROUPS
-- =====================================================
DELETE FROM [group] 
WHERE name IN (
    'مجموعة القرآن الكريم الأولى',
    'مجموعة اللغة العربية المتقدمة',
    'مجموعة الرياضيات الأساسية',
    'مجموعة العلوم التجريبية',
    'مجموعة الحاسوب للمبتدئين'
);

-- =====================================================
-- 3. DELETE STUDENTS
-- =====================================================
DELETE FROM student 
WHERE user_id IN (
    SELECT id FROM [user] 
    WHERE name IN (
        'حسن أحمد محمد',
        'مريم علي حسن', 
        'عبدالله محمد رضا',
        'زهرة أحمد علي',
        'محمد علي كاظم'
    )
);

-- =====================================================
-- 4. DELETE TEACHERS
-- =====================================================
DELETE FROM teacher 
WHERE user_id IN (
    SELECT id FROM [user] 
    WHERE name IN (
        'أحمد محمد علي',
        'فاطمة عبدالله حسن',
        'محمد سعيد الكاظمي',
        'زينب محمود أحمد',
        'علي رضا الموسوي'
    )
);

-- =====================================================
-- 5. DELETE USERS (both teachers and students)
-- =====================================================
DELETE FROM [user] 
WHERE name IN (
    -- Teachers
    'أحمد محمد علي',
    'فاطمة عبدالله حسن',
    'محمد سعيد الكاظمي',
    'زينب محمود أحمد',
    'علي رضا الموسوي',
    -- Students
    'حسن أحمد محمد',
    'مريم علي حسن', 
    'عبدالله محمد رضا',
    'زهرة أحمد علي',
    'محمد علي كاظم'
);

-- =====================================================
-- 6. DELETE COURSES
-- =====================================================
DELETE FROM course 
WHERE name IN (
    'القرآن الكريم',
    'اللغة العربية',
    'الرياضيات',
    'العلوم',
    'الحاسوب'
);

-- =====================================================
-- 7. DELETE ROOMS
-- =====================================================
DELETE FROM room 
WHERE name IN (
    'قاعة المحاضرات الرئيسية',
    'مختبر الحاسوب الأول',
    'قاعة الدراسة الصغيرة',
    'مختبر العلوم',
    'قاعة الفنون والرسم'
);

-- =====================================================
-- VERIFICATION
-- =====================================================
PRINT '=== ROLLBACK COMPLETED ===';
PRINT 'Verifying data removal:';

SELECT 'Rooms remaining' as TableName, COUNT(*) as Count FROM room WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C'
UNION ALL
SELECT 'Courses remaining', COUNT(*) FROM course WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C'
UNION ALL
SELECT 'Teachers remaining', COUNT(*) FROM teacher
UNION ALL
SELECT 'Students remaining', COUNT(*) FROM student
UNION ALL
SELECT 'Groups remaining', COUNT(*) FROM [group] WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C'
UNION ALL
SELECT 'Student-Group relationships remaining', COUNT(*) FROM student_group;

PRINT '=== ROLLBACK VERIFICATION COMPLETED ==='; 