using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DepartmentApp2.EntityFrameworkCore;

namespace DepartmentApp2.Web.Host
{
    [DependsOn(typeof(DepartmentApp2ApplicationModule), typeof(DepartmentApp2EntityFrameworkCoreModule), typeof(AbpAspNetCoreModule))]
    public class DepartmentApp2WebHostModule : AbpModule
    {
        public override void PreInitialize()
        {
            // Expose all AppServices as dynamic API controllers
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(DepartmentApp2ApplicationModule).GetAssembly(),
                    moduleName: "app",
                    useConventionalHttpVerbs: true
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DepartmentApp2WebHostModule).GetAssembly());
        }
    }
}
