using Tahil.Common.Exceptions;

namespace Tahil.Infrastructure.Repositories;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(BEContext context) : base(context.Set<Teacher>())
    {
    }

    public async Task AddTeacherAsync(Teacher teacher)
    {
        await CheckExistsAsync(teacher);

        teacher.User.IsActive = true;
        Add(teacher);
    }

    private async Task CheckExistsAsync(Teacher teacher)
    {
        var existTeacher = await GetAsync(u => u.User.Email.Value == teacher.User.Email.Value || u.User.PhoneNumber == teacher.User.PhoneNumber);

        if (existTeacher is not null && existTeacher.User.Email.Value == teacher.User.Email.Value)
            throw new DuplicateException("Email");

        if (existTeacher is not null && existTeacher.User.PhoneNumber == teacher.User.PhoneNumber)
            throw new DuplicateException("Phone Number");
    }

}