﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.DataAccess.Common;
using TopSpeed.Domain.ModelAggregate.Post;

namespace TopSpeed.DataAccess.Repository
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task Update(Post post)
        {
            var objFromDb = await _dbContext.Post.FirstOrDefaultAsync(x => x.Id == post.Id);

            if (objFromDb != null)
            {
                objFromDb.BrandId = post.BrandId;
                objFromDb.VehicleTypeId = post.VehicleTypeId;
                objFromDb.Name = post.Name;
                objFromDb.EngineAndFuelType = post.EngineAndFuelType;
                objFromDb.Transmission = post.Transmission;
                objFromDb.Engine = post.Engine;
                objFromDb.Range = post.Range;
                objFromDb.Ratings = post.Ratings;
                objFromDb.SeatingCapacity = post.SeatingCapacity;
                objFromDb.Mileage = post.Mileage;
                objFromDb.PriceFrom = post.PriceFrom;
                objFromDb.PriceTo = post.PriceTo;
                objFromDb.TopSpeed = post.TopSpeed;

                if (post.VehicleImage != null)
                {
                    objFromDb.VehicleImage = post.VehicleImage;
                }

                _dbContext.Update(objFromDb);
            }
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _dbContext.Post.Include(x => x.Brand).Include(x => x.VehicleType).FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<Post>> GetAllPost()
        {
            return await _dbContext.Post.Include(x => x.Brand).Include(x => x.VehicleType).OrderByDescending(x => x.ModifiedOn).ToListAsync();
        }


        public async Task<List<Post>> GetAllPost(int? skipRecord,int? brandId)
        {
            var query = _dbContext.Post.Include(x => x.Brand).Include(x => x.VehicleType).OrderByDescending(x => x.ModifiedOn);

            if (brandId == 0)
            {
                return await query.ToListAsync();
            }

            if (brandId > 0)
            {
                query = (IOrderedQueryable<Post>)query.Where(x => x.BrandId == brandId);
            }


            var posts = await query.ToListAsync();

            if (skipRecord.HasValue)
            {
                var recordToRemove = posts.FirstOrDefault(x => x.Id == skipRecord.Value);
                if (recordToRemove != null)
                {
                    posts.Remove(recordToRemove);
                }
            }


            return posts;
        }

        public async Task<List<Post>> GetAllPost(string? searchName, int? brandId, int? vehicleTypeId)
        {
            var query = _dbContext.Post.Include(x => x.Brand).Include(x => x.VehicleType).OrderByDescending(x => x.ModifiedOn);

            if(searchName == string.Empty && brandId ==0 && vehicleTypeId == 0)
            {
                return await query.ToListAsync();   
            }

            if (brandId > 0)
            {
                query = (IOrderedQueryable<Post>)query.Where(x => x.BrandId == brandId);
            }

            if (vehicleTypeId > 0)
            {
                query = (IOrderedQueryable<Post>)query.Where(x => x.VehicleTypeId == vehicleTypeId);
            }

            if(!string.IsNullOrEmpty(searchName))
            {                
                query = (IOrderedQueryable<Post>)query.Where(x => x.Name.Contains(searchName));
            }

            return await query.ToListAsync();
        }


    }
}
