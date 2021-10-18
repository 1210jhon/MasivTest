using Autofac;
namespace Rest.API.Infrastructure.AutoFacModule
{
    public class QueriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<RouletteQueries>().As<IRouletteQueries>().AsImplementedInterfaces();
        }
    }
}
