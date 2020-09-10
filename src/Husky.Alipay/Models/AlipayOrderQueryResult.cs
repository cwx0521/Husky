﻿using Alipay.AopSdk.Core.Response;

namespace Husky.Alipay
{
	public class AlipayOrderQueryResult : Result
	{
		public string? AlipayTradeNo { get; internal set; }
		public string? AlipayBuyerUserId { get; internal set; }
		public string? AlipayBuyerLogonId { get; internal set; }
		public decimal TotalAmount { get; internal set; }

		public AlipayTradeQueryResponse? OriginalResult { get; internal set; }
	}
}
