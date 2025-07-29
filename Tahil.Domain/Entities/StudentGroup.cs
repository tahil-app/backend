namespace Tahil.Domain.Entities;

public class StudentGroup : Base
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }

    public Student Student { get; set; } = default!;
    public Group Group { get; set; } = default!;
}