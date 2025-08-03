namespace Tahil.Domain.Localization;

public class LocalizedStrings(ILocalizationService loc)
{
    public string ServerRunning => loc[nameof(ServerRunning)];

    #region Auth
    public string InvalidCredentials => loc[nameof(InvalidCredentials)];
    public string InvalidRefreshToken => loc[nameof(InvalidRefreshToken)];
    public string Password => loc[nameof(Password)];
    public string RequiredPassword => Required(Password);

    #endregion

    #region Room
    public string Room => loc[nameof(Room)];
    public string DuplicatedRoom => Duplicate(Room);
    public string NotAvailableRoom => NotAvailable(Room);
    public string RequiredRoom => Required(Room);
    #endregion

    #region Group
    public string Group => loc[nameof(Group)];
    public string GroupHasStudentCantDelete => loc[nameof(GroupHasStudentCantDelete)];
    public string DuplicatedGroup => Duplicate(Group);
    public string RequiredGroup => Required(Group);
    #endregion

    #region Course
    public string Course => loc[nameof(Course)];
    public string DuplicatedCourse => Duplicate(Course);
    public string NotAvailableCourse => NotAvailable(Course);
    public string RequiredCourse => Required(Course);
    #endregion

    #region Teacher
    public string Teacher => loc[nameof(Teacher)];
    public string NotAvailableTeacher => NotAvailable(Teacher);
    public string RequiredTeacher => Required(Teacher);
    #endregion

    #region Lesson Schedule
    public string LessonSchedule => loc[nameof(LessonSchedule)];
    public string NotAvailableLessonSchedule => NotAvailable(LessonSchedule);
    public string RequiredLessonSchedule => Required(LessonSchedule);
    public string NotFoundLessonSchedule => NotFound(LessonSchedule);
    #endregion

    #region Shared
    public string Name => loc[nameof(Name)];
    public string Attachment => loc[nameof(Attachment)];
    public string PhoneNumber => loc[nameof(PhoneNumber)];
    public string Email => loc[nameof(Email)];
    public string StartDate => loc[nameof(StartDate)];
    public string EndDate => loc[nameof(EndDate)];
    public string ConflictBusyTime => loc[nameof(ConflictBusyTime)];
    public string CannotBeNull => loc[nameof(CannotBeNull)];
    public string MustBePositive => loc[nameof(MustBePositive)];
    public string InvalidEmailFormat => loc[nameof(InvalidEmailFormat)];
    public string StartAndEndDatesValidation => loc[nameof(StartAndEndDatesValidation)];
    #endregion

    #region Exceptions
    public string RequiredName => Required(Name);
    public string RequiredEmail => Required(Email);
    public string DuplicatedEmail => Duplicate(Email);
    public string RequiredPhoneNumber => Required(PhoneNumber);
    public string DuplicatedPhoneNumber => Duplicate(PhoneNumber);
    public string NotFoundAttachment => NotFound(Attachment);
    public string RequiredStartDate => Required(StartDate);
    public string RequiredEndDate => Required(EndDate);
    #endregion


    private string Duplicate(string item) => $"{item} {loc["IsDuplicated"]}";
    private string NotFound(string item) => $"{item} {loc["IsNotFount"]}";
    private string NotAvailable(string item) => $"{item} {loc["IsNotAvailable"]}";
    private string Required(string item) => $"{item} {loc["IsRequired"]}";

}