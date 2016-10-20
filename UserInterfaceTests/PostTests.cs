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
        private const string TheSecondPostContent = "This is another post";
        private const string TheFirstPostId = "post1";

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

            ApiControllerCatalog.Reset();
            ApiControllerCatalog.AddNancyModule(new PostController(this, this, this));
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
        public void CanCreateANewPostThrougTheUserInterface()
        {
            NavigateToTheHomePage();

            var theNewPostText = "This is a cool new post";

            WebDriver.FindElementById("post-content").SendKeys(theNewPostText);

            WebDriver.FindElementById("submit-post").Click();

            WaitUntil(() => LastCreatedPost == theNewPostText);

            PageShouldShowText(theNewPostText);
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
            LastCreatedPost = content;
            CurrentPosts.Add(new GetAllPostsResponse.Post
            {
                Content = content,
                DateMade = DateTime.Now,
                Id = "SOMEID"
            });
            return new CreatePostResponse
            {
                Successful = true
            };
        }
    }
}
