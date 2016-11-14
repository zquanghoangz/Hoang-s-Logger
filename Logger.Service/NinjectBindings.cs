using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger.Domain;
using Logger.Service.Interfaces;
using Ninject.Modules;

namespace Logger.Service
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<DbContext>().To<CrossoverEntities>();
            Bind<IApplicationService>().To<ApplicationService>();
            Bind<ILogService>().To<LogService>();
            Bind<ISettingService>().To<SettingService>();
        }
    }
}
