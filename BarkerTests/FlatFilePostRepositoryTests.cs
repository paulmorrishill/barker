using System;
using System.IO;
using System.Security.Cryptography;
using Barker.Posting;
using NUnit.Framework;
using Should;

namespace BarkerTests
{
    public class FlatFilePostRepositoryTests
    {
        private string OtherPostsFile = null;
        private FlatFilePostRepository Repo;
        private string TestPostsFileName = null;

        [SetUp]
        public void SetUp()
        {
            TestPostsFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            OtherPostsFile = TestPostsFileName + "_other";

            Repo = new FlatFilePostRepository(TestPostsFileName);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(TestPostsFileName);
            File.Delete(OtherPostsFile);
        }

        [Test]
        public void GivenThereAreNoExistingPosts_CanReturnEmptyPosts()
        {
            Repo.GetAllPosts().Count.ShouldEqual(0);
        }

        [Test]
        public void CanSaveAPostAndThenGetItReturnIt()
        {
            var someDate = DateTime.Now;
            var theOriginalPost = new Post
            {
                Content = "This is a post",
                DateMade = someDate
            };

            var id = Repo.AddPost(theOriginalPost);

            var allPosts = Repo.GetAllPosts();
            allPosts.Count.ShouldEqual(1);
            allPosts[0].Id.ShouldEqual(id);
            AssertTwoPostsAreEquivalent(allPosts[0], theOriginalPost);
        }

        private static void AssertTwoPostsAreEquivalent(Post theOutputPost, Post theOriginalPost)
        {
            theOutputPost.Id.ShouldEqual(theOriginalPost.Id);
            theOutputPost.Content.ShouldEqual(theOriginalPost.Content);
            theOutputPost.DateMade.ShouldEqual(theOriginalPost.DateMade);
        }

        [Test]
        public void CanDeleteAPost()
        {
            Repo.DeletePost(AddPost("some post"));

            Repo.GetAllPosts().Count.ShouldEqual(0);
        }

        [Test]
        public void DoesNotDeletePostsThatDoNotMatchTheIdProvided()
        {
            AddPost("post 1");

            Repo.DeletePost("some-random-id");

            Repo.GetAllPosts()[0].Content.ShouldEqual("post 1");
        }

        [Test]
        public void PersistsNewPostsToTheFile()
        {
            AddPost("a post");
            AssertPostsArePersistedToFile();
        }

        [Test]
        public void PersistsDeletionsToTheFile()
        {
            var firstPostId = AddPost("some post");
            AddPost("some other post");

            Repo.DeletePost(firstPostId);
            AssertPostsArePersistedToFile();
        }

        private void AssertPostsArePersistedToFile()
        {
            File.Copy(TestPostsFileName, OtherPostsFile);
            var newRepo = new FlatFilePostRepository(OtherPostsFile);

            newRepo.GetAllPosts().Count.ShouldEqual(1);
            AssertTwoPostsAreEquivalent(Repo.GetAllPosts()[0], newRepo.GetAllPosts()[0]);
        }


        private string AddPost(string content)
        {
            return Repo.AddPost(new Post
            {
                Content = content
            });
        }
    }
}