using System;
using System.Collections.Generic;
using Barker.Posting;

namespace BarkerTests
{
    public class InMemoryPostRepository : PostRepository
    {
        public string LastPostId = "";
        public List<Post> GetAllPosts()
        {
            return new List<Post>();
        }

        public string AddPost(Post post)
        {
            LastSavedPost = post;
            var postId = Guid.NewGuid().ToString();
            LastPostId = postId;
            return postId;
        }

        public void DeletePost(string postId)
        {
            throw new System.NotImplementedException();
        }

        public Post LastSavedPost { get; set; }
    }
}