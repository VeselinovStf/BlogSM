using BlogSM.API.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSM.API.Persistence.Configuration;

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
       public void Configure(EntityTypeBuilder<BlogPost> builder)
       {
              builder.HasKey(b => b.Id);

              // Define relationships
              builder.HasOne(b => b.Layout)
                     .WithMany()
                     .HasForeignKey(b => b.LayoutId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(b => b.Author)
                     .WithMany(a => a.BlogPosts)
                     .HasForeignKey(b => b.AuthorId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(b => b.PostTarget)
                     .WithMany()
                     .HasForeignKey(b => b.PostTargetId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(b => b.PageType)
                     .WithMany()
                     .HasForeignKey(b => b.PageTypeId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder
                     .HasMany(b => b.Categories)
                     .WithMany(c => c.BlogPosts)
                     .UsingEntity<BlogPostCategory>(
                            j => j.HasOne(bp => bp.Category).WithMany().HasForeignKey(bp => bp.CategoryId),
                            j => j.HasOne(bp => bp.BlogPost).WithMany().HasForeignKey(bp => bp.BlogPostId),
                            j =>
                            {
                                   j.HasKey(t => new { t.BlogPostId, t.CategoryId });
                                   j.ToTable("BlogPostCategories");
                            });

              builder
                     .HasMany(b => b.Tags)
                     .WithMany(t => t.BlogPosts)
                     .UsingEntity<BlogPostTag>(
                            j => j.HasOne(bp => bp.Tag).WithMany().HasForeignKey(bp => bp.TagId),
                            j => j.HasOne(bp => bp.BlogPost).WithMany().HasForeignKey(bp => bp.BlogPostId),
                            j =>
                            {
                                   j.HasKey(t => new { t.BlogPostId, t.TagId });
                                   j.ToTable("BlogPostTags");
                            });

              builder
                     .HasMany(b => b.LinkedPacks)
                     .WithMany(p => p.LinkedBlogPosts)
                     .UsingEntity<BlogPostLinkedPack>(
                            j => j.HasOne(bp => bp.LinkedPack).WithMany().HasForeignKey(bp => bp.LinkedPackId),
                            j => j.HasOne(bp => bp.BlogPost).WithMany().HasForeignKey(bp => bp.BlogPostId),
                            j =>
                            {
                                   j.HasKey(t => new { t.BlogPostId, t.LinkedPackId });
                                   j.ToTable("BlogPostLinkedPacks");
                            });

              builder
                     .HasMany(b => b.DemoPacks)
                     .WithMany(p => p.DemoBlogPosts)
                     .UsingEntity<BlogPostDemoPack>(
                            j => j.HasOne(bp => bp.DemoPack).WithMany().HasForeignKey(bp => bp.DemoPackId),
                            j => j.HasOne(bp => bp.BlogPost).WithMany().HasForeignKey(bp => bp.BlogPostId),
                            j =>
                            {
                                   j.HasKey(t => new { t.BlogPostId, t.DemoPackId });
                                   j.ToTable("BlogPostDemoPacks");
                            });
       }
}
