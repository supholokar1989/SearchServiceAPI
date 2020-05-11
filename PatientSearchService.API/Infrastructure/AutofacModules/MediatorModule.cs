using Autofac;
using MediatR;
using PatientSearchService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
           
            builder.RegisterAssemblyTypes(typeof(SearchPatientDetailsQuery).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(SearchPatientQueryHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder
                .RegisterAssemblyTypes(typeof(CreateUpdatePatientDetailsInESCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));



            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });


        }
    }
}
