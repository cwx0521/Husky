﻿using System;
using System.Threading.Tasks;
using Husky.Diagnostics.Data;
using Husky.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Husky.Diagnostics
{
	public sealed class DiagnosticsLogger : IDiagnosticsLogger
	{
		public DiagnosticsLogger(IDiagnosticsDbContext db, IHttpContextAccessor? httpContextAccessor, IPrincipalUser? principal) {
			_me = principal;
			_db = db;
			_http = httpContextAccessor;
		}

		private readonly IPrincipalUser? _me;
		private readonly IDiagnosticsDbContext _db;
		private readonly IHttpContextAccessor? _http;

		public async Task LogExceptionAsync(Exception e) => await _db.LogExceptionAsync(e, _http?.HttpContext, _me);
		public async Task LogRequestAsync() => await _db.LogRequestAsync(_http?.HttpContext, _me);
		public async Task LogOperationAsync(LogLevel logLevel, string message) => await _db.LogOperationAsync(logLevel, message, _me);
		public async Task LogPageViewAsync(string pageName, string? description = null) => await _db.LogPageViewAsync(pageName, description, _http?.HttpContext, _me);
	}
}
