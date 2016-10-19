using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barker;
using Barker.Posting;
using Barker.Posting.Operations;
using Barker.Posting.Operations.CreatePost;
using Barker.Posting.Operations.DeletePost;
using Barker.Posting.Operations.GetAllPosts;
using NUnit.Framework;
using Should;

namespace BarkerTests
{
    public class PostsIntegrationTests
    {
        [Test]
        public void GeneralCrudOperationsOnPosts_IntegrationTest()
        {
            var someFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var postStorage = new FlatFilePostRepository(someFile);

            var createPost = new CreatePostOperation(postStorage, new ServerTimeProvider());
            var deletePost = new DeletePostOperation(postStorage);
            var getAllPosts = new GetAllPostsOperation(postStorage);

            createPost.Execute("Just signed up");
            createPost.Execute("this is pretty cool");
            var oneToDelete = createPost.Execute("going to delete this one");
            createPost.Execute("getting bored now");

            deletePost.Execute(oneToDelete.PostId);

            var allPosts = getAllPosts.Execute().Posts;

            allPosts.Count.ShouldEqual(3);
            allPosts[0].Content.ShouldEqual("getting bored now");
            allPosts[1].Content.ShouldEqual("this is pretty cool");
            allPosts[2].Content.ShouldEqual("Just signed up");
        }
    }
}