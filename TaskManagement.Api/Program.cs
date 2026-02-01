using System.Reflection;

using FluentValidation;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

using TaskManagement.Api.Validators;
using TaskManagement.Core.Contracts.Request;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=data.db");
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // JwtSettings will be injected where needed, not here
        var jwtSection = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"] ??
                    throw new InvalidOperationException("JWT Key is missing from configuration."))),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IValidator<CreateTaskItemRequest>, TaskItemValidator>();
builder.Services.AddScoped<IValidator<UserRegisterRequest>, UserRegisterValidator>();
builder.Services.AddScoped<IValidator<UserLoginRequest>, UserLoginValidator>();
builder.Services
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddFluentValidationAutoValidation();


var mappingConfig = TypeAdapterConfig.GlobalSettings;
mappingConfig.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IMapper>(new Mapper(mappingConfig));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    //     app.MapScalarApiReference(options => options
    //         .AddPreferredSecuritySchemes("BearerAuth")
    //         .AddHttpAuthentication("BearerAuth", auth =>
    //         {
    //             auth.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
    //         }));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

internal sealed class BearerSecuritySchemeTransformer(Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } }] = Array.Empty<string>()
                });
            }
        }
    }
}
