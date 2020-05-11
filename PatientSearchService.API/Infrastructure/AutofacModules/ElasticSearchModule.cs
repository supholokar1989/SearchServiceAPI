using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class ElasticSearchModule : Autofac.Module
    {
        public string ConnectionString { get; }
        public string IndexName { get; }

        public ElasticSearchModule(string qconstr, string indexname)
        {
            ConnectionString = qconstr;
            IndexName = indexname;

        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => new ElasticSearchClient(ConnectionString, IndexName))
            .AsSelf()
            .SingleInstance();

            //builder.RegisterType<ElasticSearchService>()
            //    .As<IElasticSearchService>()
            //    .InstancePerLifetimeScope();

            builder.Register(c => new PatientSearchRepository(c.Resolve<ElasticSearchClient>(), IndexName))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();



        }
    }
}
