namespace Barker.Posting.Operations.DeletePost
{
    public class DeletePostOperation
    {
        private PostRepository PostRepository;

        public DeletePostOperation(PostRepository postRepository)
        {
            PostRepository = postRepository;
        }

        public void Execute(string postId)
        {
            PostRepository.DeletePost(postId);
        }
    }
}