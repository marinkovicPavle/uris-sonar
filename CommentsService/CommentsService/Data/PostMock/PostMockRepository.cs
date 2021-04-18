using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSocialBaz.Model.Mock;

namespace TheSocialBaz.Data.PostMock
{
    public class PostMockRepository : IPostMockRepository
    {
        public static List<PostDto> Postst { get; set; } = new List<PostDto>();

        public PostMockRepository()
        {
            FillData();
        }

        private void FillData()
        {
            Postst.AddRange(new List<PostDto>
            {
                new PostDto
                {
                    PostID = 1,
                    AccountID = 2,
                    PostName = "Fiat Stilo 2002",
                    PostImage = "https://s1.cdn.autoevolution.com/images/models/FIAT_Stilo-5-Door-2001_main.jpg",
                    Description = "Fiat stilo dizel, registrovan do kraja godine.",
                    Price = 1350,
                    Currency = "Euro",
                    Category = "Automobili",
                    PublicationDate = DateTime.Parse("2021-02-01T09:00:00")
                },
                new PostDto
                {
                    PostID = 2,
                    AccountID = 4,
                    PostName = "Iphone 8",
                    PostImage = "https://www.winwin.rs/media/catalog/product/420x420/682/68/6826893-1.jpg",
                    Description = "Nov iphone 8 64gb.",
                    Price = 220,
                    Currency = "Euro",
                    Category = "Mobilni telefoni",
                    PublicationDate = DateTime.Parse("2021-01-15T11:00:00")
                },
                new PostDto
                {
                    PostID = 3,
                    AccountID = 5,
                    PostName = "Sony playstation 5",
                    PostImage = "https://www.gizmochina.com/wp-content/uploads/2020/09/PlayStation-5.jpg",
                    Description = "Playstation 5 sa 4 dzoistika.",
                    Price = 500,
                    Currency = "Euro",
                    Category = "Konzole",
                    PublicationDate = DateTime.Parse("2021-03-11T10:00:00")
                },
                new PostDto
                {
                    PostID = 4,
                    AccountID = 9,
                    PostName = "Asus rog laptop",
                    PostImage = "https://www.notebookcheck.net/uploads/tx_nbc2/csm_GL504_Hero_Cam05Lighting_v1_740f45a0ab_03.jpg",
                    Description = "Najnoviji model asus rog laptopa.",
                    Price = 999,
                    Currency = "Euro",
                    Category = "Laptopovi",
                    PublicationDate = DateTime.Parse("2021-04-02T15:00:00")
                }
            });
        }

        public PostDto GetPostByID(int ID)
        {
            return Postst.FirstOrDefault(e => e.PostID == ID);
        }
    }
}
