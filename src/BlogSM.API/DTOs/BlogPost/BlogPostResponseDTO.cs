using System;

namespace BlogSM.API.DTOs.BlogPost;

public class BlogPostResponseDTO
{
    public Guid Id {get;set;}
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Preview { get; set; }
    public string Image { get; set; }
    public string Alt { get; set; }
    public string Short { get; set; }
    public bool ViewTitle { get; set; }
    public string Content { get; set; }
    public string? TopBanner { get; set; }
    public int Discount { get; set; }
    public string PostTarget { get; set; } // GUID as string
    public string PageType { get; set; } // GUID as string
    public string Author { get; set; } // GUID as string
    public string Layout { get; set; } // GUID as string
    public List<string> Categories { get; set; } // List of Category GUIDs
    public List<string> Tags { get; set; } // List of Tag GUIDs
    public List<string> LinkedPacks { get; set; } // List of Pack GUIDs
    public List<string> DemoPacks { get; set; } // List of DemoPack GUIDs
}
