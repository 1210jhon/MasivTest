using Autofac;
using Rest.API.Infrastructure.Repositories.BoardRepository;
using Rest.API.Infrastructure.Repositories.RouletteRepository;

namespace Rest.API.Infrastructure.AutoFacModule
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RouletteRepository>().As<IRouletteRepository>().AsImplementedInterfaces();
            builder.RegisterType<BoardRepository>().As<IBoardRepository>().AsImplementedInterfaces();
        }
    }
}
