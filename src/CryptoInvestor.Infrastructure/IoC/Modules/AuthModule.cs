using Autofac;
using CryptoInvestor.Infrastructure.Auth;

namespace CryptoInvestor.Infrastructure.IoC.Modules
{
    public class AuthModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Encrypter>()
                   .As<IEncrypter>()
                   .SingleInstance();

            builder.RegisterType<JwtHandler>()
                   .As<IJwtHandler>()
                   .SingleInstance();
        }
    }
}