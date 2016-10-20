namespace Barker.Posting.Operations.DeletePost
{
    public class DeletePostOperationImpl : DeletePostOperation
    {
        private PostRepository PostRepository;

        public DeletePostOperationImpl(PostRepository postRepository)
        {
            PostRepository = postRepository;
        }

        public void Execute(string postId)
        {
            PostRepository.DeletePost(postId);
        }
    }
}