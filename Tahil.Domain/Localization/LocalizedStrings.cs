namespace Tahil.Domain.Localization;

public class LocalizedStrings(ILocalizationService loc)
{
    public string ServerRunning => loc[nameof(ServerRunning)];

    #region Auth
    public string InvalidCredentials => loc[nameof(InvalidCredentials)];
    public string InvalidRefreshToken => loc[nameof(InvalidRefreshToken)];
    public string Password => loc[nameof(Password)];
    public string PasswordTooShort => loc[nameof(PasswordTooShort)];
    public string RequiredPassword => Required(Password);

    #endregion

    #region Room
    public string Room => loc[nameof(Room)];
    public string RoomHasSchedules => loc[nameof(RoomHasSchedules)];
    public string RoomHasSessions => loc[nameof(RoomHasSessions)];
    public string RoomNameTooLong => loc[nameof(RoomNameTooLong)];
    public string RoomNameTooShort => loc[nameof(RoomNameTooShort)];
    public string DuplicatedRoom => Duplicate(Room);
    public string NotAvailableRoom => NotAvailable(Room);
    public string RequiredRoom => Required(Room);
    #endregion

    #region Group
    public string Group => loc[nameof(Group)];
    public string GroupHasStudentCantDelete => loc[nameof(GroupHasStudentCantDelete)];
    public string GroupHasSchedules => loc[nameof(GroupHasSchedules)];
    public string GroupHasSessions => loc[nameof(GroupHasSessions)];
    public string GroupNameTooLong => loc[nameof(GroupNameTooLong)];
    public string GroupNameTooShort => loc[nameof(GroupNameTooShort)];
    public string DuplicatedGroup => Duplicate(Group);
    public string NotAvailableGroup => NotAvailable(Group);
    public string RequiredGroup => Required(Group);
    #endregion

    #region Course
    public string Course => loc[nameof(Course)];
    public string DuplicatedCourse => Duplicate(Course);
    public string NotAvailableCourse => NotAvailable(Course);
    public string RequiredCourse => Required(Course);
    public string CourseHasSchedules => loc[nameof(CourseHasSchedules)];
    public string CourseHasSessions => loc[nameof(CourseHasSessions)];
    public string CourseHasGroups => loc[nameof(CourseHasGroups)];
    public string CourseHasTeachers => loc[nameof(CourseHasTeachers)];
    public string CourseNameTooLong => loc[nameof(CourseNameTooLong)];
    public string CourseNameTooShort => loc[nameof(CourseNameTooShort)];
    #endregion

    #region Student
    public string Student => loc[nameof(Student)];
    public string StudentHasGroups => loc[nameof(StudentHasGroups)];
    public string StudentHasAttachments => loc[nameof(StudentHasAttachments)];
    public string StudentNameTooLong => loc[nameof(StudentNameTooLong)];
    public string StudentNameTooShort => loc[nameof(StudentNameTooShort)];
    public string StudentQualificationTooLong => loc[nameof(StudentQualificationTooLong)];
    public string StudentExperienceTooLong => loc[nameof(StudentExperienceTooLong)];
    public string DuplicatedStudent => Duplicate(Student);
    public string NotAvailableStudent => NotAvailable(Student);
    public string RequiredStudent => Required(Student);
    #endregion

    #region Teacher
    public string Teacher => loc[nameof(Teacher)];
    public string TeacherHasCourses => loc[nameof(TeacherHasCourses)];
    public string TeacherHasAttachments => loc[nameof(TeacherHasAttachments)];
    public string TeacherNameTooLong => loc[nameof(TeacherNameTooLong)];
    public string TeacherNameTooShort => loc[nameof(TeacherNameTooShort)];
    public string TeacherExperienceTooLong => loc[nameof(TeacherExperienceTooLong)];
    public string TeacherQualificationTooLong => loc[nameof(TeacherQualificationTooLong)];
    public string DuplicatedTeacher => Duplicate(Teacher);
    public string NotAvailableTeacher => NotAvailable(Teacher);
    public string RequiredTeacher => Required(Teacher);
    #endregion

    #region Employee
    public string Employee => loc[nameof(Employee)];
    public string EmployeeNameTooLong => loc[nameof(EmployeeNameTooLong)];
    public string EmployeeNameTooShort => loc[nameof(EmployeeNameTooShort)];
    public string DuplicatedEmployee => Duplicate(Employee);
    public string NotAvailableEmployee => NotAvailable(Employee);
    public string RequiredEmployee => Required(Employee);
    #endregion

    #region Class Schedule
    public string ClassSchedule => loc[nameof(ClassSchedule)];
    public string TeacherHasAnotherSchedule => loc[nameof(TeacherHasAnotherSchedule)];
    public string ScheduleHasSessions => loc[nameof(ScheduleHasSessions)];
    public string ScheduleHasSessionBeforeEndDate => loc[nameof(ScheduleHasSessionBeforeEndDate)];
    public string ScheduleHasSessionAfterStartDate => loc[nameof(ScheduleHasSessionAfterStartDate)];
    public string NotAvailableClassSchedule => NotAvailable(ClassSchedule);
    public string RequiredClassSchedule => Required(loc[nameof(ClassSchedule)]);
    public string NotFoundClassSchedule => NotFound(loc[nameof(ClassSchedule)]);
    #endregion

    #region Class Schedule
    public string ClassSession => loc[nameof(ClassSession)];
    public string NotAvailableClassSession => NotAvailable(loc[nameof(ClassSession)]);
    public string CannotCompleteSessionWithIncompleteAttendance => loc[nameof(CannotCompleteSessionWithIncompleteAttendance)];
    #endregion

    #region Shared
    public string Name => loc[nameof(Name)];
    public string User => loc[nameof(User)];
    public string Attachment => loc[nameof(Attachment)];
    public string PhoneNumber => loc[nameof(PhoneNumber)];
    public string Email => loc[nameof(Email)];
    public string StartDate => loc[nameof(StartDate)];
    public string EndDate => loc[nameof(EndDate)];
    public string ConflictBusyTime => loc[nameof(ConflictBusyTime)];
    public string RoomIsBusy => loc[nameof(RoomIsBusy)];
    public string GroupIsBusy => loc[nameof(GroupIsBusy)];
    public string CannotBeNull => loc[nameof(CannotBeNull)];
    public string MustBePositive => loc[nameof(MustBePositive)];
    public string InvalidEmailFormat => loc[nameof(InvalidEmailFormat)];
    public string InvalidStatus => loc[nameof(InvalidStatus)];
    public string RequiredDay => loc[nameof(RequiredDay)];
    public string RequiredStartTime => loc[nameof(RequiredStartTime)];
    public string RequiredEndTime => loc[nameof(RequiredEndTime)];
    public string StartTimeMustBeBeforeEndTime => loc[nameof(StartTimeMustBeBeforeEndTime)];
    public string StartDateMustBeBeforeEndDate => loc[nameof(StartDateMustBeBeforeEndDate)];
    public string StartAndEndDatesValidation => loc[nameof(StartAndEndDatesValidation)];
    #endregion

    #region Exceptions
    public string RequiredName => Required(Name);
    public string RequiredEmail => Required(Email);
    public string DuplicatedEmail => Duplicate(Email);
    public string RequiredPhoneNumber => Required(PhoneNumber);
    public string DuplicatedPhoneNumber => Duplicate(PhoneNumber);
    public string NotFoundAttachment => NotFound(Attachment);
    public string NotAvailableAttachment => NotAvailable(Attachment);
    public string RequiredStartDate => Required(StartDate);
    public string RequiredEndDate => Required(EndDate);
    public string NotAvailableUser => NotAvailable(User);
    public string UserHasTeachers => loc[nameof(UserHasTeachers)];
    public string UserHasStudents => loc[nameof(UserHasStudents)];
    #endregion


    private string Duplicate(string item) => $"{item} {loc["IsDuplicated"]}";
    private string NotFound(string item) => $"{item} {loc["IsNotFount"]}";
    private string NotAvailable(string item) => $"{item} {loc["IsNotAvailable"]}";
    private string Required(string item) => $"{item} {loc["IsRequired"]}";

}