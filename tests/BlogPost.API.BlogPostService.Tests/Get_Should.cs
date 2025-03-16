using System;

using BlogSM.API.Persistence.Repositories.Abstraction;

using Microsoft.Extensions.Logging;

using Moq;

using ServiceLayer = BlogSM.API.Services;
using DomainLayer = BlogSM.API.Domain;

namespace BlogPost.API.BlogPostService.Tests;

public class Get_Should
{
    private readonly Mock<IBlogPostRepository> _blogPostRepoMock;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly Mock<ITagRepository> _tagRepoMock;
    private readonly Mock<IAuthorRepository> _authorRepoMock;
    private readonly Mock<ILayoutRepository> _layoutRepoMock;
    private readonly Mock<IPackRepository> _packRepoMock;
    private readonly Mock<IPageTypeRepository> _pageTypeRepoMock;
    private readonly Mock<IPostTargetRepository> _postTargetRepoMock;
    private readonly ServiceLayer.BlogPostService _blogPostService;

    private readonly Mock<ILogger<ServiceLayer.BlogPostService>> _loggerMock;

    public Get_Should()
    {
        // Initialize mocks
        _blogPostRepoMock = new Mock<IBlogPostRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _tagRepoMock = new Mock<ITagRepository>();
        _authorRepoMock = new Mock<IAuthorRepository>();
        _layoutRepoMock = new Mock<ILayoutRepository>();
        _packRepoMock = new Mock<IPackRepository>();
        _pageTypeRepoMock = new Mock<IPageTypeRepository>();
        _postTargetRepoMock = new Mock<IPostTargetRepository>();

        _loggerMock = new Mock<ILogger<ServiceLayer.BlogPostService>>();

        // Initialize the service with mocked dependencies
        _blogPostService = new ServiceLayer.BlogPostService(
            _blogPostRepoMock.Object,
            _categoryRepoMock.Object,
             _tagRepoMock.Object,
             _authorRepoMock.Object,
             _layoutRepoMock.Object,
             _packRepoMock.Object,
             _pageTypeRepoMock.Object,
             _postTargetRepoMock.Object,
             _loggerMock.Object);
    }
    // 1. Happy Path Test (Found BlogPost)
    [Fact]
    public async Task ReturnBlogPost_WhenBlogPostExists()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();
        var blogPost = new DomainLayer.BlogPost
        {
            Id = blogPostId,
            Title = "Test Blog Post",
            Content = "This is a test blog post content",
            IsPublished = true
        };

        _blogPostRepoMock.Setup(repo => repo.GetPostWithCategoriesAndTagsAsync(blogPostId))
                         .ReturnsAsync(blogPost);

        // Act
        var result = await _blogPostService.Get(blogPostId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(blogPostId, result.Data.Id);
        Assert.Equal("Test Blog Post", result.Data.Title);
    }

    // 2. Not Found Test
    [Fact]
    public async Task ReturnNotFound_WhenBlogPostDoesNotExist()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();

        _blogPostRepoMock.Setup(repo => repo.GetPostWithCategoriesAndTagsAsync(blogPostId))
                         .ReturnsAsync((DomainLayer.BlogPost)null);

        // Act
        var result = await _blogPostService.Get(blogPostId);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal("Blog post not found", result.Message);
    }

    // 3. Exception Handling Test
    [Fact]
    public async Task HandleException_WhenExceptionOccurs()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();

        _blogPostRepoMock.Setup(repo => repo.GetPostWithCategoriesAndTagsAsync(blogPostId))
                         .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _blogPostService.Get(blogPostId);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal("An error occurred while retrieving the blog post", result.Message);
    }
}
