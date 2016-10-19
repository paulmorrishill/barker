using System.Collections.Generic;

namespace Barker.Posting
{
    public interface PostRepository
    {
        List<Post> GetAllPosts();

        /// <summary>
        /// Store a post and return the new ID
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        string AddPost(Post post);

        void DeletePost(string postId);
    }
}