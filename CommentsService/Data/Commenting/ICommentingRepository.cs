using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSocialBaz.Model.Enteties;

namespace TheSocialBaz.Data
{
    public interface ICommentingRepository
    {
        List<Comment> GetAllComments();

        List<Comment> GetCommentsByPostID(int postID, int userID);

        Comment GetCommentByID(Guid commentID);

        void CreateComment(Comment comment);

        void UpdateComment(Comment comment);

        void DeleteComment(Guid commentID);

        bool CheckDoIFollowSeller(int userID, int sellerID); //

        bool CheckDidIBlockedSeller(int userId, int sellerID); //

        public bool SaveChanges(); //
    }
}
