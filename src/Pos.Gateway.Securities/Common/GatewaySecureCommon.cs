using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Gateway.Securities.Common
{
    public static class GatewaySecureCommon
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


        #region Funtion Common
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
                string currentTarget = Directory.GetCurrentDirectory();
                Console.WriteLine("currentTarget >>" + currentTarget);

                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(currentTarget)
                                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                       .AddJsonFile("appsettings.Development.json", optional: true)
                                       .Build();
                var result = configuration.GetConnectionString(key);

                return result;
            }
        }
        #endregion

    }
}
