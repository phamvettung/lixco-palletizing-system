using Intech.NarimePalletizingSystem.UI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.UI.Configurations
{
    public static class AppConfig
    {
        public static PlcConfig? GetPlcConfig()
        {
            PlcConfig config = new PlcConfig();
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).Build();
            config.CpuType = S7.Net.CpuType.S71200;
            config.IpAddress = configuration["plc:ipAddress"];
            bool rackRet, slotRet; int Rack, Slot;
            rackRet = int.TryParse(configuration["plc:rack"], out Rack);
            slotRet = int.TryParse(configuration["plc:slot"], out Slot);
            config.Rack = (short)Rack;
            config.Slot = (short)Slot;
            return config;
        }

        public static Account GetAccount()
        {
            Account account = new Account();
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).Build();
            account.userName = configuration["account:userName"];
            account.passWord = configuration["account:passWord"];
            return account;
        }

        public static TscConfig GetTscConfig()
        {
            TscConfig config = new TscConfig();
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).Build();
            config.Port = configuration["tsc:port"];
            config.Size = configuration["tsc:size"];
            config.Speed = configuration["tsc:speed"];
            config.Density = configuration["tsc:density"];
            config.Direction = configuration["tsc:direction"];
            config.Tear = configuration["tsc:tear"];
            config.Codepage = configuration["tsc:codepage"];
            return config;
        }
    }
}
