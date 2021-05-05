# EcsRx.Infrastructure.DryIoc

[![Build Status][build-status-image]][build-status-url]
[![Nuget Version][nuget-image]][nuget-url]

This is just an implementation for DryIoc to act as a provider within the EcsRx Infrastructure layer.

## How do I use it?

Just grab latest version off nuget and then change your applications DI provider like so:
```c#
public override IDependencyContainer Container { get; } = new DryIocDependencyContainer();
```

[build-status-image]: https://ci.appveyor.com/api/projects/status/4so7w42epso4ujfd?svg=true
[build-status-url]: https://ci.appveyor.com/project/grofit/ecsrx-infrastructure-dryioc/branch/master
[nuget-image]: https://img.shields.io/nuget/v/ecsrx.infrastructure.dryioc.svg
[nuget-url]: https://www.nuget.org/packages/EcsRx.Infrastructure.DryIoc/
