using System;
using System.Collections.Generic;
using Barker;
using Barker.Posting;
using Barker.Posting.Operations;
using Barker.Posting.Operations.GetAllPosts;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace BarkerTests
{
    public class GetAllPostsOperationTests
    {
        private GetAllPostsOperationImpl GetAllPostsOperationImpl;
        private PostRepository MockPostRepository;
        private DateTime Yesterday;
        private DateTime Today;
        private DateTime TheDayBeforeYesterday;

        [SetUp]
        public void SetUp()
        {
            MockPostRepository = Substitute.For<PostRepository>();
            GetAllPostsOperationImpl = new GetAllPostsOperationImpl(MockPostRepository);
            Yesterday = DateTime.Today.AddHours(-27);
            TheDayBeforeYesterday = DateTime.Today.AddHours(-49);
            Today = DateTime.Now;
        }

        [Test]
        public void GivenThereAreNoPosts_ThenItReturnsAnEmptyListOfPosts()
        {
            MockPostRepository.GetAllPosts().Returns(new List<Post>());
            GetAllPostsOperationImpl.Execute().Posts.Count.ShouldEqual(0);
        }

        [Test]
        public void GivenThereAreSomePosts_ThenItReturnsThePosts()
        {
            MockPostRepository.GetAllPosts().Returns(new List<Post>
            {
                new Post
                {
                    Id = "2541",
                    Content = "Barker is great",
                    DateMade = Today
                },
                new Post
                {
                    Id = "5152",
                    Content = "Test post",
                    DateMade = Yesterday
                }
            });

            var posts = GetAllPostsOperationImpl.Execute().Posts;
            posts.Count.ShouldEqual(2);

            var firstPost = posts[0];
            firstPost.Content.ShouldEqual("Barker is great");
            firstPost.Id.ShouldEqual("2541");
            firstPost.DateMade.ShouldEqual(Today);

            var secondPost = posts[1];
            secondPost.Content.ShouldEqual("Test post");
            secondPost.Id.ShouldEqual("5152");
            secondPost.DateMade.ShouldEqual(Yesterday);
        }

        [Test]
        public void GivenThereAreSomePosts_ThenItReturnsThePostsInDateOrder()
        {
            MockPostRepository.GetAllPosts().Returns(new List<Post>
            {
                CreateAPost("2", Yesterday),
                CreateAPost("1", Today),
                CreateAPost("3", TheDayBeforeYesterday)
            });

            var posts = GetAllPostsOperationImpl.Execute().Posts;

            posts[0].Id.ShouldEqual("1");
            posts[1].Id.ShouldEqual("2");
            posts[2].Id.ShouldEqual("3");
        }

        private Post CreateAPost(string id, DateTime dateMade)
        {
            return new Post
            {
                Id = id,
                DateMade = dateMade
            };
        }

    }
}