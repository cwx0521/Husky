﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Husky.GridQuery
{
	public static class GridColumnBuilder
	{
		internal const int DefaultGridColumnWidth = 160;

		public static string Json(this List<GridColumn> columns) {
			return JsonSerializer.Serialize(columns, new JsonSerializerOptions(JsonSerializerDefaults.Web) {
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
			});
		}

		public static List<GridColumn> GetGridColumns<TGridModel>() => typeof(TGridModel).GetGridColumns();
		public static List<GridColumn> GetGridColumns(this Type typeOfGridModel) {
			if ( typeOfGridModel == null ) {
				throw new ArgumentNullException(nameof(typeOfGridModel));
			}

			var properties = typeOfGridModel.GetProperties();
			var columns = new List<GridColumn>();
			var pending = new List<Tuple<GridColumn, GridColumnAttribute>>();

			// read the properties and create columns one by one
			// when attribute.DisplayAfter is set, the columns will be added later on
			foreach ( var p in properties ) {
				var attribute = p.GetCustomAttribute<GridColumnAttribute>();
				if ( attribute != null && !attribute.Visible ) {
					continue;
				}
				if ( attribute != null && !string.IsNullOrEmpty(attribute.DisplayAfter) ) {
					var tuple = new Tuple<GridColumn, GridColumnAttribute>(
						BuildGridColumn(p, attribute),
						attribute
					);
					pending.Add(tuple);
				}
				columns.Add(BuildGridColumn(p, attribute));
			}

			// now insert columns which have DisplayAfter
			foreach ( var each in pending ) {
				var insertAt = columns.FindIndex(x => x.Field == each.Item2.DisplayAfter) + 1;
				columns.Insert(insertAt, each.Item1);
			}

			// grouping
			var result = new List<GridColumn>();
			foreach ( var col in columns ) {
				if ( string.IsNullOrEmpty(col.Category) ) {
					result.Add(col);
					continue;
				}
				var category = result.Find(x => x.Title == col.Category);
				if ( category == null ) {
					category = new GridColumn {
						Title = col.Category,
						Columns = new List<GridColumn>()
					};
					result.Add(category);
				}
				category.Columns!.Add(col);
			}

			return result;
		}

		private static GridColumn BuildGridColumn(PropertyInfo property, GridColumnAttribute? attr) => new GridColumn {
			Field = property?.Name.CamelCase(),
			Title = attr?.Title ?? property?.Name?.SplitWordsByCapital(),
			Category = attr?.Category,
			Width = (attr == null || attr.Width == 0) ? DefaultGridColumnWidth : (attr.Width != -1 ? attr.Width : (int?)null),
			Template = attr?.Template ?? GetTemplateString(attr, property?.Name),
			Format = attr?.Format,
			Aggregates = attr?.Aggregates.ToNameArray(),
			Filterable = property != null && (attr?.Filterable ?? true),
			Sortable = property != null && (attr?.Sortable ?? true),
			EditableFlag = property != null && (attr?.Editable ?? false),
			Hidable = (attr?.Hidable ?? true),
			Groupable = (attr?.Groupable ?? false),
			Locked = (attr?.Locked ?? false),
			Hidden = (attr?.Hidden ?? false),
			Attributes = attr?.CssClass == null ? null : $" class='{attr.CssClass}'",
			Type = property?.GetMappedJavaScriptType(),
			Values = property?.GetEnumerableValues(),
		};

		private static string? CamelCase(this string? fieldName) {
			if ( string.IsNullOrEmpty(fieldName) ) {
				return fieldName;
			}
			return fieldName[0].ToString() + fieldName[1..];
		}

		private static string[]? ToNameArray(this Enum? values) {
			return values?.ToString().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}

		private static string? GetMappedJavaScriptType(this PropertyInfo property) {
			var t = !property.PropertyType.IsGenericType
				? property.PropertyType
				: property.PropertyType.GenericTypeArguments[0];

			if ( t == typeof(DateTime) ) {
				return "date";
			}
			if ( t == typeof(int) || t == typeof(long) || t == typeof(uint) || t == typeof(decimal) || t == typeof(double) || t == typeof(float) ) {
				return "number";
			}
			return null;
		}

		private static SelectListItem[]? GetEnumerableValues(this PropertyInfo property) {
			var t = !property.PropertyType.IsGenericType
				? property.PropertyType
				: property.PropertyType.GenericTypeArguments[0];

			if ( !t.IsEnum ) {
				return null;
			}
			var result = new List<SelectListItem>();
			var values = Enum.GetValues(t);
			foreach ( var i in values ) {
				result.Add(new SelectListItem {
					Text = (i as Enum)!.ToLabel(),
					Value = i!.ToString()
				});
			}
			return result.ToArray();
		}

		private static string? GetTemplateString(GridColumnAttribute? attr, string? fieldName) {
			if ( attr == null || string.IsNullOrEmpty(fieldName) ) {
				return null;
			}
			if ( !string.IsNullOrEmpty(attr.LinkUrl) && attr.KnownTemplate == GridColumnTemplate.None ) {
				attr.KnownTemplate = GridColumnTemplate.HyperLink;
			}
			switch ( attr.KnownTemplate ) {
				default:
					return null;

				case GridColumnTemplate.HyperLink:
					return $"#if ({fieldName}) {{# <a href='{attr.LinkUrl}'>#:{fieldName}#</a> #}} #";

				case GridColumnTemplate.Modal:
				case GridColumnTemplate.ModalLG:
				case GridColumnTemplate.ModalXL:
				case GridColumnTemplate.ModalFull:
					var size = attr.KnownTemplate.ToLower().Substring(5);
					var fragment1 = $"<a " +
						$"href='{attr.LinkUrl}' " +
						$"data-ajax='husky' " +
						$"data-toggle='modal' " +
						$"data-target='\\#modal' " +
						$"data-modal-title='#:{fieldName} || '{fieldName}'#'" +
						$"data-modal-size='modal-{size}'>" +
						$"#:{fieldName}#" +
					$"</a>";
					return $"#if ({fieldName}) {{# {fragment1} #}} #";

				case GridColumnTemplate.CheckBox:
					var fragment2 = "<input " +
						$"type='checkbox' name='idCollection' value='#:Id#' class='grid-row-checkbox' " +
						$"# {fieldName} == {(int)CheckBoxState.Checked} ? 'checked' : '' # " +
						$"# {fieldName} == {(int)CheckBoxState.Disabled} ? 'disabled' : '' # " +
					"/>";
					return $"#if ({fieldName} !== {(int)CheckBoxState.NoDisplay}) {{# {fragment2} #}} #";

				case GridColumnTemplate.Date:
					return $"#: $.toDateString({fieldName}) #";

				case GridColumnTemplate.DateTime:
					return $"#: $.toDateTimeString({fieldName}) #";

				case GridColumnTemplate.TimeAgo:
					return $"#: $.toTimeAgoString({fieldName}) #";

				case GridColumnTemplate.TimeElapsed:
					return $"#: $.toTimeElapsedString({fieldName}) #";

				case GridColumnTemplate.ZeroAsEmpty:
					return string.IsNullOrEmpty(attr.Format)
						? $"#: {fieldName} == 0 ? '' : {fieldName} #"
						: $"#: {fieldName} == 0 ? '' : kendo.toString({fieldName}, '{attr.Format}') #";
			}
		}
	}
}