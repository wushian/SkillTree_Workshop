﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Workshop.Areas.Backend.Models
{
	public class LogonViewModel
	{
		[Required]
		[Display(Name = "帳號")]
		public string Account { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "密碼")]
		public string Password { get; set; }

		[Display(Name = "記住我")]
		public bool Remember { get; set; }
	}
}