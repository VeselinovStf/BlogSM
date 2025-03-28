

using BlogSM.API.Domain;
using BlogSM.API.Persistence.Query;
using BlogSM.API.Persistence.Repositories.Abstraction;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Persistence.Repositories;

public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }

    public async Task<BlogPost?> GetPostWithRelationIdsAsync(Guid id)
    {
        return await _dbSet
            .Where(bp => bp.Id == id)
            .Select(bp => new BlogPost
            {
                Id = bp.Id,
                IsPublished = bp.IsPublished,
                URLTitle = bp.URLTitle,
                Title = bp.Title,
                Date = bp.Date,
                Preview = bp.Preview,
                Image = bp.Image,
                Alt = bp.Alt,
                Short = bp.Short,
                ViewTitle = bp.ViewTitle,
                Content = bp.Content,
                TopBanner = bp.TopBanner,
                Discount = bp.Discount,
                PostTargetId = bp.PostTargetId,
                PageTypeId = bp.PageTypeId,
                AuthorId = bp.AuthorId,
                LayoutId = bp.LayoutId,
                Categories = bp.Categories.Select(c => new Category(){ Id = c.Id }).ToList(),
                Tags = bp.Tags.Select(t => new Tag() { Id = t.Id }).ToList(),
                LinkedPacks = bp.LinkedPacks.Select(lp => new Pack() { Id = lp.Id }).ToList(),
                DemoPacks = bp.DemoPacks.Select(dp => new Pack() { Id = dp.Id }).ToList(),
            })
            .FirstOrDefaultAsync();
    }

    public async Task<BlogPost?> GetPostWithIncludesAsync(Guid id)
    {
        return await _dbSet
           .Where(bp => bp.Id == id)
           .Include(bp => bp.Categories)
           .Include(bp => bp.Tags)
           .Include(bp => bp.LinkedPacks)
           .Include(bp => bp.DemoPacks)
           .Include(bp => bp.Author)
           .Include(bp => bp.PageType)
           .Include(bp => bp.PostTarget)
           .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync(QueryPipeline<BlogPost> queryPipeline)
    {
        var blogPostsQuery = _dbSet
            .Include(bp => bp.Categories)
            .Include(bp => bp.Tags)
            .Include(bp => bp.LinkedPacks)
            .Include(bp => bp.DemoPacks)
            .Include(bp => bp.Author)
            .Include(bp => bp.PageType)
            .Include(bp => bp.PostTarget)
            .AsNoTracking()
            .AsQueryable();

        return await queryPipeline.Apply(blogPostsQuery).ToListAsync();
    }
}

