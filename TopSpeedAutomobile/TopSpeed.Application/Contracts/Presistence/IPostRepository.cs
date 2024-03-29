﻿using System;
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

        Task<Post> GetPostById(int id);

        Task<List<Post>> GetAllPost();

        Task<List<Post>> GetAllPost(int? skipRecord, int? brandId);

        Task<List<Post>> GetAllPost(string? searchName,int? brandId,int? vehicleTypeId);


    }
}
