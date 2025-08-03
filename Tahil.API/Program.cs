using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Tahil.API.Middlewares;
using Tahil.Application;
using Tahil.Domain.Helpers;
using Tahil.Domain.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ApplicationConfig.Initialize();

builder.Services.AddLocalizations();
builder.Services.AddCORS();
builder.Services.AddServices();
builder.Services.AddMSContext();
builder.Services.AddRepositories();
builder.Services.AddEmailSettings();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandling>();

builder.Services.AddCarterModule();
builder.Services.AddMediatRModule(typeof(ApplicationConfig).Assembly);
builder.Services.AddValidationModule();

builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs
builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication();
builder.Services.AddAuthorizationPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORS");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "attachments")
    ),
    RequestPath = "/attachments",
    ServeUnknownFileTypes = true, // Optional: serve files without known MIME types
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
    }
});

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

var localizedStrings = app.Services.GetRequiredService<LocalizedStrings>();
Check.Configure(localizedStrings);

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGroup("/api").MapCarter();

app.MapGet("/", (LocalizedStrings service) => service.ServerRunning);

app.Run();
