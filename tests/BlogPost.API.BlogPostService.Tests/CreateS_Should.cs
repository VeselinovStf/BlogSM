using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;

using Moq;

using ServiceLayer = BlogSM.API.Services;
using DomainLayer = BlogSM.API.Domain;
using Microsoft.Extensions.Logging;

namespace BlogPost.API.BlogPostService.Tests;

public class Create_Should
{
    private readonly Mock<IBlogPostRepository> _blogPostRepoMock;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly Mock<ITagRepository> _tagRepoMock;
    private readonly ServiceLayer.BlogPostService _blogPostService;

    private readonly Mock<ILogger<ServiceLayer.BlogPostService>> _loggerMock;

    public Create_Should()
    {
        // Initialize mocks
        _blogPostRepoMock = new Mock<IBlogPostRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _tagRepoMock = new Mock<ITagRepository>();
        _loggerMock = new Mock<ILogger<ServiceLayer.BlogPostService>>();

        // Initialize the service with mocked dependencies
        _blogPostService = new ServiceLayer.BlogPostService(_blogPostRepoMock.Object, _categoryRepoMock.Object, _tagRepoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnSuccess_WhenValidBlogPostParameterIsPassed()
    {
        // Arrange
        var blogPost = new DomainLayer.BlogPost
        {
            Id = Guid.NewGuid(),
            Categories = new List<Category> { new Category { Id = Guid.NewGuid() } },
            Tags = new List<Tag> { new Tag { Id = Guid.NewGuid() } }
        };

        // Setup mocks
        _categoryRepoMock.Setup(repo => repo.GetCategoriesByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Category> { new Category { Id = blogPost.Categories.First().Id } });

        _tagRepoMock.Setup(repo => repo.GetTagsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Tag> { new Tag { Id = blogPost.Tags.First().Id } }.AsQueryable());

        // Mock AddAsync and SaveAsync methods
        _blogPostRepoMock.Setup(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()))
            .Returns(Task.CompletedTask);  // Simulate adding a blog post

        _blogPostRepoMock.Setup(repo => repo.SaveAsync())
            .Returns(Task.CompletedTask);  // Simulate saving changes

        // Act
        var result = await _blogPostService.Create(blogPost);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Blog post created successfully.", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(blogPost, result.Data);
    }

    [Fact]
    public async Task Create_ShouldSaveNewBlogPost_WhenValidBlogPostParameterIsPassed()
    {
        // Arrange
        var blogPost = new DomainLayer.BlogPost
        {
            Id = Guid.NewGuid(),
            Categories = new List<Category> { new Category { Id = Guid.NewGuid() } },
            Tags = new List<Tag> { new Tag { Id = Guid.NewGuid() } }
        };

        // Setup mocks
        _categoryRepoMock.Setup(repo => repo.GetCategoriesByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Category> { new Category { Id = blogPost.Categories.First().Id } });

        _tagRepoMock.Setup(repo => repo.GetTagsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Tag> { new Tag { Id = blogPost.Tags.First().Id } }.AsQueryable());

        // Mock AddAsync and SaveAsync methods
        _blogPostRepoMock.Setup(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()))
            .Returns(Task.CompletedTask);  // Simulate adding a blog post

        _blogPostRepoMock.Setup(repo => repo.SaveAsync())
            .Returns(Task.CompletedTask);  // Simulate saving changes

        // Act
        var result = await _blogPostService.Create(blogPost);

        // Verify AddAsync was called exactly once
        _blogPostRepoMock.Verify(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()), Times.Once);

        // Verify SaveAsync was not called
        _blogPostRepoMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenCategoriesDoNotExist()
    {
        // Arrange
        var blogPost = new DomainLayer.BlogPost
        {
            Id = Guid.NewGuid(),
            Categories = new List<Category> { new Category { Id = Guid.NewGuid() } },
            Tags = new List<Tag> { new Tag { Id = Guid.NewGuid() } }
        };

        // Setup mocks for categories (simulate non-existing category)
        _categoryRepoMock.Setup(repo => repo.GetCategoriesByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Category>());

        _tagRepoMock.Setup(repo => repo.GetTagsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Tag> { new Tag { Id = blogPost.Tags.First().Id } }.AsQueryable());

        // Act
        var result = await _blogPostService.Create(blogPost);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Some categories do not exist.", result.Message);
        Assert.Null(result.Data);

        // Verify AddAsync was not called
        _blogPostRepoMock.Verify(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()), Times.Never);

        // Verify SaveAsync was not called
        _blogPostRepoMock.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenTagsDoNotExist()
    {
        // Arrange
        var blogPost = new DomainLayer.BlogPost
        {
            Id = Guid.NewGuid(),
            Categories = new List<Category> { new Category { Id = Guid.NewGuid() } },
            Tags = new List<Tag> { new Tag { Id = Guid.NewGuid() } }
        };

        // Setup mocks for tags (simulate non-existing tag)
        _tagRepoMock.Setup(repo => repo.GetTagsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Tag>());

        // Setup mocks
        _categoryRepoMock.Setup(repo => repo.GetCategoriesByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Category> { new Category { Id = blogPost.Categories.First().Id } });

        // Act
        var result = await _blogPostService.Create(blogPost);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Some tags do not exist.", result.Message);
        Assert.Null(result.Data);

        // Verify AddAsync was not called
        _blogPostRepoMock.Verify(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()), Times.Never);

        // Verify SaveAsync was not called
        _blogPostRepoMock.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenTagsAndCategoriesAreEmpty()
    {
        // Arrange
        var blogPost = new DomainLayer.BlogPost
        {
            Id = Guid.NewGuid(),
            Categories = new List<Category>(),
            Tags = new List<Tag>()
        };

        // Act
        var result = await _blogPostService.Create(blogPost);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Categories or tags cannot be empty.", result.Message);
        Assert.Null(result.Data);

        // Verify AddAsync was not called
        _blogPostRepoMock.Verify(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()), Times.Never);

        // Verify SaveAsync was not called
        _blogPostRepoMock.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenBlogPostIsNull()
    {
        // Arrange
        DomainLayer.BlogPost blogPost = null;

        // Act
        var result = await _blogPostService.Create(blogPost);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Blog post cannot be null.", result.Message);
        Assert.Null(result.Data);

        // Verify AddAsync was not called
        _blogPostRepoMock.Verify(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()), Times.Never);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenSaveFails()
    {
        // Arrange
        var blogPost = new DomainLayer.BlogPost
        {
            Id = Guid.NewGuid(),
            Categories = new List<Category> { new Category { Id = Guid.NewGuid() } },
            Tags = new List<Tag> { new Tag { Id = Guid.NewGuid() } }
        };

        // Setup mocks
        _categoryRepoMock.Setup(repo => repo.GetCategoriesByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Category> { new Category { Id = blogPost.Categories.First().Id } });

        _tagRepoMock.Setup(repo => repo.GetTagsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<Tag> { new Tag { Id = blogPost.Tags.First().Id } });

        _blogPostRepoMock.Setup(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()))
            .Returns(Task.CompletedTask);  // Simulate adding a blog post

        _blogPostRepoMock.Setup(repo => repo.SaveAsync())
            .ThrowsAsync(new Exception("Database error"));  // Simulate save failure

        // Act
        var result = await _blogPostService.Create(blogPost);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("An error occurred while saving the blog post.", result.Message);
        Assert.Null(result.Data);

        // Verify AddAsync was called exactly once
        _blogPostRepoMock.Verify(repo => repo.AddAsync(It.IsAny<DomainLayer.BlogPost>()), Times.Once);

        // Verify SaveAsync was called exactly once
        _blogPostRepoMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }

}
