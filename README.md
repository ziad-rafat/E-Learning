# E-Learning Platform

A comprehensive ASP.NET Core Web API for an E-Learning platform with course management, user authentication, enrollments, and review systems.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Features](#features)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)
- [Configuration](#configuration)
- [Development](#development)

---

## Project Overview

This E-Learning Platform is a multi-layered .NET application designed to provide a robust backend for managing online courses, user enrollments, instructor management, and course reviews. The application uses clean architecture principles with separation of concerns across multiple layers:

- **API Layer** (E-Learning.View) - ASP.NET Core Web API controllers
- **Application Layer** (E-Learning.Application) - Business logic and services
- **Data Layer** (E-Learning.Infrastructure & E-Learning.Context) - Database access and Entity Framework
- **Models Layer** (E-Learning.Models) - Entity models
- **DTOs Layer** (E-Learning.Dtos) - Data transfer objects

---

## Technology Stack

- **.NET Framework**: ASP.NET Core (latest)
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **ORM**: Entity Framework Core
- **Auto Mapping**: AutoMapper
- **Email**: SMTP (Gmail)
- **API Documentation**: Swagger/OpenAPI

---

## Project Structure

```
E-Learning/
├── E-Learning.Application/           # Business logic layer
│   ├── Contract/                      # Repository interfaces
│   ├── IService/                      # Service interfaces
│   ├── Services/                      # Service implementations
│   ├── Mapper/                        # AutoMapper profiles
│   └── E-Learning.Application.csproj
│
├── E-Learning.Context/               # Data context layer
│   ├── ELearningContext.cs            # DbContext
│   ├── Migrations/                    # Database migrations
│   └── E-Learning.Context.csproj
│
├── E-Learning.Infrastructure/        # Data access layer
│   ├── *Repository.cs                # Repository implementations
│   └── E-Learning.Infrastructure.csproj
│
├── E-Learning.Models/                # Domain models
│   ├── User.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   ├── Review.cs
│   ├── Topic.cs
│   ├── Category.cs
│   ├── SubCategory.cs
│   ├── Assignment.cs
│   ├── Order.cs
│   └── E-Learning.Models.csproj
│
├── E-Learning.Dtos/                  # Data Transfer Objects
│   ├── Course/
│   ├── Category/
│   ├── User/
│   ├── Lecture/
│   ├── Topic/
│   ├── Review/
│   ├── Email/
│   ├── ViewResult/
│   └── E-Learning.Dtos.csproj
│
├── E-Learning.View/                  # API layer
│   ├── Controllers/                  # API endpoints
│   ├── Program.cs                    # Startup configuration
│   ├── appsettings.json              # Configuration
│   ├── wwwroot/                      # Static files
│   └── E-Learning.View.csproj
│
├── E-Learning.sln                    # Solution file
└── README.md
```

---

## Features

### 👤 User Management
- User registration and authentication
- Email confirmation required
- User profiles with photos
- Instructor and learner roles
- Soft delete support

### 📚 Course Management
- Create, update, and delete courses
- Bilingual course content (English & Arabic)
- Course pricing
- Course images and promotional videos
- Course ratings and review system
- Search and filtering capabilities
- Top-rated and most-enrolled course lists

### 📖 Course Structure
- **Topics & SubCategories**: Organize courses hierarchically
- **Lectures**: Video-based learning content
- **Sections**: Group lectures within courses
- **Assignments**: Create course assignments

### ✅ Enrollment System
- Student enrollment in courses
- Track enrollment count
- Enrollment status management

### ⭐ Review & Rating System
- Students can review courses
- 5-star rating system
- Calculate average ratings
- Track rating counts

### 📧 Email Service
- Confirm email addresses
- Transactional emails
- SMTP configuration with Gmail

### 💾 File Management
- Upload course materials
- File storage service
- Support for images and videos

### 💳 Order Management
- Order placement for courses
- Order tracking
- OrderCourse relationships for multiple courses per order

---

## Getting Started

### Prerequisites
- .NET SDK (v6.0 or higher)
- SQL Server (LocalDB or Express)
- Visual Studio or Visual Studio Code
- Git

### Installation

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd E-Learning
   ```

2. **Update the database connection string** in [appsettings.json](appsettings.json):
   ```json
   "ConnectionStrings": {
       "Database": "Data Source=.;Initial Catalog=E-Learning;Integrated Security=True;Encrypt=True;encrypt=false"
   }
   ```

3. **Configure email settings** in [appsettings.json](appsettings.json):
   ```json
   "EmailSettings": {
       "SmtpServer": "smtp.gmail.com",
       "SmtpPort": 587,
       "SenderEmail": "your-email@gmail.com",
       "SenderName": "E-Learning Platform",
       "Username": "your-email@gmail.com",
       "Password": "your-app-password"
   }
   ```
   > For Gmail, use an [App Password](https://support.google.com/accounts/answer/185833) instead of your regular password.

4. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

5. **Apply database migrations**:
   ```bash
   dotnet ef database update --project E-Learning.Context --startup-project E-Learning.View
   ```

6. **Run the application**:
   ```bash
   dotnet run --project E-Learning.View
   ```

The API will be available at `https://localhost:5001` (or the configured port).

---

## API Endpoints

### Categories
- `GET /api/category` - Get all categories
- `GET /api/category/{id}` - Get category by ID
- `POST /api/category` - Create new category
- `PUT /api/category/{id}` - Update category
- `DELETE /api/category/{id}` - Delete category

### Courses
- `GET /api/course` - Get all courses
- `GET /api/course/{id}` - Get course details
- `POST /api/course` - Create new course
- `PUT /api/course/{id}` - Update course
- `DELETE /api/course/{id}` - Soft delete course
- `GET /api/course/top-rated` - Get top-rated courses
- `GET /api/course/most-enrolled` - Get most-enrolled courses
- `GET /api/course/instructor/{instructorId}` - Get instructor's courses

### Courses search & filtering
- `POST /api/course/search` - Search courses with filters

### Topics
- `GET /api/topic` - Get all topics
- `GET /api/topic/{id}` - Get topic by ID
- `POST /api/topic` - Create new topic
- `PUT /api/topic/{id}` - Update topic
- `DELETE /api/topic/{id}` - Delete topic

### SubCategories
- `GET /api/subcategory` - Get all sub-categories
- `GET /api/subcategory/{id}` - Get sub-category by ID
- `POST /api/subcategory` - Create new sub-category
- `PUT /api/subcategory/{id}` - Update sub-category
- `DELETE /api/subcategory/{id}` - Delete sub-category

### Users
- `GET /api/user` - Get all users
- `GET /api/user/{id}` - Get user by ID
- `POST /api/user/register` - Register new user
- `POST /api/user/login` - User login
- `PUT /api/user/{id}` - Update user profile

### Reviews
- `GET /api/review` - Get all reviews
- `GET /api/review/{id}` - Get review by ID
- `POST /api/review` - Create new review
- `PUT /api/review/{id}` - Update review
- `DELETE /api/review/{id}` - Delete review

---

## Database Schema

### Core Entities

**User**
- Extends ASP.NET Identity User
- Properties: FirstName, LastName, Photo, IsDeleted
- Relationships: Enrollments, Reviews, Orders, Courses (as instructor)

**Course**
- Properties: Title, Ar_Title, Description, Ar_Description, Price, CourseImage, PromotionalVideo
- Ratings: RatingAverage, RatingCount, EnrollmentsCount
- Foreign Keys: UserId (Instructor), TopicId
- Relationships: Enrollments, Reviews, OrderCourses, Topics

**Topic**
- Properties: Name, Ar_Name
- Relationships: SubCategories, Courses

**SubCategory**
- Properties: Name, Ar_Name
- Foreign Key: TopicId
- Relationships: Courses

**Category**
- Properties: Name, Ar_Name
- Relationships: SubCategories

**Enrollment**
- Foreign Keys: UserId, CourseId
- Tracks student enrollment in courses

**Review**
- Properties: Rating (1-5), Comment
- Foreign Keys: UserId, CourseId
- Timestamps: CreatedAt, UpdatedAt

**Order**
- Properties: TotalPrice, OrderDate
- Foreign Key: UserId
- Relationships: OrderCourses

**OrderCourse**
- Junction table for Order-Course many-to-many relationship
- Foreign Keys: OrderId, CourseId

**Lecture, Section, Assignment**
- Support course content organization

---

## Configuration

### Database
Update the connection string in `appsettings.json` based on your SQL Server setup:

```json
{
  "ConnectionStrings": {
    "Database": "Data Source=.;Initial Catalog=E-Learning;Integrated Security=True;Encrypt=True;encrypt=false"
  }
}
```

### Email Service
Configure SMTP settings for email notifications:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "E-Learning Platform",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### Logging
Configure logging levels in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## Development

### Dependency Injection Container
Services and repositories are registered in [Program.cs](E-Learning.View/Program.cs):

- **ICourseService** → CourseService
- **ITopicService** → TopicService
- **IReviewService** → ReviewService
- **ICategoryService** → CategoryService
- **ISubCategoryService** → SubCategoryService
- **IUserService** → UserService
- **IEmailService** → EmailService
- **IFileService** → FileService

### AutoMapper
Mapping profiles are configured in [AutoMapperProfile.cs](E-Learning.Application/Mapper/AutoMapperProfile.cs) for automatic DTO-to-Model conversions.

### Entity Framework Migrations
To add new migrations:
```bash
dotnet ef migrations add MigrationName --project E-Learning.Context --startup-project E-Learning.View
dotnet ef database update --project E-Learning.Context --startup-project E-Learning.View
```

### API Documentation
Swagger/OpenAPI documentation is available at:
```
https://localhost:5001/swagger
```

---

## Future Enhancements

- [ ] Payment gateway integration
- [ ] Advanced analytics and reporting
- [ ] Video streaming optimization
- [ ] Course recommendations engine
- [ ] Real-time notifications
- [ ] Mobile app integration
- [ ] Localization improvements
- [ ] Performance optimization

---

## License

This project is part of the ITI E-Learning initiative.

---

## Support

For questions or issues, please contact the development team or create an issue in the repository.