# EcsRx.Infrastructure.DryIoc

This is just an implementation for DryIoc to act as a provider within the EcsRx Infrastructure layer.

## How do I use it?

Just grab latest version off nuget and then change your applications DI provider like so:
```c#
public override IDependencyContainer Container { get; } = new DryIocDependencyContainer();
```