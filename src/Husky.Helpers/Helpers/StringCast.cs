﻿using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Husky
{
	public static class StringCast
	{
		public static int AsInt(this string? str, int defaultValue = default) => int.TryParse(str, out var i) ? i : defaultValue;
		public static long AsInt64(this string? str, long defaultValue = default) => long.TryParse(str, out var i) ? i : defaultValue;
		public static decimal AsDecimal(this string? str, decimal defaultValue = default) => decimal.TryParse(str, out var i) ? i : defaultValue;
		public static bool AsBool(this string? str, bool defaultValue = default) => bool.TryParse(str, out var b) ? b : defaultValue;
		public static Guid AsGuid(this string? str, Guid defaultValue = default) => Guid.TryParse(str, out var g) ? g : defaultValue;

		public static T As<T>(this string? str, T defaultValue = default) where T : struct {
			if ( str == null ) {
				return default;
			}
			if ( typeof(T) == typeof(Guid) ) {
				return (T)(object)str.AsGuid((Guid)(object)defaultValue);
			}
			try {
				if ( typeof(IConvertible).IsAssignableFrom(typeof(T)) ) {
					return (T)Convert.ChangeType(str, typeof(T));
				}
				return JsonSerializer.Deserialize<T>(str);
			}
			catch {
				return defaultValue;
			}
		}

		public static int HexToInt(this string hex) {
			if ( string.IsNullOrEmpty(hex) ) {
				throw new ArgumentNullException(nameof(hex));
			}

			hex = hex.ToLower();
			if ( hex.StartsWith("0x") ) {
				hex = hex.Substring(2);
			}

			var result = 0;
			var charArray = hex.Reverse().ToArray();

			for ( var i = 0; i < charArray.Length; i++ ) {
				int num;
				var c = charArray[i];

				if ( c >= 'a' && c <= 'f' ) {
					num = 10 + (c - 'a');
				}
				else if ( c >= '0' && c <= '9' ) {
					num = (c - '0');
				}
				else {
					throw new FormatException($"{hex} is not a valid hex number string.");
				}
				result += num * (int)Math.Pow(16, i);
			}
			return result;
		}

		public static string? Mask(this string? str) {
			if ( string.IsNullOrWhiteSpace(str) ) {
				return str;
			}
			if ( str.Length == 2 || str.Length == 3 ) {
				return $"{str[0]}{new string('*', str.Length - 1)}";
			}
			if ( str.IsCellphone() ) {
				return $"{str.Substring(0, 3)}****{str.Substring(7)}";
			}
			if ( str.IsEmail() ) {
				var at = str.IndexOf('@');
				return $"{str.Substring(0, 1)}{new string('*', at - 1)}{str.Substring(at)}";
			}
			if ( str.IsCardNumber() ) {
				return $"{str.Substring(0, 4)}{new string('*', str.Length - 8)}{str.Substring(str.Length - 4)}";
			}
			if ( str.IsSocialIdNumber() ) {
				return $"{str.Substring(0, 6)}{new string('*', str.Length - 8)}{str.Substring(str.Length - 4)}";
			}
			return str;
		}

		public static string? BetterDisplayCardNumber(this string? str) {
			if ( string.IsNullOrEmpty(str) ) {
				return str;
			}
			var sb = new StringBuilder();
			for ( var i = 0; i < str.Length; i++ ) {
				if ( i != 0 && i % 4 == 0 ) {
					sb.Append(' ');
				}
				sb.Append(str[i]);
			}
			return sb.ToString();
		}

		public static string? TextEncode(this string? str) {
			return str == null
				? null
				: new StringBuilder(str)
					.Replace("&", "&amp;")
					.Replace("<", "&lt;")
					.Replace(">", "&gt;")
					.Replace("\"", "&quot;")
					.Replace("\'", "&#39;")
					.ToString();
		}

		public static string? HtmlEncode(this string? str) {
			return str == null
				? null
				: new StringBuilder(str)
					.Replace("&", "&amp;")
					.Replace("<", "&lt;")
					.Replace(">", "&gt;")
					.Replace("\"", "&quot;")
					.Replace("\'", "&#39;")
					.Replace("\t", "&nbsp; &nbsp; ")
					.Replace("\r", "")
					.Replace("\n", "<br />")
					.ToString();
		}
	}
}