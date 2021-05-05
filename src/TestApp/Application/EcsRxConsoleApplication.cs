using SystemsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure;
using EcsRx.Infrastructure.DryIoc;
using EcsRx.Plugins.Batching;
using EcsRx.Plugins.Computeds;
using EcsRx.Plugins.GroupBinding;
using EcsRx.Plugins.Persistence;
using EcsRx.Plugins.ReactiveSystems;
using EcsRx.Plugins.ReactiveSystems.Extensions;
using EcsRx.Plugins.Views;

namespace TestApp.Application
{
    public abstract class EcsRxConsoleApplication : EcsRxApplication
    {
        public override IDependencyContainer Container { get; } = new DryIocDependencyContainer();

        protected override void LoadPlugins()
        {
            RegisterPlugin(new ReactiveSystemsPlugin());
            RegisterPlugin(new ComputedsPlugin());
            RegisterPlugin(new ViewsPlugin());
            RegisterPlugin(new BatchPlugin());
            RegisterPlugin(new PersistencePlugin());
            RegisterPlugin(new GroupBindingsPlugin());
        }

        protected override void StartSystems()
        {
            this.StartAllBoundReactiveSystems();
        }
    }
}