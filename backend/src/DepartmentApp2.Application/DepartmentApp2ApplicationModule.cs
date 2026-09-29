using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DepartmentApp2
{
    [DependsOn(typeof(DepartmentApp2CoreModule), typeof(AbpAutoMapperModule))]
    public class DepartmentApp2ApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.AddMaps(typeof(DepartmentApp2ApplicationModule).GetAssembly());
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DepartmentApp2ApplicationModule).GetAssembly());
        }
    }
}
