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
using Nancy.ModelBinding;
using Newtonsoft.Json;
using Owin;

namespace BarkerApi
{
    public class PostController : NancyModule
    {

        public PostController(GetAllPostsOperation getAll, DeletePostOperation delete, CreatePostOperation create)
        {
            Get["/posts"] = _ => JsonConvert.SerializeObject(getAll.Execute().Posts);

            Delete["/posts/{id}"] = _ =>
            {
                delete.Execute(_.id.ToString());
                return HttpStatusCode.OK;
            };

            Post["/posts"] = _ => JsonConvert.SerializeObject(create.Execute(this.Bind<CreateRequest>().Content));
        }

        private class CreateRequest
        {
            public string Content;
        }
    }
}
