# 📧 Gmail Search Application

<div align="center">

![Gmail Logo](https://img.shields.io/badge/Gmail-D14836?style=for-the-badge&logo=gmail&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=aspnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

**A modern web application for searching and viewing Gmail messages with advanced filtering capabilities**
</div>






## 🚀 Features

### ✨ Core Functionality
- **🔍 Advanced Email Search** - Search emails by keywords, date ranges, or both
- **📅 Date Range Filtering** - Filter emails by specific date periods
- **📱 Responsive Design** - Works seamlessly on desktop, tablet, and mobile devices
- **🔐 Secure Authentication** - OAuth 2.0 integration with Google accounts
- **📄 Pagination** - Efficient browsing through large email collections
- **👁️ Email Preview** - View full email content in a modal window

### 🛡️ Security & Performance
-  OAuth 2.0 Authentication - Secure Google account integration
-  Optimized Queries - Efficient Gmail API usage with proper error handling
-  Input Validation - Client and server-side validation for all inputs
-  Error Handling - Graceful error handling with user-friendly messages

### 🎨 User Experience
-  Intuitive Interface - Clean, modern UI with Material Design elements
-  Real-time Feedback - Loading indicators and progress feedback
-  Mobile-First - Responsive design that works on all devices
-  Modern Styling - Beautiful glass-morphism design with smooth animations


## <img width="1919" height="912" alt="image" src="https://github.com/user-attachments/assets/4bc2df4c-cae4-4d38-b94d-f79ab2fab94b" />

## <img width="1919" height="913" alt="image" src="https://github.com/user-attachments/assets/d305b038-1276-4815-a429-3b6fe845b027" />

## <img width="1919" height="911" alt="image" src="https://github.com/user-attachments/assets/7be453a4-4990-462c-8ac4-5d0a345275dd" />


## 📋 Table of Contents

- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Usage](#-usage)
- [Architecture](#-architecture)
- [Testing](#-testing)
- [API Reference](#-api-reference)

---

## 🔧 Prerequisites

Before running this application, ensure you have the following installed:

- **.NET 8.0 SDK** or later
- **Visual Studio 2022** or **Visual Studio Code**
- **Google Cloud Console** account with Gmail API enabled
- **Git** for version control

### Required Google API Setup

1. **Create a Google Cloud Project**
   - Go to [Google Cloud Console](https://console.cloud.google.com/)
   - Create a new project or select an existing one

2. **Enable Gmail API**
   - Navigate to "APIs & Services" > "Library"
   - Search for "Gmail API" and enable it

3. **Create OAuth 2.0 Credentials**
   - Go to "APIs & Services" > "Credentials"
   - Click "Create Credentials" > "OAuth 2.0 Client IDs"
   - Configure the OAuth consent screen
   - Add authorized redirect URIs (e.g., `https://localhost:7001/Auth/Callback`)

---

## 🚀 Installation

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/gmail-search-app.git
cd gmail-search-app
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Application Settings

Create or update `appsettings.Development.json` in the `WebApplication1` directory:

```json
{
  "GoogleAuth": {
    "ClientId": "your-google-client-id",
    "ClientSecret": "your-google-client-secret",
    "RedirectUri": "https://localhost:7001/Auth/Callback"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 4. Run the Application

```bash
cd WebApplication1
dotnet run
```

The application will be available at `https://localhost:7001`

---

## ⚙️ Configuration

### Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `GoogleAuth:ClientId` | Google OAuth 2.0 Client ID | ✅ |
| `GoogleAuth:ClientSecret` | Google OAuth 2.0 Client Secret | ✅ |
| `GoogleAuth:RedirectUri` | OAuth redirect URI | ✅ |

### Application Settings

The application uses the following configuration sections:

- **GoogleAuth**: OAuth 2.0 configuration for Google authentication
- **Logging**: Application logging configuration
- **Session**: Session management settings

---

## 📖 Usage

### 1. Authentication

1. **Access the Application**
   - Navigate to the application URL
   - Click "Увійти через Google" (Login with Google)

2. **Google OAuth Flow**
   - You'll be redirected to Google's consent screen
   - Grant permission to access your Gmail account
   - You'll be redirected back to the application

### 2. Email Search

1. **Basic Search**
   - Enter keywords in the search field
   - Click "Search" to find matching emails

2. **Date Range Search**
   - Use the "After" and "Before" date fields
   - Combine with keywords for precise filtering
   - Click "Search" to apply filters

3. **Advanced Search**
   - Combine keywords with date ranges
   - Use the "Clear" button to reset search criteria

### 3. Viewing Emails

1. **Email List**
   - View search results in a responsive table
   - Navigate through pages using pagination

2. **Email Preview**
   - Click "View" button to open email content
   - Email content opens in a modal window
   - Supports HTML content rendering

---

## 🏗️ Architecture

### Project Structure

```
GmailTask/
├── WebApplication1/                 # Main ASP.NET Core Web Application
│   ├── Controllers/                 # MVC Controllers
│   │   ├── AuthController.cs       # OAuth authentication
│   │   ├── GmailController.cs      # Gmail operations
│   │   └── HomeController.cs       # Home page
│   ├── Services/                   # Business Logic Services
│   │   ├── GmailServiceHelper.cs   # Gmail API wrapper
│   │   └── EmailParser.cs          # Email content parsing
│   ├── Interfaces/                 # Service interfaces
│   ├── Models/                     # View Models
│   ├── Views/                      # Razor Views
│   ├── Wrappers/                   # API wrappers
│   └── wwwroot/                    # Static files
├── GmailSearch.Tests/              # Unit Tests
│   ├── AuthTests.cs               # Authentication tests
│   ├── GmailSearchTests.cs        # Gmail service tests
│   └── ParserTests.cs             # Email parser tests
└── README.md                       # This file
```

### Technology Stack

- **Backend**: ASP.NET Core 8.0, C#
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **Authentication**: Google OAuth 2.0
- **API**: Google Gmail API v1
- **Testing**: xUnit, Moq, FluentAssertions
- **UI Framework**: Bootstrap 5 with Material Design

### Design Patterns

- **Dependency Injection**: Services are registered and injected
- **Repository Pattern**: Gmail API wrapper abstracts data access
- **MVC Pattern**: Clear separation of concerns
- **Service Layer**: Business logic separated from controllers

---

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test GmailSearch.Tests/
```

### Test Coverage

The application includes comprehensive unit tests for:

- **Authentication Flow** (`AuthTests.cs`)
- **Gmail Service Operations** (`GmailSearchTests.cs`)
- **Email Parsing** (`ParserTests.cs`)

### Test Structure

```csharp
[Fact]
public async Task SearchMessages_Success_ReturnsMessages()
{
    // Arrange - Setup test data and mocks
    // Act - Execute the method under test
    // Assert - Verify the expected behavior
}
```

---

## 🔌 API Reference

### Gmail Controller Endpoints

| Endpoint | Method | Description | Parameters |
|----------|--------|-------------|------------|
| `/Gmail/Index` | GET | Search and display emails | `keyword`, `before`, `after`, `page` |
| `/Gmail/ViewMessage` | GET | Get email content | `id` |

### Authentication Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/Auth/Login` | GET | Initiate OAuth flow |
| `/Auth/Callback` | GET | Handle OAuth callback |
| `/Auth/Logout` | POST | Logout user |

### Request Examples

```bash
# Search emails with keyword
GET /Gmail/Index?keyword=meeting&page=1

# Search emails by date range
GET /Gmail/Index?after=2024-01-01&before=2024-12-31

# View specific email
GET /Gmail/ViewMessage?id=message_id_here
```

---


## 🙏 Acknowledgments

- **Google Gmail API** for providing the email access functionality
- **ASP.NET Core** team for the excellent web framework
- **Bootstrap** for the responsive UI components
- **Material Design** for design inspiration

---

## 📞 Support

If you encounter any issues or have questions:

- **Create an Issue** on GitHub
- **Check the Documentation** in this README
- **Review the Test Cases** for usage examples

---

<div align="center">

**Made with ❤️ using ASP.NET Core and Google Gmail API**

[![GitHub stars](https://img.shields.io/github/stars/yourusername/gmail-search-app?style=social)](https://github.com/yourusername/gmail-search-app)
[![GitHub forks](https://img.shields.io/github/forks/yourusername/gmail-search-app?style=social)](https://github.com/yourusername/gmail-search-app)

</div>
