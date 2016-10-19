namespace Barker.Posting.Operations.CreatePost
{
    public class CreatePostResponse
    {
        public bool Successful { get; set; }
        public bool ErrorPostContentEmpty { get; set; }

        public bool ErrorPostContentTooLong { get; set; }
        public string PostId { get; set; }
    }
}