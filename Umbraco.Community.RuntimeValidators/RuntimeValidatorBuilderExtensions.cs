using Umbraco.Cms.Infrastructure.Runtime;
using Umbraco.Community.RuntimeValidators.Validators.AzureLoadBalancing;

namespace Umbraco.Community.RuntimeValidators
{
	public static class RuntimeValidatorBuilderExtensions
	{
		public static RuntimeModeValidatorCollectionBuilder AddAzureLoadBalancingValidators(this RuntimeModeValidatorCollectionBuilder builder)
		{
				return builder
					.Add<TempFilesValidator>()
					.Add<HostSyncValidator>()
					.Add<ExamineValidator>();
		}
	}
}
