using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheSocialBaz.Model.Mock
{
    /// <summary>
    /// DTO Post model
    /// </summary>
    public class PostDto
    {
        /// <summary>
        /// Post ID
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Account ID
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Post name
        /// </summary>
        public String PostName { get; set; }

        /// <summary>
        /// Post picture
        /// </summary>
        public String PostImage { get; set; }

        /// <summary>
        /// Post Description
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Post price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Post price currency
        /// </summary>
        public String Currency { get; set; }

        /// <summary>
        /// Post category
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// Post creation date
        /// </summary>
        public DateTime PublicationDate { get; set; }
    }
}
