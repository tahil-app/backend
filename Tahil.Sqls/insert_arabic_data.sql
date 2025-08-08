-- SQL Script to Insert Arabic Data for Tahil Educational System
-- This script inserts: 5 Rooms, 5 Courses, 5 Teachers, 5 Groups, 5 Students, and 10 Student-Group relationships

-- =====================================================
-- 1. INSERT ROOMS (5 rooms in Arabic)
-- =====================================================
INSERT INTO room (name, capacity, is_active, tenant_id) VALUES
('قاعة المحاضرات الرئيسية', 50, true, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('مختبر الحاسوب الأول', 25, true, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('قاعة الدراسة الصغيرة', 15, true, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('مختبر العلوم', 30, true, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('قاعة الفنون والرسم', 20, true, '6AF39530-F6E0-4298-A890-FB5C50310C7C');

-- =====================================================
-- 2. INSERT COURSES (5 courses in Arabic)
-- =====================================================
INSERT INTO course (name, description, is_active, tenant_id) VALUES
('القرآن الكريم', 'دراسة القرآن الكريم وحفظه وتلاوته', true, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('اللغة العربية', 'قواعد اللغة العربية والنحو والصرف', true, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('الرياضيات', 'الجبر والهندسة والإحصاء', true, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('العلوم', 'الفيزياء والكيمياء والأحياء', true, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('الحاسوب', 'برمجة وتطبيقات الحاسوب', true, '6AF39530-F6E0-4298-A890-FB5C50310C7C');

-- =====================================================
-- 3. INSERT USERS for TEACHERS (5 teachers in Arabic)
-- =====================================================
INSERT INTO "user" (name, email, phone_number, password, role, gender, joined_date, birth_date, image_path, tenant_id, is_active) VALUES
('أحمد محمد علي', 'ahmed.ali@tahil.edu', '+966501234567', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 3, 1, '2024-01-15', '1985-03-20', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true),
('فاطمة عبدالله حسن', 'fatima.hassan@tahil.edu', '+966502345678', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 3, 2, '2024-02-01', '1988-07-12', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true),
('محمد سعيد الكاظمي', 'mohammed.kadhim@tahil.edu', '+966503456789', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 3, 1, '2024-01-20', '1982-11-05', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true),
('زينب محمود أحمد', 'zainab.ahmed@tahil.edu', '+966504567890', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 3, 2, '2024-02-10', '1987-09-18', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true),
('علي رضا الموسوي', 'ali.mousawi@tahil.edu', '+966505678901', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 3, 1, '2024-01-25', '1984-12-30', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true);

-- =====================================================
-- 4. INSERT TEACHERS (5 teachers with qualifications)
-- =====================================================
INSERT INTO teacher (qualification, experience, user_id) VALUES
('دكتوراه في الدراسات الإسلامية', '10', (SELECT id FROM "user" WHERE name = 'أحمد محمد علي' LIMIT 1)),
('ماجستير في اللغة العربية', '8', (SELECT id FROM "user" WHERE name = 'فاطمة عبدالله حسن' LIMIT 1)),
('دكتوراه في الرياضيات', '12', (SELECT id FROM "user" WHERE name = 'محمد سعيد الكاظمي' LIMIT 1)),
('ماجستير في العلوم', '7', (SELECT id FROM "user" WHERE name = 'زينب محمود أحمد' LIMIT 1)),
('بكالوريوس في علوم الحاسوب', '5', (SELECT id FROM "user" WHERE name = 'علي رضا الموسوي' LIMIT 1));

-- =====================================================
-- 5. INSERT USERS for STUDENTS (5 students in Arabic)
-- =====================================================
INSERT INTO "user" (name, email, phone_number, password, role, gender, joined_date, birth_date, image_path, tenant_id, is_active) VALUES
('حسن أحمد محمد', 'hassan.mohammed@student.tahil.edu', '+966506789012', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 4, 1, '2024-09-01', '2008-05-15', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true),
('مريم علي حسن', 'maryam.hassan@student.tahil.edu', '+966507890123', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 4, 2, '2024-09-01', '2009-08-22', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true),
('عبدالله محمد رضا', 'abdullah.rida@student.tahil.edu', '+966508901234', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 4, 1, '2024-09-01', '2007-12-10', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true),
('زهرة أحمد علي', 'zahra.ali@student.tahil.edu', '+966509012345', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 4, 2, '2024-09-01', '2008-03-25', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true),
('محمد علي كاظم', 'mohammed.kazim@student.tahil.edu', '+966500123456', '$2a$11$vdYkqoIhXwVHH/IWl3ddU.t1r8UD4XZJnPNMr2wRkqqpdDgLlDZ/K', 4, 1, '2024-09-01', '2009-01-08', NULL, '6AF39530-F6E0-4298-A890-FB5C50310C7C', true);

-- =====================================================
-- 6. INSERT STUDENTS (5 students with qualifications)
-- =====================================================
INSERT INTO student (qualification, user_id) VALUES
('طالب في الصف الثالث الثانوي', (SELECT id FROM "user" WHERE name = 'حسن أحمد محمد' LIMIT 1)),
('طالبة في الصف الثاني الثانوي', (SELECT id FROM "user" WHERE name = 'مريم علي حسن' LIMIT 1)),
('طالب في الصف الأول الثانوي', (SELECT id FROM "user" WHERE name = 'عبدالله محمد رضا' LIMIT 1)),
('طالبة في الصف الثالث الثانوي', (SELECT id FROM "user" WHERE name = 'زهرة أحمد علي' LIMIT 1)),
('طالب في الصف الثاني الثانوي', (SELECT id FROM "user" WHERE name = 'محمد علي كاظم' LIMIT 1));

-- =====================================================
-- 7. INSERT GROUPS (5 groups in Arabic)
-- =====================================================
INSERT INTO "group" (name, course_id, teacher_id, capacity, tenant_id) VALUES
('مجموعة القرآن الكريم الأولى', (SELECT id FROM course WHERE name = 'القرآن الكريم' LIMIT 1), (SELECT id FROM teacher WHERE user_id = (SELECT id FROM "user" WHERE name = 'أحمد محمد علي' LIMIT 1)), 15, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('مجموعة اللغة العربية المتقدمة', (SELECT id FROM course WHERE name = 'اللغة العربية' LIMIT 1), (SELECT id FROM teacher WHERE user_id = (SELECT id FROM "user" WHERE name = 'فاطمة عبدالله حسن' LIMIT 1)), 12, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('مجموعة الرياضيات الأساسية', (SELECT id FROM course WHERE name = 'الرياضيات' LIMIT 1), (SELECT id FROM teacher WHERE user_id = (SELECT id FROM "user" WHERE name = 'محمد سعيد الكاظمي' LIMIT 1)), 18, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('مجموعة العلوم التجريبية', (SELECT id FROM course WHERE name = 'العلوم' LIMIT 1), (SELECT id FROM teacher WHERE user_id = (SELECT id FROM "user" WHERE name = 'زينب محمود أحمد' LIMIT 1)), 20, '6AF39530-F6E0-4298-A890-FB5C50310C7C'),
('مجموعة الحاسوب للمبتدئين', (SELECT id FROM course WHERE name = 'الحاسوب' LIMIT 1), (SELECT id FROM teacher WHERE user_id = (SELECT id FROM "user" WHERE name = 'علي رضا الموسوي' LIMIT 1)), 10, '6AF39530-F6E0-4298-A890-FB5C50310C7C');

-- =====================================================
-- 8. INSERT STUDENT-GROUP RELATIONSHIPS (10 relationships)
-- =====================================================
INSERT INTO student_group (student_id, group_id) VALUES
-- Student 1 (حسن) in multiple groups
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'حسن أحمد محمد' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة القرآن الكريم الأولى' LIMIT 1)),
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'حسن أحمد محمد' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة الرياضيات الأساسية' LIMIT 1)),

-- Student 2 (مريم) in multiple groups
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'مريم علي حسن' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة اللغة العربية المتقدمة' LIMIT 1)),
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'مريم علي حسن' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة العلوم التجريبية' LIMIT 1)),

-- Student 3 (عبدالله) in multiple groups
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'عبدالله محمد رضا' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة القرآن الكريم الأولى' LIMIT 1)),
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'عبدالله محمد رضا' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة الحاسوب للمبتدئين' LIMIT 1)),

-- Student 4 (زهرة) in multiple groups
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'زهرة أحمد علي' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة اللغة العربية المتقدمة' LIMIT 1)),
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'زهرة أحمد علي' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة الرياضيات الأساسية' LIMIT 1)),

-- Student 5 (محمد) in multiple groups
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'محمد علي كاظم' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة العلوم التجريبية' LIMIT 1)),
((SELECT id FROM student WHERE user_id = (SELECT id FROM "user" WHERE name = 'محمد علي كاظم' LIMIT 1)), 
 (SELECT id FROM "group" WHERE name = 'مجموعة الحاسوب للمبتدئين' LIMIT 1));

-- =====================================================
-- VERIFICATION QUERIES
-- =====================================================

-- Verify the inserted data
DO $$
BEGIN
    RAISE NOTICE '=== VERIFICATION RESULTS ===';
    
    RAISE NOTICE 'Rooms inserted: %', (SELECT COUNT(*) FROM room WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C');
    
    RAISE NOTICE 'Courses inserted: %', (SELECT COUNT(*) FROM course WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C');
    
    RAISE NOTICE 'Teachers inserted: %', (SELECT COUNT(*) FROM teacher);
    
    RAISE NOTICE 'Students inserted: %', (SELECT COUNT(*) FROM student);
    
    RAISE NOTICE 'Groups inserted: %', (SELECT COUNT(*) FROM "group" WHERE tenant_id = '6AF39530-F6E0-4298-A890-FB5C50310C7C');
    
    RAISE NOTICE 'Student-Group relationships inserted: %', (SELECT COUNT(*) FROM student_group);
    
    RAISE NOTICE '=== DATA INSERTION COMPLETED SUCCESSFULLY ===';
END $$; 