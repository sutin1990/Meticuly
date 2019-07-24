using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace mcore.Services
{
    public class UriConnection
    {   //Paisarn create 2018-07-13
        //return Web api address from Appsetting.Json http://192.168.5.167:5001/NetDiagram/API/NetDiagrams"
        // return//$"http://192.168.5.167:5001");
        public string GetUri()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile($"appsettings.json", true)
                      .AddEnvironmentVariables();
                var config = builder.Build();
                var uri = config.GetSection("ConnectionStrings").GetSection("ServiceUri").Value;
                return uri;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
