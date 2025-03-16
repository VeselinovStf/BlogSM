using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

namespace BlogSM.API.Persistence.Repositories;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    public AuthorRepository(BlogSMDbContext blogSMDbContext) : base(blogSMDbContext)
    {
    }
}
