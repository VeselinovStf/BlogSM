using System;

using AutoMapper;

using BlogSM.API.Domain;
using BlogSM.API.DTOs.BlogPost;

namespace BlogSM.API.Mappings;

public class BlogPostMappingProfile : Profile
{
    public BlogPostMappingProfile(){
        // DTO → Entity (Create)
        CreateMap<CreateBlogPostRequestDTO, BlogPost>()
            .ForMember(dest => dest.LayoutId, opt => opt.MapFrom(src => src.LayoutId))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.PostTargetId, opt => opt.MapFrom(src => src.PostTargetId))
            .ForMember(dest => dest.PageTypeId, opt => opt.MapFrom(src => src.PageTypeId))
            .ForMember(dest => dest.Categories, opt => opt.Ignore()) // Categories will be mapped manually
            .ForMember(dest => dest.Tags, opt => opt.Ignore())        // Tags will be mapped manually
            .ForMember(dest => dest.LinkedPacks, opt => opt.Ignore()) // LinkedPacks manually handled
            .ForMember(dest => dest.DemoPacks, opt => opt.Ignore());

        // Entity → DTO (Response)
        CreateMap<BlogPost, BlogPostResponseDTO>()
            .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.LayoutId.ToString()))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.AuthorId.ToString()))
            .ForMember(dest => dest.PostTarget, opt => opt.MapFrom(src =>  src.PostTargetId.ToString()))
            .ForMember(dest => dest.PageType, opt => opt.MapFrom(src => src.PageTypeId))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id.ToString()).ToList()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Id.ToString()).ToList()))
            .ForMember(dest => dest.LinkedPacks, opt => opt.MapFrom(src => src.LinkedPacks.Select(p => p.Id.ToString()).ToList()))
            .ForMember(dest => dest.DemoPacks, opt => opt.MapFrom(src => src.DemoPacks.Select(p => p.Id.ToString()).ToList()));
    }
}
