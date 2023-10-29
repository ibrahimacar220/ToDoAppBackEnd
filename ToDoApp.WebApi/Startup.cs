using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ToDoApp.WebApi
{
    public class Startup
    {
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            string databaseName = "ToDoAppDb";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT database_id FROM sys.databases WHERE Name = @DatabaseName", connection);
                command.Parameters.AddWithValue("@DatabaseName", databaseName);

                var databaseId = command.ExecuteScalar();

                if (databaseId == null)
                {
                    // Veritabanı yok, scripti çalıştır
                    RunSqlScript();
                }
                else
                {
                    // Veritabanı zaten varsa başka bir şey yapabilirsiniz veya hiçbir şey yapmayabilirsiniz.
                }
            }

            // Diğer servis konfigürasyonları devam eder
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Diğer yapılandırma adımları devam eder
        }

        private void RunSqlScript()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string scriptPath = Path.Combine(_env.ContentRootPath, "Business\\ToDoApp.Business.SqlServer\\DatabaseBackUpScript\\ToDoApp.sql");
            string script = File.ReadAllText(scriptPath);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(script, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}

