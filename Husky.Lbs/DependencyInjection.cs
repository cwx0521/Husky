﻿using Husky.Lbs;
using Microsoft.Extensions.DependencyInjection;

namespace Husky.DependencyInjection
{
	public static class DependencyInjection
	{
		public static HuskyDependencyInjectionHub AddLbs(this HuskyDependencyInjectionHub husky, string key) {
			husky.Services.AddSingleton<ILbs>(new QQLbs(key));
			return husky;
		}
	}
}