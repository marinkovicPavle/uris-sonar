using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheSocialBaz.Model
{
    /// <summary>
    /// DTO comment model
    /// </summary>
    [Table("Commenting")]
    public class CommentingDto
    {
        /// <summary>
        /// Comment ID
        /// </summary>
        public Guid CommentID { get; set; }

        /// <summary>
        /// Post ID to which the comment refers
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Account ID that posted comment
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Comment content
        /// </summary>
        [Required(ErrorMessage = "Sadržaj komentara je obavezan.")]
        public String CommentText { get; set; }

        /// <summary>
        /// Date when comment was posted
        /// </summary>
        [Required(ErrorMessage = "Datum je obavezan.")]
        public DateTime CommentDate { get; set; }
    }
}
