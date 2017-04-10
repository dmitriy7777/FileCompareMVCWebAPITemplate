using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tutorial.FileUpload.Models
{
    public class TutorialModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
		public string Description1 { get; set; }
		public HttpPostedFileBase Attachment1 { get; set; }
		public HttpPostedFileBase Attachment2 { get; set; }
	}
}