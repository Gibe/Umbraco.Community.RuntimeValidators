using Umbraco.Cms.Infrastructure.Runtime;
using Umbraco.Community.RuntimeValidators.Validators.AzureLoadBalancing;

namespace Umbraco.Community.RuntimeValidators
{
	public static class RuntimeValidatorBuilderExtensions
	{
		public static RuntimeModeValidatorCollectionBuilder AddAzureLoadBalancingValidators(this RuntimeModeValidatorCollectionBuilder builder)
		{
			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID")))
			{
				return builder
					.Add<TempFilesValidator>()
					.Add<HostSyncValidator>()
					.Add<ExamineValidator>();
			}

			return builder;
		}
	}
}
