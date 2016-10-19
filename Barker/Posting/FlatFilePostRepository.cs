using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Barker.Posting
{
    public class FlatFilePostRepository : PostRepository
    {
        private List<Post> Posts = new List<Post>();
        private string FileName;

        public FlatFilePostRepository(string fileName)
        {
            FileName = fileName;
            if (!File.Exists(fileName)) return;

            var existingPostsAsJson = File.ReadAllText(fileName);
            Posts = JsonConvert.DeserializeObject<List<Post>>(existingPostsAsJson);
        }

        public List<Post> GetAllPosts()
        {
            return Posts;
        }

        public string AddPost(Post post)
        {
            var id = Guid.NewGuid().ToString();
            post.Id = id;
            Posts.Add(post);
            PersistChanges();
            return id;
        }

        private void PersistChanges()
        {
            File.WriteAllText(FileName, JsonConvert.SerializeObject(Posts));
        }

        public void DeletePost(string postId)
        {
            Posts = Posts
                    .Where(post => post.Id != postId)
                    .ToList();
            PersistChanges();
        }
    }
}
