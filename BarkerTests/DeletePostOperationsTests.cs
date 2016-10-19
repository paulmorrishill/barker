using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barker;
using Barker.Posting;
using Barker.Posting.Operations;
using Barker.Posting.Operations.DeletePost;
using NSubstitute;
using NUnit.Framework;

namespace BarkerTests
{
    class DeletePostOperationsTests
    {
        private PostRepository PostRepository;
        private DeletePostOperation Delete;

        [Test]
        public void CanDeleteAPost()
        {
            PostRepository = Substitute.For<PostRepository>();
            Delete = new DeletePostOperation(PostRepository);
            Delete.Execute("some-id");

            PostRepository.Received().DeletePost("some-id");
        }
    }
}
