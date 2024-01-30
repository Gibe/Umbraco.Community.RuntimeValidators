using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Infrastructure.Runtime;

namespace Umbraco.Community.RuntimeValidators.Validators.AzureLoadBalancing
{
	public class HostSyncValidator : IRuntimeModeValidator
	{
		private readonly IOptionsMonitor<GlobalSettings> _globalSettings;

		public HostSyncValidator(IOptionsMonitor<GlobalSettings> globalSettings)
		{
			_globalSettings = globalSettings;
		}

		public bool Validate(RuntimeMode runtimeMode, [NotNullWhen(false)] out string? validationErrorMessage)
		{
			// Dont bother checking and say its OK to run if we are not in production mode
			if (runtimeMode != RuntimeMode.Production)
			{
				validationErrorMessage = null;
				return true;
			}

			// Host Sync & MainDomLock
			// https://docs.umbraco.com/umbraco-cms/fundamentals/setup/server-setup/load-balancing/azure-web-apps#host-synchronization
			// This needs to be for the CMS/Admin (Scheduling Publisher) AND the scaling public front end servers (Subscriber)
			if (_globalSettings.CurrentValue.MainDomLock.Equals("FileSystemMainDomLock", StringComparison.InvariantCultureIgnoreCase) == false)
			{
				validationErrorMessage = "Umbraco:CMS:Global:MainDomLock needs to be set to 'FileSystemMainDomLock' in production mode.";
				return false;
			}

			validationErrorMessage = null;
			return true;
		}
	}
}
