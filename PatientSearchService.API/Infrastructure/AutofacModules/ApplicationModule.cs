using Autofac;
using iPas.Infrastructure.EventBus.Abstractions;
using System.Reflection;

namespace PatientSearchService.API
{
    public class ApplicationModule : Autofac.Module
    {
        public string GRPCRegistrationURL { get; }

        public ApplicationModule(string registrationURL)
        {
            GRPCRegistrationURL = registrationURL;
        }

        protected override void Load(ContainerBuilder builder)
        {


            builder.RegisterType<PatientSearchRepository>()
                .As<IPatientSearchRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(PatientsDetailsMessageReceivedEventHandler).GetTypeInfo().Assembly)
                 .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

            builder.Register(c => new RegistrationGRPCClientService(GRPCRegistrationURL))
                .As<IRegistrationGRPCClientService>()
                .InstancePerLifetimeScope();


        }
    }
}
