using System.Linq;

namespace Barker.Posting.Operations.GetAllPosts
{
    public class GetAllPostsOperationImpl : GetAllPostsOperation
    {
        private PostRepository PostRepository;

        public GetAllPostsOperationImpl(PostRepository mockPostRepository)
        {
            PostRepository = mockPostRepository;
        }

        public GetAllPostsResponse Execute()
        {
            var posts = PostRepository.GetAllPosts()
                        .OrderByDescending(post => post.DateMade)
                        .Select(GetResponsePostFromRepositoryPost)
                        .ToList();

            return new GetAllPostsResponse
            {
                Posts = posts
            };
        }

        private GetAllPostsResponse.Post GetResponsePostFromRepositoryPost(Post inputPost)
        {
            return new GetAllPostsResponse.Post
            {
                Content = inputPost.Content,
                Id = inputPost.Id,
                DateMade = inputPost.DateMade
            };
        }
    }
}