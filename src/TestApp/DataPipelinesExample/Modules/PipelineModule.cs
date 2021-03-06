using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using LazyData.Json;
using LazyData.Json.Handlers;
using TestApp.DataPipelinesExample.Pipelines;

namespace TestApp.DataPipelinesExample.Modules
{
    public class PipelineModule : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            // By default only the binary stuff is loaded, but you can load json, yaml, bson etc
            container.Bind<IJsonPrimitiveHandler, BasicJsonPrimitiveHandler>();
            container.Bind<IJsonSerializer, JsonSerializer>();
            container.Bind<IJsonDeserializer, JsonDeserializer>();
            
            // Register our custom pipeline using the json stuff above
            container.Bind<PostJsonHttpPipeline>();
        }
    }
}