using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.WebApplication.Common
{
    public static class WebApplicationCommon
    {
        public const string Success = "ok";
        public const string Warning = "warning";
        public const string NotFound = "not-found";
        public const string ErrorSystem = "error-system";

        public class UserClaim
        {
            public const string Roles = "Roles";
            public const string Id = "Id";
            public const string Permissions = "permissions";
            public const string FullName = "fullName";
        }

        public static string GetEnvByKey(string key)
        {
            var everionmentDocker = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(everionmentDocker))
            {
                Console.WriteLine("key >>>" + key);
                return everionmentDocker;

            }
            else
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                Console.WriteLine("currentTarget >>" + currentDirectory);

                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(currentDirectory)
                                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                       .AddJsonFile("appsettings.Development.json", optional: true)
                                       .Build();

                var result = configuration.GetSection(key).Value;
                return result;
            }
        }

        public static string GetEnvConnection(string key)
        {
            var everionmentDocker = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(everionmentDocker))
            {
                Console.WriteLine("key >>>" + key);
                return everionmentDocker;
            }
            else
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                //string currentTarget = currentDirectory.Replace("Infrastructure", "WebApi");
                Console.WriteLine("currentTarget >>" + currentDirectory);
                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(currentDirectory)
                                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                       .AddJsonFile("appsettings.Development.json", optional: true)
                                       .Build();
                var result = configuration.GetConnectionString(key);
                return result;
            }
        }
    }
}
