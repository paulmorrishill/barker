using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Barker.Posting.Operations;
using Barker.Posting.Operations.CreatePost;
using Barker.Posting.Operations.DeletePost;
using Barker.Posting.Operations.GetAllPosts;

namespace BarkerApi
{
    public class BarkerApi : ApiController
    {
        private CreatePostOperation CreatePost;
        private GetAllPostsOperation GetAll;
        private DeletePostOperation DeletePost;

        public BarkerApi(CreatePostOperation createPost, GetAllPostsOperation getAll, DeletePostOperation deletePost)
        {
            DeletePost = deletePost;
            GetAll = getAll;
            CreatePost = createPost;
        }

    }
}
