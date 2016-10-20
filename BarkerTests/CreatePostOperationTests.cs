using System;
using Barker;
using Barker.Posting.Operations;
using Barker.Posting.Operations.CreatePost;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace BarkerTests
{
    public class CreatePostOperationTests
    {
        private CreatePostOperationImpl Create;
        private DateTime CurrentTime;
        private InMemoryPostRepository PostRepository;

        [SetUp]
        public void SetUp()
        {
            CurrentTime = new DateTime(2016, 01, 01, 12, 34, 00);
            PostRepository = new InMemoryPostRepository();

            var mockCurrentTimeProvider = Substitute.For<CurrentTimeProvider>();
            mockCurrentTimeProvider.GetCurrentTime().Returns(CurrentTime);

            Create = new CreatePostOperationImpl(PostRepository, mockCurrentTimeProvider);
        }

        [Test]
        public void GivenTheUserPostsAValidPost_ItCreatesThePostAndReturnsSuccessful()
        {
            AssertCanPostAPostWithContent("this is a new post");
            AssertCanPostAPostWithContent(CreateAValidPostWithLength(149));
        }

        [Test]
        public void GivenTheUserProvidesAnEmptyPostContent_ItFailsAndDoesNotCreateAnyPosts()
        {
            AssertThatTheReponseIndicatesPostContentWasEmpty(Create.Execute(""));
            AssertThatTheReponseIndicatesPostContentWasEmpty(Create.Execute(null));
            AssertThatTheReponseIndicatesPostContentWasEmpty(Create.Execute("    "));
            AssertThatTheReponseIndicatesPostContentWasEmpty(Create.Execute("\t"));
        }

        [Test]
        public void GivenTheUserTriesToPostAMessageLongerThan150Characters_ItFailsAndDoesNotCreateAnyPosts()
        {
            var contentLongerThan150Characters = CreateAValidPostWithLength(151);

            var response = Create.Execute(contentLongerThan150Characters);

            response.Successful.ShouldBeFalse();
            response.ErrorPostContentTooLong.ShouldBeTrue();
        }

        private void AssertCanPostAPostWithContent(string postContent)
        {
            var response = Create.Execute(postContent);

            PostRepository.LastSavedPost.Content.ShouldEqual(postContent);
            PostRepository.LastSavedPost.DateMade.ShouldEqual(CurrentTime);
            response.Successful.ShouldBeTrue();
            response.PostId.ShouldEqual(PostRepository.LastPostId);
        }

        private static string CreateAValidPostWithLength(int length)
        {
            var thePost = "";
            for (var i = 0; i < length; i++)
                thePost += "a";
            return thePost;
        }

        private void AssertThatTheReponseIndicatesPostContentWasEmpty(CreatePostResponse response)
        {
            response.Successful.ShouldBeFalse();
            response.ErrorPostContentEmpty.ShouldBeTrue();
            PostRepository.LastSavedPost.ShouldBeNull();
        }
    }
}