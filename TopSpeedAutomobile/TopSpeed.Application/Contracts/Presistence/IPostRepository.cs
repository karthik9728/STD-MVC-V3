using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.ModelAggregate.Post;

namespace TopSpeed.Application.Contracts.Presistence
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task Update(Post post);

        Task<List<Post>> GetAllPost();

        Task<Post> GetPostById(int id);
    }
}
