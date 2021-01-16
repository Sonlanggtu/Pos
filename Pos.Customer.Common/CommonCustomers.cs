using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pos.Customer.Common
{
    public static class CommonCustomers
    {
        public static string GetVarEverionmentByKey(string key)
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
                string currentTarget = currentDirectory.Replace("Infrastructure", "WebApi");
                Console.WriteLine("currentTarget >>" + currentTarget);

                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(currentTarget)
                                       .AddJsonFile("appsettings.json",optional: true, reloadOnChange: true)
                                       .AddJsonFile("appsettings.Development.json", optional: true)
                                       .Build();
                var result = configuration.GetConnectionString(key);

                return result;
            }
        }
    }
}
