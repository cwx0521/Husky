﻿using System;
using Husky.KeyValues;
using Husky.Lbs;
using Husky.Mail;
using Husky.TwoFactor;
using Microsoft.Extensions.DependencyInjection;

namespace Husky
{
	public class HuskyInjector
	{
		internal HuskyInjector(IServiceCollection services) {
			Services = services;
		}

		public IServiceCollection Services { get; }


		public HuskyInjector AddKeyValueManager<TImplement>() where TImplement : class, IKeyValueManager {
			Services.AddScoped<IKeyValueManager, TImplement>();
			return this;
		}
		public HuskyInjector AddKeyValueManager<TImplement>(Func<IServiceProvider, TImplement> implementationFactory) where TImplement : class, IKeyValueManager {
			Services.AddScoped<IKeyValueManager, TImplement>(implementationFactory);
			return this;
		}


		public HuskyInjector AddLbs<TImplement>() where TImplement : class, ILbs {
			Services.AddSingleton<ILbs, TImplement>();
			return this;
		}
		public HuskyInjector AddLbs<TImplement>(Func<IServiceProvider, TImplement> implementationFactory) where TImplement : class, ILbs {
			Services.AddSingleton<ILbs, TImplement>(implementationFactory);
			return this;
		}


		public HuskyInjector AddMailSender<TImplement>() where TImplement : class, IMailSender {
			Services.AddScoped<IMailSender, TImplement>();
			return this;
		}
		public HuskyInjector AddMailSender<TImplement>(Func<IServiceProvider, TImplement> implementationFactory) where TImplement : class, IMailSender {
			Services.AddScoped<IMailSender, TImplement>(implementationFactory);
			return this;
		}


		public HuskyInjector AddTwoFactor<TImplement>() where TImplement : class, ITwoFactorManager {
			Services.AddScoped<ITwoFactorManager, TImplement>();
			return this;
		}
		public HuskyInjector AddTwoFactor<TImplement>(Func<IServiceProvider, TImplement> implementationFactory) where TImplement : class, ITwoFactorManager {
			Services.AddScoped<ITwoFactorManager, TImplement>(implementationFactory);
			return this;
		}
	}
}
