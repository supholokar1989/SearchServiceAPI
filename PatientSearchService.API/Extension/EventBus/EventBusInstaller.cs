using Autofac;
using iPas.Infrastructure.EventBus;
using iPas.Infrastructure.EventBus.Abstractions;
using iPas.Infrastructure.EventBusServiceBus;
using iPas.Infrastructure.IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;

namespace PatientSearchService.API
{
    public static class EventBusInstaller
    {
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));


            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                var serviceBusConnectionString = configuration["EventBusConnection"];
                var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];

            services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                    eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
            });


            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
