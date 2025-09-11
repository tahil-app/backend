using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Globalization;
using System.Reflection;
using System.Text;
using Tahil.API.Authorization.Handlers;
using Tahil.API.Filters;
using Tahil.API.Jobs;
using Tahil.Application;
using Tahil.Common.Behaviors;
using Tahil.Domain.Authorization.Services;
using Tahil.Domain.Authorization.Strategies;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Repositories;
using Tahil.EmailSender.Models;
using Tahil.EmailSender.Services;
using Tahil.Infrastructure.Data;
using Tahil.Infrastructure.Reports;
using Tahil.Infrastructure.Repositories;

namespace Tahil.API.Extensions;

public static class SetupExtensions
{
    public static IServiceCollection AddCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CORS", policy =>
            {
                policy.AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod();
            });
        });

        return services;
    }

    public static IServiceCollection AddMSContext(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;
        var config = services.BuildServiceProvider().GetService<IConfigService>()!;

        services.AddDbContext<BEContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("SQL"));
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ILookupRepository, LookupRepository>();
        services.AddScoped<IClassScheduleRepository, ClassScheduleRepository>();
        services.AddScoped<IClassSessionRepository, ClassSessionRepository>();
        services.AddScoped<IStudentAttendnceRepository, StudentAttendnceRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IConfigService, ConfigService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUploadService, UploadService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IApplicationContext, ApplicationContext>();
        
        // Report services
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReport, TeacherScheduleReport>();
        services.AddScoped<IReport, GroupScheduleReport>();
        services.AddScoped<IReport, StudentScheduleReport>();
        services.AddScoped<IReport, StudentFeedbackReport>();
        services.AddScoped<IReport, StudentAttendanceMonthlyReport>();
        services.AddScoped<IReport, StudentAttendanceDailyReport>();
        
        // Authorization services
        services.AddScoped<IResourceAuthorizationService, ResourceAuthorizationService>();
        services.AddScoped<IAuthorizationHandler, ResourceAuthorizationHandler>();

        // Resource authorization strategies
        services.AddScoped<IEntityAuthorizationStrategy, GroupAuthorizationStrategy>();
        services.AddScoped<IEntityAuthorizationStrategy, CourseAuthorizationStrategy>();
        services.AddScoped<IEntityAuthorizationStrategy, RoomAuthorizationStrategy>();
        services.AddScoped<IEntityAuthorizationStrategy, TeacherAuthorizationStrategy>();
        services.AddScoped<IEntityAuthorizationStrategy, StudentAuthorizationStrategy>();
        services.AddScoped<IEntityAuthorizationStrategy, ClassScheduleAuthorizationStrategy>();
        services.AddScoped<IEntityAuthorizationStrategy, ClassSessionAuthorizationStrategy>();
        services.AddScoped<IEntityAuthorizationStrategy, BackgroundJobAuthorizationStrategy>();
        services.AddScoped<IEntityAuthorizationStrategy, StudentAttendanceAuthorizationStrategy>();
        
        return services;
    }

    public static IServiceCollection AddQuartaz(this IServiceCollection services) 
    {
        // Background job services
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.AddHostedService<QuartzSchedulerService>();

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Tahil API Template",
                Version = "v1",
                Description = "Your clean architecture API documentation"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = @"JWT Authorization header using the Bearer scheme. 
                                Example: 'Bearer YOUR_TOKEN'"
            });

            options.OperationFilter<MinimalApiAuthOperationFilter>();
        });

        return services;
    }

    public static IServiceCollection AddCarterModule(this IServiceCollection services)
    {
        // Find all ICarterModule implementations
        var modules = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => typeof(ICarterModule).IsAssignableFrom(t) &&
                       !t.IsAbstract &&
                       !t.IsInterface);

        services.AddCarter(null, config =>
        {
            foreach (var module in modules)
            {
                config.WithModules(module);
            }
        });

        return services;
    }

    public static IServiceCollection AddMediatRModule(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavoir<,>));
        });

        return services;
    }

    public static IServiceCollection AddValidationModule(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ApplicationConfig>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;

        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero // Optional: eliminates default 5 mins tolerance
                };
            });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AdminOnly, policy => policy.RequireRole(UserRole.Admin.ToString()));
            options.AddPolicy(Policies.AdminOrEmployee, policy => policy.RequireRole(UserRole.Admin.ToString(), UserRole.Employee.ToString()));
            options.AddPolicy(Policies.AdminOrEmployeeOrTeacher, policy => policy.RequireRole(UserRole.Admin.ToString(), UserRole.Employee.ToString(), UserRole.Teacher.ToString()));
            options.AddPolicy(Policies.TeacherOnly, policy => policy.RequireRole(UserRole.Teacher.ToString()));
            options.AddPolicy(Policies.ALL, policy => policy.RequireRole(UserRole.Admin.ToString(), UserRole.Employee.ToString(), UserRole.Teacher.ToString(), UserRole.Student.ToString()));
        });

        return services;
    }

    public static IServiceCollection AddEmailSettings(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        services.AddScoped<IEmailSenderService, EmailSenderService>();

        return services;
    }

    public static IServiceCollection AddLocalizations(this IServiceCollection services)
    {
        services.AddSingleton<ILocalizationService, LocalizationService>();
        services.AddSingleton<LocalizedStrings>(r =>
            new LocalizedStrings(r.GetRequiredService<ILocalizationService>())
        );

        services.AddLocalization(opt => opt.ResourcesPath = "Resources");
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { "en", "ar" };
            var cultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();

            options.DefaultRequestCulture = new RequestCulture("ar");
            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;

            options.RequestCultureProviders = new[]
            {
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });

        return services;
    }
}