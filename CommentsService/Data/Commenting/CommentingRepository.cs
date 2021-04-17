using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSocialBaz.Model.Enteties;

namespace TheSocialBaz.Data
{
    public class CommentingRepository : ICommentingRepository
    {
        private readonly DBContext context;

        public CommentingRepository(DBContext DBcontext)
        {
            context = DBcontext;
        }

        public bool CheckDidIBlockedSeller(int userId, int sellerID)
        {
            throw new NotImplementedException();
        }

        public bool CheckDoIFollowSeller(int userID, int sellerID)
        {
            throw new NotImplementedException();
        }

        public void CreateComment(Comment comment)
        {
            context.Comments.Add(comment);
        }

        public void DeleteComment(Guid commentID)
        {
            var comment = GetCommentByID(commentID);
            context.Remove(comment);
        }

        public List<Comment> GetAllComments()
        {
            if (context.Database.CanConnect())
            {
                Console.WriteLine("Bravo");
            } else
            {
                Console.WriteLine("Sranje");
            }
            return context.Comments.ToList();
        }

        public Comment GetCommentByID(Guid commentID)
        {
            return context.Comments.FirstOrDefault(e => e.CommentID == commentID);
        }

        public List<Comment> GetCommentsByPostID(int postID, int userID)
        {
            var query = from comment in context.Comments
                        where comment.PostID == postID
                        select comment;

            return query.ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateComment(Comment comment)
        {
            var a = context.Update(comment);
            context.Update(comment);
        }
    }
}
