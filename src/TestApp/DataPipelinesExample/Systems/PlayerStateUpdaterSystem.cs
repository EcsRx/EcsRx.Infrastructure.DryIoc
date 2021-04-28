using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Scheduling;
using EcsRx.Systems;
using TestApp.DataPipelinesExample.Components;

namespace TestApp.DataPipelinesExample.Systems
{
    public class PlayerStateUpdaterSystem : IBasicSystem
    {
        public IGroup Group { get; } = new Group(typeof(PlayerStateComponent));
        
        public ITimeTracker TimeTracker { get; }

        public PlayerStateUpdaterSystem(ITimeTracker timeTracker)
        {
            TimeTracker = timeTracker;
        }

        public void Process(IEntity entity)
        {
            var playerState = entity.GetComponent<PlayerStateComponent>();
            playerState.PlayTime += TimeTracker.ElapsedTime.DeltaTime;
        }
    }
}