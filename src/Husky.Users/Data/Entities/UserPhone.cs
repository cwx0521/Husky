﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Husky.Users.Data
{
	public class UserPhone
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int UserId { get; set; }

		[MaxLength(11), Column(TypeName = "varchar(11)"), Unique]
		public string Number { get; set; } = null!;

		public bool IsVerified { get; set; }

		[DefaultValueSql("getdate()"), NeverUpdate]
		public DateTime CreatedTime { get; set; } = DateTime.Now;


		// nav props

		[JsonIgnore]
		public User User { get; set; } = null!;
	}
}
