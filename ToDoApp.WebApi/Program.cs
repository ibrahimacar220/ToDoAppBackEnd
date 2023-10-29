using ToDoApp.Business.SqlServer.Business.Interface;
using ToDoApp.Business.SqlServer.Business;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoApp.WebApi;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = true;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = false,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Stc.JwtIssuer,
                   ValidAudience = Stc.JwtAudience,
                   ClockSkew = TimeSpan.Zero,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Stc.JwtKey))
               };
           });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "jwt_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "here enter JWT Token with bearer format like bearer[space] token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{  new OpenApiSecurityScheme
        {
            Reference= new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{}
        }
    });
});
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<ITodoBusiness, ToDoBusiness>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);


var app = builder.Build();


CheckAndRunDatabaseScript(app.Configuration, app.Environment);

//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
                                        //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
    .AllowCredentials()); // allow credentials

builder.Services.AddCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void CheckAndRunDatabaseScript(IConfiguration configuration, IWebHostEnvironment env)
{
    string connectionString = configuration.GetConnectionString("DefaultConnection");
    string databaseName = "ToDoAppDb";
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        var command = new SqlCommand("SELECT database_id FROM sys.databases WHERE Name = @DatabaseName", connection);
        command.Parameters.AddWithValue("@DatabaseName", databaseName);
        var databaseId = command.ExecuteScalar();

        if (databaseId == null)
        {
            RunSqlScript(configuration, env);
        }
        // Diðer servis konfigürasyonlarý devam eder
    }
}

void RunSqlScript(IConfiguration configuration, IWebHostEnvironment env)
{
    string connectionString = configuration.GetConnectionString("DefaultConnection");
    string contentRootPath = env.ContentRootPath;
    string folderToRemove = "ToDoApp.WebApi";
    string modifiedPath = "";
    if (contentRootPath.EndsWith(folderToRemove))
    {
         modifiedPath = contentRootPath.Substring(0, contentRootPath.Length - folderToRemove.Length - 1);
        // -1, ayraç karakteri (\) için
        Console.WriteLine(modifiedPath); // Sonuç: C:\Users\iacar\source\repos\ToDoApp
    }

    string scriptPath = Path.Combine(modifiedPath, "ToDoApp.Business", "DatabaseBackUpScript","ToDoApp.sql");

    if (File.Exists(scriptPath))
    {
        string scriptContent = File.ReadAllText(scriptPath);
        string[] commands = scriptContent.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            foreach (var commandText in commands)
            {
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        // else durumunu ekle, dosya yoksa yapýlacak iþlemler
    }
}
