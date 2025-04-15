using System.ComponentModel.DataAnnotations;
using System.Web;

namespace INDIACom.Models
{
    public class RevisedPaperModel
    {
        // Hidden Field
        public int PaperId { get; set; }

        // Display fields
        public string EventName { get; set; }
        public string TrackName { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public int VersionNumber { get; set; }
        public string ReviewResult { get; set; }
        public string DetailedComments { get; set; }

        // Inputs
        [Display(Name = "Author's Comments")]
        [DataType(DataType.MultilineText)]
        public string AuthorsComments { get; set; }

        [Display(Name = "Submit Revision")]
        public bool SubmitRevision { get; set; }

        // File Uploads
        [Display(Name = "Master List of Revisions (PDF)")]
        public HttpPostedFileBase RevisionListFile { get; set; }

        [Display(Name = "Revised Paper (DOC/DOCX)")]
        public HttpPostedFileBase RevisedPaperFile { get; set; }

        [Display(Name = "Certificate of Originality (PDF)")]
        public HttpPostedFileBase OriginalityCertFile { get; set; }

        [Display(Name = "Copyright Transfer Form (PDF)")]
        public HttpPostedFileBase CopyrightFormFile { get; set; }

        [Display(Name = "Presentation (PPT/PPTX)")]
        public HttpPostedFileBase PresentationFile { get; set; }
    }
}
