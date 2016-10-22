using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barker;
using Barker.Posting;
using Barker.Posting.Operations.CreatePost;
using Barker.Posting.Operations.DeletePost;
using Barker.Posting.Operations.GetAllPosts;
using BarkerApi;
using Microsoft.Owin.Hosting;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var postRepo = new FlatFilePostRepository("local.txt");
            var create = new CreatePostOperationImpl(postRepo, new ServerTimeProvider());
            var delete = new DeletePostOperationImpl(postRepo);
            var get = new GetAllPostsOperationImpl(postRepo);

            var postController = new PostController(get, delete, create);
            ApiControllerCatalog.AddController(postController);

            using (WebApp.Start<ApiStartup>("http://localhost:8080/"))
            {
                Console.WriteLine("Listening on 8080, open the UI to begin the barking experience. Press enter to close.");
                Console.ReadLine();
            }
        }
    }
}
