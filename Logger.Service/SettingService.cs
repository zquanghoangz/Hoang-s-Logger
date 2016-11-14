using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger.Domain;
using Logger.Service.Interfaces;

namespace Logger.Service
{
    public class SettingService : ServiceBase<Setting>, ISettingService
    {
        public SettingService(DbContext context) : base(context)
        {
        }
    }
}
