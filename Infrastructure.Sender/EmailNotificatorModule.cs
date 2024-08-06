using Autofac;

namespace Infrastructure.Sender
{
    public class EmailNotificatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailNotificator>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

        }
    }
}
