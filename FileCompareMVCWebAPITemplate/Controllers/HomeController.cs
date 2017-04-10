using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Tutorial.FileUpload.Models;

namespace FileCompareMVCWebAPITemplate.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult LoadFile1(TutorialModel tutorial)
		{
			var test = tutorial.Attachment1.InputStream;
			string result = new StreamReader(test).ReadToEnd();


			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult LoadFile2(TutorialModel tutorial)
		{
			var test = tutorial.Attachment2.InputStream;
			string result = new StreamReader(test).ReadToEnd();


			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult CompareFiles(TutorialModel tutorial)
		{
			var file1Content = tutorial.Attachment1.InputStream;
			var file2Content = tutorial.Attachment2.InputStream;
			string file1Result = new StreamReader(file1Content).ReadToEnd();
			string file2Result = new StreamReader(file2Content).ReadToEnd();

			byte[] fileBytes = new byte[tutorial.Attachment1.ContentLength];
			tutorial.Attachment1.InputStream.Read(fileBytes, 0, Convert.ToInt32(tutorial.Attachment1.ContentLength));

			var lines1 = file1Result.Split('\n');
			var lines2 = file2Result.Split('\n');

			var sourceFile = lines1.Length >= lines2.Length
					? CombineComparatoinLines(lines1, lines2, "+", "-", true)
					: CombineComparatoinLines(lines2, lines1, "-", "+", false);


			string html = GetMyTable<ComparerLine>(sourceFile, x => x.LineNumber, x => x.LineState, x => x.SourceFileLine, x => x.DestinationFileLine);
			return Json(html, JsonRequestBehavior.AllowGet);
		}

		private static List<ComparerLine> CombineComparatoinLines(IEnumerable<string> linesList1, IReadOnlyList<string> linesList2, string lineStateSign1, string lineStateSign2, bool sourceDestEquivalence)
		{
			int counter = 0;
			var returnedSourceFileLines = new List<ComparerLine>();

			foreach (var currentline in linesList1)
			{
				var comparerLine = new ComparerLine
				{
					LineNumber = counter
				};

				string currentSourceFilelineWtBreack;
				string currentDestFilelineWtBreack;

				if (linesList2.Count > counter)
				{
					currentSourceFilelineWtBreack = currentline.Replace("\r", string.Empty);
					currentDestFilelineWtBreack = linesList2[counter].Replace("\r", string.Empty);

					if (sourceDestEquivalence)
					{
						comparerLine.SourceFileLine = currentSourceFilelineWtBreack;
						comparerLine.DestinationFileLine = currentDestFilelineWtBreack;
					}
					else
					{
						comparerLine.SourceFileLine = currentDestFilelineWtBreack;
						comparerLine.DestinationFileLine = currentSourceFilelineWtBreack;
					}


					if (currentSourceFilelineWtBreack.Equals(currentDestFilelineWtBreack))
					{
						comparerLine.LineState = string.Empty;
					}
					else
					{
						if (!currentDestFilelineWtBreack.IsNullOrWhiteSpace()
							&& !currentSourceFilelineWtBreack.IsNullOrWhiteSpace()
							&& !currentSourceFilelineWtBreack.Equals(currentDestFilelineWtBreack))
						{
							comparerLine.LineState = "*";
						}
						else if (currentSourceFilelineWtBreack.IsNullOrWhiteSpace() && !currentDestFilelineWtBreack.IsNullOrWhiteSpace())
						{
							comparerLine.LineState = lineStateSign1;
						}
						else if (!currentSourceFilelineWtBreack.IsNullOrWhiteSpace() && currentDestFilelineWtBreack.IsNullOrWhiteSpace())
						{
							comparerLine.LineState = lineStateSign2;
						}
					}
				}
				else
				{
					comparerLine.LineState = lineStateSign2;

					currentSourceFilelineWtBreack = currentline.Replace("\r", string.Empty);
					currentDestFilelineWtBreack = string.Empty;

					if (sourceDestEquivalence)
					{
						comparerLine.SourceFileLine = currentSourceFilelineWtBreack;
						comparerLine.DestinationFileLine = currentDestFilelineWtBreack;
					}
					else
					{
						comparerLine.SourceFileLine = currentDestFilelineWtBreack;
						comparerLine.DestinationFileLine = currentSourceFilelineWtBreack;
					}
				}

				returnedSourceFileLines.Add(comparerLine);
				counter++;
			}

			return returnedSourceFileLines;
		}

		public static string GetMyTable<T>(IEnumerable<ComparerLine> list, params Func<ComparerLine, object>[] fxns)
		{

			StringBuilder sb = new StringBuilder();
			sb.Append("<TABLE>\n");
			foreach (var item in list)
			{
				if (item.LineState == "*")
				{
					sb.Append("<TR style=\'color:yellow\'>\n");
				}
				else if (item.LineState == "+")
				{
					sb.Append("<TR style=\'color:green\'>\n");
				}
				else if (item.LineState == "-")
				{
					sb.Append(@"<TR style='color:red'>\n");
				}
				else
				{
					sb.Append("<TR>\n");
				}

				foreach (var fxn in fxns)
				{
					sb.Append("<TD>");
					sb.Append(fxn(item));
					sb.Append("</TD>");
				}
				sb.Append("</TR>\n");
			}
			sb.Append("</TABLE>");

			return sb.ToString();
		}

		public class ComparerLine
		{
			public int LineNumber { get; set; }
			public string SourceFileLine { get; set; }
			public string DestinationFileLine { get; set; }
			public string LineState { get; set; }

		}
	}

}
