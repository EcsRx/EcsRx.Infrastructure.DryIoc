using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SystemsRx.Systems.Conventional;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Plugins.GroupBinding.Attributes;
using EcsRx.Systems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestApp.DataPipelinesExample.Components;
using TestApp.DataPipelinesExample.Events;
using TestApp.DataPipelinesExample.Pipelines;

namespace TestApp.DataPipelinesExample.Systems
{
    public class TriggerPipelineSystem : IReactToEventSystem<SavePipelineEvent>, IGroupSystem
    {
        public IGroup Group => new Group(typeof(PlayerStateComponent));

        [FromGroup]
        public IObservableGroup ObservableGroup;
        
        public PostJsonHttpPipeline SaveJsonPipeline { get; }

        public TriggerPipelineSystem(PostJsonHttpPipeline saveJsonPipeline)
        {
            SaveJsonPipeline = saveJsonPipeline;
        }
        
        public void Process(SavePipelineEvent eventData)
        {
            var entity = ObservableGroup.Single();
            var playerState = entity.GetComponent<PlayerStateComponent>();
            Task.Run(() => TriggerPipeline(playerState));
        }

        public async Task TriggerPipeline(PlayerStateComponent playerState)
        {
            var httpResponse = (HttpResponseMessage) await SaveJsonPipeline.Execute(playerState);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var prettyResponse = MakeDataPretty(responseContent);
            Console.WriteLine($"Server Responded With {prettyResponse}");
        }

        // Feel free to output everything in the JToken if you want, only showing data for simplicity
        public string MakeDataPretty(string jsonData)
        { return JToken.Parse(jsonData)["data"].ToString(Formatting.Indented); }
    }
}