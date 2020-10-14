﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace Husky.Principal
{
	public class PrincipalUser : Identity, IIdentity, IPrincipalUser, IPrincipalAdmin
	{
		private PrincipalUser(IServiceProvider serviceProvider) {
			ServiceProvider = serviceProvider;
			IdentityManager = serviceProvider.GetService<IIdentityManager>();
		}

		public PrincipalUser(IIdentityManager identityManager, IServiceProvider serviceProvider) {
			var identity = identityManager.ReadIdentity();
			if ( identity != null && identity.IsAuthenticated ) {

				Id = identity.Id;
				DisplayName = identity.DisplayName;

				identityManager.SaveIdentity(this);
			}

			IdentityManager = identityManager;
			ServiceProvider = serviceProvider;
		}

		public static PrincipalUser Personate(Identity identity, IServiceProvider serviceProvider) {
			return new PrincipalUser(serviceProvider) {
				Id = identity.Id,
				DisplayName = identity.DisplayName
			};
		}

		public static PrincipalUser Personate(int id, string displayName, IServiceProvider serviceProvider) {
			return new PrincipalUser(serviceProvider) {
				Id = id,
				DisplayName = displayName
			};
		}

		public IIdentityManager IdentityManager { get; }
		public IServiceProvider ServiceProvider { get; }
	}
}