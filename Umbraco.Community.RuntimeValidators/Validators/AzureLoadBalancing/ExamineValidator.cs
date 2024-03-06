using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Sync;
using Umbraco.Cms.Infrastructure.Runtime;

namespace Umbraco.Community.RuntimeValidators.Validators.AzureLoadBalancing
{
	public class ExamineValidator : IRuntimeModeValidator
	{
		private readonly IOptionsMonitor<IndexCreatorSettings> _indexCreatorSettings;
		private readonly IServerRoleAccessor _serverRoleAccessor;

		public ExamineValidator(IOptionsMonitor<IndexCreatorSettings> indexCreatorSettings, IServerRoleAccessor	serverRoleAccessor)
		{
			_indexCreatorSettings = indexCreatorSettings;
			_serverRoleAccessor = serverRoleAccessor;
		}

		public bool Validate(RuntimeMode runtimeMode, [NotNullWhen(false)] out string? validationErrorMessage)
		{
			// Dont bother checking and say its OK to run if we are not in production mode
			if (runtimeMode != RuntimeMode.Production)
			{
				validationErrorMessage = null;
				return true;
			}

			// Examine and Lucene Config
			// https://docs.umbraco.com/umbraco-cms/fundamentals/setup/server-setup/load-balancing/azure-web-apps#lucene-examine-configuration
			var currentServerRole = _serverRoleAccessor.CurrentServerRole;

			// Front End Scaling Servers
			if (currentServerRole == ServerRole.Subscriber)
			{
				if (_indexCreatorSettings.CurrentValue.LuceneDirectoryFactory != LuceneDirectoryFactory.TempFileSystemDirectoryFactory)
				{
					validationErrorMessage = "Umbraco:CMS:Examine:LuceneDirectoryFactory needs to be set to 'TempFileSystemDirectoryFactory' in production mode for Subscriber (Front End) servers.";
					return false;
				}
			}
			// Single CMS/Admin Server
			else if (currentServerRole == ServerRole.SchedulingPublisher)
			{
				if (_indexCreatorSettings.CurrentValue.LuceneDirectoryFactory != LuceneDirectoryFactory.SyncedTempFileSystemDirectoryFactory && _indexCreatorSettings.CurrentValue.LuceneDirectoryFactory != LuceneDirectoryFactory.TempFileSystemDirectoryFactory)
				{
					validationErrorMessage = "Umbraco:CMS:Examine:LuceneDirectoryFactory needs to be set to 'SyncedTempFileSystemDirectoryFactory' or 'TempFileSystemDirectoryFactory' in production mode for SchedulingPublisher (Admin) server.";
					return false;
				}
			}
			else
			{
				validationErrorMessage = "The Server Role is not set to Subscriber or SchedulingPublisher";
				return false;
			}

			validationErrorMessage = null;
			return true;
		}
	}
}
