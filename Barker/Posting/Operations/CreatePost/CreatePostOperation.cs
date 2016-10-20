namespace Barker.Posting.Operations.CreatePost
{
    public interface CreatePostOperation
    {
        CreatePostResponse Execute(string content);
    }
}