using Blog;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace BlogClient
{
    class Program
    {
        const string Host = "localhost";
        const int Port = 50088;

        static async Task Main(string[] args)
        {
            var channel = new Channel(Host, Port, ChannelCredentials.Insecure);

            await channel.ConnectAsync().ContinueWith(task =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Client connected.");
                }
            });

            var client = new BlogService.BlogServiceClient(channel);

            // create blog item
            // var createdBlog = await CreateBlogItem(client);

            // read blog item
            // var readBlog = await ReadBlogItem(client, "60532a036a3de246f859acd7");

            // update blog item
            // var updatedBlog = await UpdateBlogItem(client, readBlog.Id);

            // delete blog item
            // await DeleteBlogItem(client, "60532a036a3de246f859acd7");

            // read deleted blog item
            // var readBlog2 = await ReadBlogItem(client, "60532a036a3de246f859acd7");

            // read all
            // await ReadAll(client);

            await channel.ShutdownAsync();
            Console.WriteLine("Client shut down.");
            Console.ReadLine();
        }

        private static async Task ReadAll(BlogService.BlogServiceClient client)
        {
            var response = client.ListBlog(new ListBlogRequest());
            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine($"Item read: {response.ResponseStream.Current.Blog}");
            }
            Console.WriteLine("All items are listed");
        }

        private static async Task DeleteBlogItem(BlogService.BlogServiceClient client, string id)
        {
            try
            {
                var resp = await client.DeleteBlogAsync(
                    new DeleteBlogRequest
                    {
                        BlogId = id
                    });
                Console.WriteLine($"Blog deleted: {resp.BlogId}");
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"Deletion failed: {ex.Status.Detail}");
            }
        }

        private static async Task<Blog.Blog> UpdateBlogItem(BlogService.BlogServiceClient client, string id)
        {
            try
            {
                var resp = await client.UpdateBlogAsync(
                    new UpdateBlogRequest
                    {
                        Blog = new Blog.Blog
                        {
                            Id = id,
                            AuthorId = "Lev Tolstoi",
                            Title = "Modified title",
                            Content = "Modified content"
                        }
                    });
                Console.WriteLine($"Blog updated: {resp.Blog}");
                return resp.Blog;
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"Update failed: {ex.Status.Detail}");
            }
            return null;
        }

        private static async Task<Blog.Blog> ReadBlogItem(BlogService.BlogServiceClient client, string id)
        {
            try
            {
                var blogReadResponse = await client.ReadBlogAsync(new ReadBlogRequest { BlogId = id });
                Console.WriteLine($"Blog read: {blogReadResponse.Blog}");
                return blogReadResponse.Blog;
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"Read failed: {ex.Status.Detail}");
            }
            return null;
        }

        private static async Task<Blog.Blog> CreateBlogItem(BlogService.BlogServiceClient client)
        {
            var blogCreatedResp = await client.CreateBlogAsync(new CreateBlogRequest
            {
                Blog = new Blog.Blog
                {
                    AuthorId = "Fourth to test updates",
                    Title = "Initial title",
                    Content = "Initial content"
                }
            });
            Console.WriteLine($"Blog created: {blogCreatedResp.Blog}");
            return blogCreatedResp.Blog;
        }
    }
}
