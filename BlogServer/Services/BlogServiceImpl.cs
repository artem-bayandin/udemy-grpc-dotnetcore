using Blog;
using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Blog.BlogService;

namespace BlogServer.Services
{
    public class BlogServiceImpl : BlogServiceBase
    {
        private static MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase mongoDatabase = mongoClient.GetDatabase("udemy-grpc-dotnetcore-blog");
        private static IMongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>("blog");

        public override async Task<CreateBlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
        {
            var blog = request.Blog;
            BsonDocument doc = new BsonDocument("author_id", blog.AuthorId)
                                               .Add("title", blog.Title)
                                               .Add("content", blog.Content);
            await mongoCollection.InsertOneAsync(doc);
            string id = doc.GetValue("_id").ToString();
            blog.Id = id;
            return new CreateBlogResponse { Blog = blog };
        }

        public override async Task<ReadBlogResponse> ReadBlog(ReadBlogRequest request, ServerCallContext context)
        {
            var blogId = request.BlogId;
            var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("_id", new ObjectId(blogId));
            var doc = (await mongoCollection.FindAsync(filter)).FirstOrDefault();
            if (doc == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Blog with id={blogId} was not found"));
            }
            return new ReadBlogResponse
            {
                Blog = new Blog.Blog
                {
                    Id = blogId,
                    AuthorId = doc.GetValue("author_id").AsString,
                    Title = doc.GetValue("title").AsString,
                    Content = doc.GetValue("content").AsString
                }
            };
        }

        public override async Task<UpdateBlogResponse> UpdateBlog(UpdateBlogRequest request, ServerCallContext context)
        {
            var blogId = request.Blog.Id;
            var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("_id", new ObjectId(blogId));
            var doc = (await mongoCollection.FindAsync(filter)).FirstOrDefault();
            if (doc == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Blog with id={blogId} was not found"));
            }
            var updatedDoc = new BsonDocument("author_id", request.Blog.AuthorId)
                                               .Add("title", request.Blog.Title)
                                               .Add("content", request.Blog.Content);
            try
            {
                //await mongoCollection.UpdateOneAsync(filter, updatedDoc);
                await mongoCollection.ReplaceOneAsync(filter, updatedDoc);
                return new UpdateBlogResponse
                {
                    Blog = new Blog.Blog
                    {
                        Id = blogId,
                        AuthorId = updatedDoc.GetValue("author_id").AsString,
                        Title = updatedDoc.GetValue("title").AsString,
                        Content = updatedDoc.GetValue("content").AsString
                    }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<DeleteBlogResponse> DeleteBlog(DeleteBlogRequest request, ServerCallContext context)
        {
            var blogId = request.BlogId;
            var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("_id", new ObjectId(blogId));
            var deleted = await mongoCollection.DeleteOneAsync(filter);
            if (deleted.DeletedCount == 0)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Blog with id={blogId} was not found"));
            }
            return new DeleteBlogResponse { BlogId = blogId };
        }

        public override async Task ListBlog(ListBlogRequest request, IServerStreamWriter<ListBlogResponse> responseStream, ServerCallContext context)
        {
            var filter = new FilterDefinitionBuilder<BsonDocument>().Empty;

            var docs = await (await mongoCollection.FindAsync(filter)).ToListAsync();

            foreach (var doc in docs)
            {
                await Task.Delay(250);
                await responseStream.WriteAsync(new ListBlogResponse
                {
                    Blog = new Blog.Blog
                    {
                        Id = doc.GetValue("_id").ToString(),
                        AuthorId = doc.GetValue("author_id").AsString,
                        Title = doc.GetValue("title").AsString,
                        Content = doc.GetValue("content").AsString
                    }
                });
            }
        }
    }
}
