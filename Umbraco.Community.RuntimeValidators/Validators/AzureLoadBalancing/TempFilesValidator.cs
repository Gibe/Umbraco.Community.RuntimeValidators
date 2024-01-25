using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Configuration;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Infrastructure.Runtime;

namespace Umbraco.Community.RuntimeValidators.Validators.AzureLoadBalancing
{
	public class TempFilesValidator : IRuntimeModeValidator
	{
		private IOptionsMonitor<HostingSettings> _hostingSettings;

		public TempFilesValidator(IOptionsMonitor<HostingSettings> hostingSettings)
        {
			_hostingSettings = hostingSettings;

		}

        public bool Validate(RuntimeMode runtimeMode, [NotNullWhen(false)] out string? validationErrorMessage)
		{
			// Dont bother checking and say its OK to run if we are not in production mode
			if (runtimeMode != RuntimeMode.Production)
			{
				validationErrorMessage = null;
				return true;
			}

			// Umbraco TEMP files
			// https://docs.umbraco.com/umbraco-cms/fundamentals/setup/server-setup/load-balancing/azure-web-apps#umbraco-temp-files
			if (_hostingSettings.CurrentValue.LocalTempStorageLocation != LocalTempStorage.EnvironmentTemp)
			{
				validationErrorMessage = "Umbraco:CMS:Hosting:LocalTempStorage needs to be set to 'EnvironmentTemp' in production mode.";
				return false;
			}

			validationErrorMessage = null;
			return true;
		}

	}
}
