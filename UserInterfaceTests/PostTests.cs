using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Barker.Posting.Operations.CreatePost;
using Barker.Posting.Operations.DeletePost;
using Barker.Posting.Operations.GetAllPosts;
using BarkerApi;
using Microsoft.Owin.Hosting;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Owin;
using Should;
using BarkerApi = BarkerApi.PostController;

namespace UserInterfaceTests
{
    public class PostTests : GetAllPostsOperation, DeletePostOperation, CreatePostOperation
    {
        private const string TheFirstPostsContent = "Post 1 content";
        private static ChromeDriver WebDriver;
        private IDisposable Api;
        private List<GetAllPostsResponse.Post> CurrentPosts;
        private string LastCreatedPost;
        private CreatePostResponse NextCreatePostResponse;
        private bool RespondingToCreateRequestsIsSuspended;
        private const string TheSecondPostContent = "This is another post";
        private const string TheFirstPostId = "post1";
        private const string PleaseProvideSomethingToBarkErrorMessage = "Please provide something to bark!";
        private const string PostTooLongErrorMessage = "Pleast keep your barks under 150 characters.";
        private int NumberOfCreateRequestsRecieved;

        [OneTimeSetUp]
        public static void SetUpOnceForAllTests()
        {
            WebDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService());
            WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
        }

        [OneTimeTearDown]
        public static void TearDownOnceForAllTests()
        {
            WebDriver.Close();
            WebDriver.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            RespondingToCreateRequestsIsSuspended = false;
            NumberOfCreateRequestsRecieved = 0;

            CurrentPosts = new List<GetAllPostsResponse.Post>
            {
                new GetAllPostsResponse.Post
                {
                    Content = TheFirstPostsContent,
                    DateMade = DateTime.Now.AddMinutes(-5),
                    Id = TheFirstPostId
                },
                new GetAllPostsResponse.Post
                {
                    Content = TheSecondPostContent,
                    DateMade = DateTime.Now.AddMinutes(-60)
                }
            };
            NextCreatePostResponse = new CreatePostResponse();

            ApiControllerCatalog.Reset();
            ApiControllerCatalog.AddController(new PostController(this, this, this));
            Api = WebApp.Start<ApiStartup>("http://localhost:8080/");
        }

        [TearDown]
        public void TearDown()
        {
            Api.Dispose();
        }

        [Test]
        public void CanReadTheLatestPostsThroughTheUi()
        {
            NavigateToTheHomePage();

            PageShouldShowText(TheFirstPostsContent);
            PageShouldShowText("5 minutes ago");
            PageShouldShowText(TheSecondPostContent);
            PageShouldShowText("an hour ago");
        }

        [Test]
        public void CanDeleteAPostThroughTheUi()
        {
            NavigateToTheHomePage();

            WebDriver.FindElementById("delete-post-" + TheFirstPostId).Click();

            PageShouldNotShowText(TheFirstPostsContent);
            PageShouldShowText(TheSecondPostContent);
        }

        [Test]
        public void CanCreateANewPostThroughTheUserInterface()
        {
            var theNewPostText = "This is a cool new post";
            TryToSubmitPost(theNewPostText);
            NextCreatePostResponse.Successful = true;

            WaitUntil(() => LastCreatedPost == theNewPostText);
            PageShouldShowText(theNewPostText);
            AssertThePostTextBoxIsEmpty();
        }

        [Test]
        public void DoesNotAttemptToCreateThePostLoadsOfTimesIfTheUserClicksTheButtonLoadsOfTimes()
        {
            SuspendRespondingToCreateRequests();
            TryToSubmitPost("this is a post here");
            ClickTheSubmitPostButton();
            ClickTheSubmitPostButton();
            ResumeRespondingToCreateRequests();
            Thread.Sleep(1000);
            NumberOfCreateRequestsRecieved.ShouldEqual(1);
        }

        [Test]
        public void CanPostMultipleSequentialPosts()
        {
            NextCreatePostResponse.Successful = true;
            TryToSubmitPost("this is a post here");

            WaitUntil(() => NumberOfCreateRequestsRecieved == 1);

            SetThePostTextBoxContentTo("this is the second post");
            ClickTheSubmitPostButton();

            WaitUntil(() => NumberOfCreateRequestsRecieved == 2);
        }

        [Test]
        public void CanCreateAPostWithThatIncludesALineFeed()
        {
            var contentWithLineFeed = "some post\r\noh look here we are";
            NextCreatePostResponse.Successful = true;
            TryToSubmitPost(contentWithLineFeed);
            PageShouldNotShowText(contentWithLineFeed);
        }

        [Test]
        public void ShowsAMessageIfThePostContentIsTooLong()
        {
            NextCreatePostResponse.ErrorPostContentTooLong = true;
            TryToSubmitPost("message");
            PageShouldShowText(PostTooLongErrorMessage);
        }

        [Test]
        public void ItShowsAnErrorMessageWhenTheUserProvidesAnEmptyPost()
        {
            NextCreatePostResponse.ErrorPostContentEmpty = true;
            TryToSubmitPost("");
            PageShouldShowText(PleaseProvideSomethingToBarkErrorMessage);
        }

        [Test]
        public void ItHidesThePreviousErrorWhenTheUserStartsTypingInThePostBox()
        {
            NextCreatePostResponse.ErrorPostContentEmpty = true;
            TryToSubmitPost("");
            PageShouldShowText(PleaseProvideSomethingToBarkErrorMessage);
            SetThePostTextBoxContentTo("a");
            PageShouldNotShowText(PleaseProvideSomethingToBarkErrorMessage);
        }

        private void SuspendRespondingToCreateRequests()
        {
            RespondingToCreateRequestsIsSuspended = true;
        }

        private void ResumeRespondingToCreateRequests()
        {
            RespondingToCreateRequestsIsSuspended = false;
            Thread.Sleep(100);
        }

        private static void TryToSubmitPost(string theNewPostText)
        {
            NavigateToTheHomePage();
            SetThePostTextBoxContentTo(theNewPostText);
            ClickTheSubmitPostButton();
        }

        private static void ClickTheSubmitPostButton()
        {
            WebDriver.FindElementById("submit-post").Click();
        }

        private static void SetThePostTextBoxContentTo(string theNewPostText)
        {
            WebDriver.FindElementById("post-content").SendKeys(theNewPostText);
        }

        private static void AssertThePostTextBoxIsEmpty()
        {
            WebDriver.FindElementById("post-content").GetAttribute("value").ShouldBeEmpty();
        }

        private void PageShouldNotShowText(string text)
        {
            WaitUntil(() => GetNumberOfElementsOnPageWithText(text) == 0);
        }

        private static int GetNumberOfElementsOnPageWithText(string text)
        {
            return WebDriver.FindElements(By.XPath($"//*[contains(text(), '{text}')]")).Count;
        }

        private static void NavigateToTheHomePage()
        {
            WebDriver.Navigate().GoToUrl("http://localhost:9000/");
        }

        private static void PageShouldShowText(string text)
        {
            WaitUntil(() => GetNumberOfElementsOnPageWithText(text) > 0);
        }

        private static void WaitUntil(Func<bool> thingToBecomeTrue, int timeout = 2000)
        {
            var start = DateTime.Now;
            while (!thingToBecomeTrue())
            {
                var durationWaited = DateTime.Now - start;
                if(durationWaited.TotalMilliseconds > timeout) throw new Exception("Took too long to become true.");
                Thread.Sleep(200);
            }
        }

        public GetAllPostsResponse Execute()
        {
            return new GetAllPostsResponse
            {
                Posts = CurrentPosts
            };
        }

        void DeletePostOperation.Execute(string postId)
        {
            CurrentPosts = CurrentPosts.Where(post => post.Id != postId).ToList();
        }

        CreatePostResponse CreatePostOperation.Execute(string content)
        {
            while(RespondingToCreateRequestsIsSuspended) Thread.Sleep(10);
            LastCreatedPost = content;
            NumberOfCreateRequestsRecieved++;

            CurrentPosts.Add(new GetAllPostsResponse.Post
            {
                Content = content,
                DateMade = DateTime.Now,
                Id = "SOMEID"
            });
            return NextCreatePostResponse;
        }
    }
}
