using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDIACom.Models
{
    public class CoAuthorModel
    {
        public int CoAuthorId { get; set; } // ID of the Co-author
        public string MemberId { get; set; }
        public int PaperId { get; set; }     // ID of the Paper this co-author is linked to
        public bool IsCorresponding { get; set; }  // Flag to mark the corresponding author
    }

}