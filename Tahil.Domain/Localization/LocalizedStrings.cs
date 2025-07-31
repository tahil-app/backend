namespace Tahil.Domain.Localization;

public class LocalizedStrings(ILocalizationService loc)
{
    public string ServerRunning => loc[nameof(ServerRunning)];

    //#region Room
    public string Room => loc[nameof(Room)];
    public string DuplicatedRoom => Duplicate(Room);
    public string NotAvailableRoom => NotAvailable(Room);
    //#endregion

    //#region Group
    public string Group => loc[nameof(Group)];
    public string GroupHasStudentCantDelete => loc[nameof(GroupHasStudentCantDelete)];
    public string DuplicatedGroup => Duplicate(Group);
    //#endregion

    //#region Course
    public string Course => loc[nameof(Course)];
    public string DuplicatedCourse => Duplicate(Course);
    public string NotAvailableCourse => NotAvailable(Course);
    //#endregion

    //#region Teacher
    public string Teacher => loc[nameof(Teacher)];
    public string NotAvailableTeacher => NotAvailable(Teacher);
    //#endregion

    //#region Shared
    public string Attachment => loc[nameof(Attachment)];
    public string PhoneNumber => loc[nameof(PhoneNumber)];
    public string Email => loc[nameof(Email)];
    public string ConflictBusyTime => loc[nameof(ConflictBusyTime)];
    public string CannotBeNull => loc[nameof(CannotBeNull)];
    public string MustBePositive => loc[nameof(MustBePositive)];
    public string InvalidEmailFormat => loc[nameof(InvalidEmailFormat)];
    //#endregion

    //#region Exceptions
    public string DuplicatedEmail => Duplicate(Email);
    public string DuplicatedPhoneNumber => Duplicate(PhoneNumber);
    public string NotFoundAttachment => NotFound(Attachment);
    //#endregion


    private string Duplicate(string item) => $"{item} {loc["IsDuplicated"]}";
    private string NotFound(string item) => $"{item} {loc["IsNotFount"]}";
    private string NotAvailable(string item) => $"{item} {loc["IsNotAvailable"]}";

}