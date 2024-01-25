# Umbraco.Community.RuntimeValidators
A community project of [Umbraco runtime validators](https://docs.umbraco.com/umbraco-cms/fundamentals/setup/server-setup/runtime-modes) to add to your projects to ensure your site has the correct configuration.

## Validators

### Azure Load Balancing
* `AzureLoadBalancing.ExamineValidator` - XX
* `AzureLoadBalancing.HostSyncValidator` - XY
* `AzureLoadBalancing.TempFilesValidator` - ZZ

## How to use
Install the NuGet package `Umbraco.Community.RuntimeValidators` into your Umbraco project.

You can add the validators to the collection of RuntimeValidators found in the Umbraco Application builder. You can do this directly in your startup.cs file or in a composer as shown in the various ways below.

```csharp
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.RuntimeValidators.Validators.AzureLoadBalancing;
using Umbraco.Extensions;

namespace YourProject.Website

public class RuntimeValidatorsComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		builder.RuntimeModeValidators()
			.Add<TempFilesValidator>()
			.Add<HostSyncValidator>()
			.Add<ExamineValidator>();
	}
}
```

Alternatively you can use the extension method `AddAzureLoadBalancingValidators()` to add all the validators in the Azure Load Balancing namespace.

```csharp
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.RuntimeValidators;
using Umbraco.Extensions;

namespace YourProject.Website;

public class RuntimeValidatorsComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		builder.RuntimeModeValidators()
			.AddAzureLoadBalancingValidators();
	}
}
```

Or if you want fine grain control you can add each validator individually.

```csharp
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace YourProject.Website;

public class RuntimeValidatorsComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		builder.RuntimeModeValidators()
			.Add<Umbraco.Community.RuntimeValidators.Validators.AzureLoadBalancing.ExamineValidator>()
			.Add<Umbraco.Community.RuntimeValidators.Validators.AzureLoadBalancing.HostSyncValidator>();
	}
}
```

## Don't Forget
You will need to set the Runtime Mode in your `appsettings.json` file to `Production` for the current validators to run alongside Umbraco's own.
For further information and documentation about [Umbraco runtime modes, please refer to their documentation](https://docs.umbraco.com/umbraco-cms/fundamentals/setup/server-setup/runtime-modes)

```json
{
	"Umbraco": {
		"CMS": {
			"Runtime": {
				"Mode" : "Production"
            }
		}
	}
}
```


## Requirements
This class library is built against Umbraco 10.1.0 where the concept of RuntimeValidators was introduced, so your Umbraco build will need to use
