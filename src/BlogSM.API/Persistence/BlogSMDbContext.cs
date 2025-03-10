using System;

using BlogSM.API.Domain;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Persistence;

public class BlogSMDbContext : DbContext
{
    public DbSet<BlogPost> BlogPosts {get;set; }
    public DbSet<Author> Authors { get;set;}
    public DbSet<Category> Categorys { get;set;}
    public DbSet<Layout> Layouts { get;set;}
    public DbSet<Pack> Packs { get;set;}
    public DbSet<PageType> PageTypes { get;set;}
    public DbSet<PostTarget> PostTargets { get;set;}
    public DbSet<Tag> Tags { get; set; }

    public BlogSMDbContext(DbContextOptions<BlogSMDbContext> options) : base(options)
    {
        
    }
}
