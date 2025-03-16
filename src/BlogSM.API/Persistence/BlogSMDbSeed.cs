using System;

using BlogSM.API.Domain;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Persistence;

public static class BlogSMDbSeed
{
    public static void SeedInitialData(DbContext context, bool arg2)
    {
        var category = context.Set<Category>().FirstOrDefault();
        if (category == null)
        {
            context.Set<Category>().AddRange([
                new Category { Name = "music" },
                new Category { Name = "fm" },
                new Category { Name = "synthesis" },
                new Category { Name = "tutorial" },
                new Category { Name = "packs" },
            ]);
            context.SaveChanges();
        }

        var tags = context.Set<Tag>().FirstOrDefault();
        if (tags == null)
        {
            context.Set<Tag>().AddRange([
                new Tag { Name = "beats" },
                new Tag { Name = "music" },
                new Tag { Name = "production" },
                new Tag { Name = "royalty-free music" },
                new Tag { Name = "join us" },
                new Tag { Name = "community" },
                new Tag { Name = "set" },
                new Tag { Name = "course" },
                new Tag { Name = "synthesis" },
                new Tag { Name = "fm" }
            ]);
            context.SaveChanges();
        }

        var author = context.Set<Author>().FirstOrDefault();
        if (author == null)
        {
            context.Set<Author>().Add(new Author { Name = "ToneSharp" });
            context.SaveChanges();
        }

        var postTarget = context.Set<PostTarget>().FirstOrDefault();
        if (postTarget == null)
        {
            context.Set<PostTarget>().Add(new PostTarget { Name = "offer" });
            context.SaveChanges();
        }

        var pageType = context.Set<PageType>().FirstOrDefault();
        if (pageType == null)
        {
            context.Set<PageType>().Add(new PageType { Name = "landing" });
            context.SaveChanges();
        }

        var layout = context.Set<Layout>().FirstOrDefault();
        if (layout == null)
        {
            context.Set<Layout>().Add(new Layout { Name = "post" });
            context.SaveChanges();
        }

        var packs = context.Set<Pack>().FirstOrDefault();
        if(packs == null)
        {
            context.Set<Pack>().Add(new Pack { Name = "Demo Pack 1" });
            context.Set<Pack>().Add(new Pack { Name = "Demo Pack 2" });
            context.SaveChanges();
        }
    }
}
