using System;
using System.Collections;
using SystemsRx.Infrastructure.Dependencies;
using DryIoc;

namespace EcsRx.Infrastructure.DryIoc
{
    /// <summary>
    /// This is a ninject implementation for the dependency container.
    /// 
    /// As with all the dependency container implementations, you should implement
    /// a basic way to bind/resolve/resolveall but as some DI config may be more
    /// complex the underlying container should be exposed to the consumer so they
    /// can make use of native features if needed.
    /// 
    /// One thing to mention though is if any native container calls are used
    /// they will only be compatible with that dependency container, so when making
    /// plugins you ideally want to stick to the methods exposed on the interface
    /// to make your stuff cross platform.
    /// </summary>
    public class DryIocDependencyContainer : IDependencyContainer
    {
        private readonly Container _container;

        public DryIocDependencyContainer(Container container = null)
        { _container = container ?? new Container(); }

        public object NativeContainer => _container;
        
        public static object InitializerDecorator(object instance, IDependencyContainer container, Action<IDependencyContainer, object> action)
        {
            action(container, instance);
            return instance;
        }
        
        public void Bind(Type fromType, Type toType, BindingConfiguration configuration = null)
        {
            if (configuration == null)
            {
                _container.Register(fromType, toType, Reuse.Singleton);
                return;
            }
            
            var reuseOption = configuration.AsSingleton ? Reuse.Singleton : Reuse.Transient;
            
            if (configuration.ToInstance != null)
            { _container.UseInstance(fromType, configuration.ToInstance, IfAlreadyRegistered.Replace); }
            else if (configuration.ToMethod != null)
            { _container.Use(fromType, x => configuration.ToMethod(this)); }
            else
            {
                var parameters = Parameters.Of;
                foreach (var constructorArg in configuration.WithNamedConstructorArgs)
                { parameters = parameters.Name(constructorArg.Key, x => constructorArg.Value); }
            
                foreach (var constructorArg in configuration.WithTypedConstructorArgs)
                { parameters = parameters.Type(constructorArg.Key, x => constructorArg.Value); }
                var made = Made.Of(parameters: parameters);

                Setup setup = null;
                if (configuration.WhenInjectedInto.Count != 0)
                {
                    foreach (var type in configuration.WhenInjectedInto)
                    { setup = Setup.With(condition: request => request.Parent.ServiceType.IsAssignableTo(type)); }
                }
                
                _container.Register(fromType, toType, reuseOption, made, setup, IfAlreadyRegistered.AppendNewImplementation, configuration.WithName);
                
                if (configuration.OnActivation != null)
                {
                    _container.Register(fromType, reuseOption,
                        Made.Of(() => InitializerDecorator(Arg.Of(fromType, IfUnresolved.Throw), this,
                                configuration.OnActivation)), Setup.Decorator);
                }
            }
        }

        public void Bind(Type type, BindingConfiguration configuration = null)
        { Bind(type, type, configuration); }

        public bool HasBinding(Type type, string name = null)
        {
            if(string.IsNullOrEmpty(name))
            { return _container.IsRegistered(type); }

            return _container.IsRegistered(type, name);
        }

        public object Resolve(Type type, string name = null)
        {
            if(string.IsNullOrEmpty(name))
            { return _container.Resolve(type); }

            return _container.Resolve(type, name);
        }

        public void Unbind(Type type)
        { _container.Unregister(type); }

        public IEnumerable ResolveAll(Type type)
        { return _container.ResolveMany(type); }

        public void LoadModule(IDependencyModule module)
        { module.Setup(this); }

        public void Dispose()
        { _container?.Dispose(); }
    }
}