# BlogSM

BlogSM is a microservices-based application designed for managing a website-store and blog. It is built to facilitate CRUD operations on blog posts and integrates with a static site builder (Jekyll) for dynamic content generation. The application is evolving, and while it follows a microservices architecture, the current implementation is based on a monolithic API for managing blog posts.

## Project Overview

BlogSM is a multi-functional system that combines the management of a blog and an online store. The system supports the following features:

- **Managing blog posts**: Add, edit, delete, and retrieve blog posts.
- **File handling**: Upload and organize blog post images to a static site builder folder.
- **Category and Tag management**: Each blog post is linked to categories and tags for better content organization.
- **Microservices-based architecture** (future): The application is evolving toward a microservices-based architecture. Currently, the API is monolithic but is being refactored into individual services.

### Current Structure:
- **API Layer**: `BlogSM.API` – A monolithic API that handles CRUD operations for blog posts, categories, and tags. It also facilitates file uploads to a static site builder folder for Jekyll-based websites.
- **Test Layer**: `BlogSM.API.Tests` – Unit tests to ensure the functionality of the API layer.
- **Future Evolution**: The project is being refactored to follow a microservices architecture for better scalability and maintainability.

---

## Features

- **Blog Post CRUD**: 
  - Create, Read, Update, and Delete blog posts with associated categories and tags.
  - The blog posts can include text, images, and other media.
  
- **Static Site Integration**:
  - Automatically uploads blog posts to a Jekyll-based static site.
  - Each blog post is transformed into markdown files that can be used directly in the static site.

- **API Integration**:
  - The API provides endpoints for managing blog posts and retrieving them from the static site builder's folder.
  
---

## Setup and Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/BlogSM.git
   cd BlogSM

2. Install dependencies: Navigate to BlogSM.API and run:
    ```bash
    dotnet restore
    dotnet build

3. Setup DB:
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=!Aa12345678" -p 1433:1433 --name sql1 --hostname sql1  -d mcr.microsoft.com/mssql/server:2022-latest

4. Run the application:
   ```bash
   dotnet run --project BlogSM.API

## Folder Structure
   - src/BlogSM.API: The core API for managing blog posts. This includes all the CRUD operations and uploading functionality.
   - test/BlogSM.API.Tests: The unit tests for the BlogSM API. It includes tests for various features such as CRUD operations and static site integration.
   - static-site-builder-folder: A folder that mimics the Jekyll static site folder. Blog posts are written as markdown files in this directory.

## Current Architecture

### Monolithic Architecture

Currently, BlogSM is based on a monolithic API. This means that all features (blog post management, category/tag handling, file upload) are handled within a single API layer. While this approach is simpler, it limits the scalability and maintainability of the application.

### Future Evolution to Microservices

The project is evolving into a microservices architecture, which will include separating different parts of the application into distinct services such as:

- Blog Post Service
- Category/Tag Service
- User/Authentication Service
- File Upload/Static Site Service

This will improve scalability, fault isolation, and maintainability, and allow each service to be independently scaled.

## API Endpoints

### 1. Create a Blog Post  
**POST /api/v1/blogposts**  
Request body:
```json
{
  "URLTitle": "mastering-art-of-cinematic-fx",
  "LayoutId": "993255D0-2B74-4667-AF4C-67CD13F6E572",
  "Title": "Mastering the Art of Cinematic FX",
  "Date": "2025-02-04T06:00:00+03:00",
  "Preview": "blog_post_2025_02_4_preview.webp",
  "Image": "blog_post_2025_02_4.webp",
  "Alt": "Mastering the Art of Cinematic FX",
  "CategoryIds": [
    "037E0B45-2F39-4141-9235-0D891813827E",
    "F6DF5AA6-6833-4624-8B92-48CA7171295C"
  ],
  "AuthorId": "219392DE-4414-4D0E-A93A-F5A1C171F2D4",
  "Short": "Mastering the Art of Cinematic FX - Impacts, Sweeps, Atmospheres & Soundscapes",
  "TagIds": [
    "C651E066-DADB-441E-BEED-930E7096DC37",
    "DF007FDB-69E7-4777-A41C-A0147F209465"
  ],
  "ViewTitle": true,
  "TopBanner": "cinematic_fx_banner.webp",
  "Discount": 20,
  "PostTargetId": "BEDE26BE-4E4C-490E-BE68-88E9803BCAEB",
  "PageTypeId": "81545FF1-CDEA-40AA-8E11-7E3DD771B0ED",
  "Content": "In the world of music production, film scoring, and game sound design, sound effects play a crucial role in shaping the listener’s experience..."
}
```

### 3. Get a Blog Post by ID

**GET /api/v1/blogposts/{id}**
Returns the details of a single blog post.

# Testing

## To run tests:
1. Navigate to the `test/BlogSM.API.Tests` folder.
2. Run the tests using:
    ```bash
    dotnet test
    ```

# Future Improvements

- **Refactor to Microservices**: We plan to refactor the monolithic API into separate microservices for better scalability and maintainability.
- **Improve File Uploads**: Optimize file uploads for large files and integrate cloud storage options.
- **Authentication and Authorization**: Add user authentication to restrict access to certain API endpoints.

# Contributing

If you'd like to contribute to the project, please fork the repository and create a pull request with your changes. Make sure to include tests for any new features or bug fixes.

# License

This project is licensed under the MIT License - see the LICENSE file for details.

# Notes:

- **The project is evolving**: As the project progresses, we will keep transitioning from a monolithic architecture to a more scalable and efficient microservices-based design. This transition is ongoing, so expect some features to be refactored.
