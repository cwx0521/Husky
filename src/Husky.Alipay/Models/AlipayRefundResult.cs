﻿using Alipay.AopSdk.Core.Response;

namespace Husky.Alipay.Models
{
	public class AlipayRefundResult : Result
	{
		public decimal AggregatedRefundAmount { get; internal set; }

		public AlipayTradeRefundResponse? OriginalResult { get; internal set; }
	}
}
