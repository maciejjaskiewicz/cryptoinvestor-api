using Autofac;
using CryptoInvestor.Infrastructure.Services;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Reflection;

namespace CryptoInvestor.Infrastructure.IoC.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ServiceModule).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IService>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<DataInitializer>()
                   .As<IDataInitializer>()
                   .SingleInstance();

            builder.RegisterType<CoinsProvider>()
                   .As<ICoinsProvider>()
                   .SingleInstance();
        }
    }
}