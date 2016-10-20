using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    public class PostTests
    {
        private const string TheFirstPostsContent = "Post 1 content";
        private static ChromeDriver WebDriver;
        private IDisposable Api;

        [OneTimeSetUp]
        public static void SetUpOnceForAllTests()
        {
            WebDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService());
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
            var getAll = Substitute.For<GetAllPostsOperation>();
            getAll.Execute().Returns(new GetAllPostsResponse
            {
                Posts = new List<GetAllPostsResponse.Post>
                {
                    new GetAllPostsResponse.Post
                    {
                        Content = TheFirstPostsContent
                    }
                }
            });

            ApiControllerCatalog.AddNancyModule(new PostController(getAll));
            Api = WebApp.Start<ApiStartup>("http://localhost:8080/");
        }

        [TearDown]
        public void TearDown()
        {
            Api.Dispose();
        }

        [Test]
        public void CanReadTheLatestPosts()
        {
            WebDriver.Navigate().GoToUrl("http://localhost:9000/");
            PageShouldShowText(TheFirstPostsContent);
        }

        private static void PageShouldShowText(string text)
        {
            WaitUntil(() => WebDriver.FindElements(By.XPath($"//*[contains(text(), '{text}')]")).Count > 0);
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
    }
}
