﻿using Appdoon.Application.Interfaces;
using Appdoon.Application.Validatores.LessonValidatore;
using Appdoon.Common.Dtos;
using Appdoon.Domain.Entities.RoadMaps;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appdoon.Application.Services.Lessons.Command.CreateLessonService
{
	public interface ICreateLessonService
	{
		ResultDto Execute(HttpRequest httpRequest, string currentpath);
	}

	public class CreateLessonService : ICreateLessonService
	{
		private readonly IDatabaseContext _context;

		public CreateLessonService(IDatabaseContext context)
		{
			_context = context;
		}
		public ResultDto Execute(HttpRequest httpRequest, string currentpath)
		{
			try
			{
				List<string> data = new List<string>();

				foreach (var key in httpRequest.Form.Keys)
				{
					var val = httpRequest.Form[key];
					data.Add(val);
				}

				var Title = data[0];
				var Text = data[1];
				var PhotoFileName = data[2];

				//Uniqueness(Title)
				if (_context.Lessons.Where(s => s.Title == Title.ToString()).Count() != 0)
				{
					return new ResultDto()
					{
						IsSuccess = false,
						Message = "این نام برای مقاله تکراری است",
					};
				}

				var imageSrc = "";

				if (httpRequest.Form.Files.Count() != 0)
				{
					var postedFile = httpRequest.Form.Files[0];
					string filename = postedFile.FileName;
					var physicalPath = currentpath + "/Photos/Lesson/" + $"({Title})" + filename;
					using (var stream = new FileStream(physicalPath, FileMode.Create))
					{
						postedFile.CopyTo(stream);
					}
					imageSrc = $"({Title})" + PhotoFileName.ToString();
				}
				else
				{
					PhotoFileName = "1.jpg";
					imageSrc = PhotoFileName;
				}




				var lesson = new Lesson()
				{
					Title = Title.ToString(),
					Text = Text.ToString(),
					TopBannerSrc = imageSrc
				};

				_context.Lessons.Add(lesson);
				_context.SaveChanges();

				return new ResultDto()
				{
					IsSuccess = true,
					Message = "مقاله ساخته شد",
				};
            }
			catch (Exception e)
			{
				return new ResultDto()
				{
					IsSuccess = false,
					Message = "خطا در ساخت مقاله!",
				};
			}
		}
	}

	public class CreateLessonDto
	{
		public string Title { get; set; } = string.Empty;
		public string TopBannerSrc { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
	}
}
