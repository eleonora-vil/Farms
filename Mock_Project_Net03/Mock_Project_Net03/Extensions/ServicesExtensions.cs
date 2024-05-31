using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mock_Project_Net03.Data;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Helpers;
using Mock_Project_Net03.Helpers.Photos;
using Mock_Project_Net03.Mapper;
using Mock_Project_Net03.Middlewares;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Services.Syllabus;
using Mock_Project_Net03.Settings;
using System.Text;

namespace Mock_Project_Net03.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ExceptionMiddleware>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Add Mapper
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ApplicationMapper());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        //Set time
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        services.Configure<JwtSettings>(val =>
        {
            val.Key = jwtSettings.Key;
        });

        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

        services.Configure<CloundSettings>(configuration.GetSection(nameof(CloundSettings)));

        services.AddAuthorization();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("PgDbConnection"));
        });

        services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<DatabaseInitialiser>();
        services.AddScoped<IdentityService>();
        services.AddScoped<UserService>();
        services.AddScoped<UserRoleService>();
        services.AddScoped<PermissionService>();
        services.AddScoped<SyllabusService>();
        services.AddScoped<OutlineMaterialsServices>();
        services.AddScoped<SyllabusOutlineLearningObjServices>();
        services.AddScoped<SyllabusOutlineUnitServices>();
        services.AddScoped<EmailService>();
        services.AddScoped<CloudService>();
        services.AddScoped<TrainingProgramServices>();
        services.AddScoped<ManaService>();
        services.AddScoped<ClassService>();
        services.AddScoped<CreateFullSyllabusService>();
        services.AddScoped<AssessmentSchemeService>();
        services.AddScoped<OutputStandardService>();
        services.AddScoped<ViewTrainingCalendarService>();
        services.AddScoped<RoomService>();
        services.AddScoped<EnrollmentService>();
        services.AddScoped<SemesterService>();



        return services;
    }
}