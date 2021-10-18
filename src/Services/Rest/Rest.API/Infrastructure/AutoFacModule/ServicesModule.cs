using Autofac;
using Rest.API.Application.Services.RouleteMS;

namespace Rest.API.Infrastructure.AutoFacModule
{
    public class ServicesModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RouletteServices>().As<IRouletteServices>().AsImplementedInterfaces();
        }
    }
}
