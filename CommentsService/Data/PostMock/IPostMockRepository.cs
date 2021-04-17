using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSocialBaz.Model.Mock;

namespace TheSocialBaz.Data.PostMock
{
    public interface IPostMockRepository
    {
        PostDto GetPostByID(int ID);
    }
}
