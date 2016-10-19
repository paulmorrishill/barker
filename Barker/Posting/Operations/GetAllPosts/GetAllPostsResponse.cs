using System;
using System.Collections.Generic;

namespace Barker.Posting.Operations.GetAllPosts
{
    public class GetAllPostsResponse
    {
        public List<Post> Posts = new List<Post>();

        public class Post
        {
            public string Content { get; set; }
            public string Id { get; set; }
            public DateTime DateMade { get; set; }
        }
    }
}