using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barker.Posting.Operations;
using Barker.Posting.Operations.CreatePost;
using Barker.Posting.Operations.DeletePost;
using Barker.Posting.Operations.GetAllPosts;
using Nancy;
using Newtonsoft.Json;
using Owin;

namespace BarkerApi
{
    public class PostController : NancyModule
    {

        public PostController(GetAllPostsOperation getAll)
        {
            Get["/posts"] = _ => JsonConvert.SerializeObject(getAll.Execute().Posts);
        }

    }
}
