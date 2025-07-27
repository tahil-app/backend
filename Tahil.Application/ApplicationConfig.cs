using Tahil.Application.Auth.Mappings;
using Tahil.Application.Courses.Mappings;
using Tahil.Application.Rooms.Mappings;
using Tahil.Application.Teachers.Mappings;

namespace Tahil.Application;

public class ApplicationConfig
{
    public static void Initialize()
    {
        SignupMapping.RegisterMappings();
        CourseMapping.RegisterMappings();
        RoomMapping.RegisterMappings();
        TeacherMapping.RegisterMappings();
    }
}