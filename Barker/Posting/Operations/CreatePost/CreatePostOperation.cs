namespace Barker.Posting.Operations.CreatePost
{
    public class CreatePostOperation
    {
        private PostRepository PostRepository;
        private CurrentTimeProvider CurrentTimeProvider;

        public CreatePostOperation(PostRepository postRepository, CurrentTimeProvider currentTimeProvider)
        {
            CurrentTimeProvider = currentTimeProvider;
            PostRepository = postRepository;
        }

        public CreatePostResponse Execute(string content)
        {
            if (string.IsNullOrEmpty(content?.Trim()))
            {
                return new CreatePostResponse
                {
                    ErrorPostContentEmpty = true
                };
            }

            if (content.Length > 150)
            {
                return new CreatePostResponse
                {
                    ErrorPostContentTooLong = true
                };
            }

            var newPostId = PostRepository.AddPost(new Post
            {
                Content = content,
                DateMade = CurrentTimeProvider.GetCurrentTime()
            });

            return new CreatePostResponse
            {
                Successful = true,
                PostId = newPostId
            };
        }
    }
}