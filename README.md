# 🎮 gameDB - Game Review CMS

A full-featured Content Management System built with **ASP.NET Core** that allows users to browse, review, and rate video games. Admins can manage game entries, developers, genres, and platforms, while users can create accounts and submit reviews.

- **Game**: Title, Description, ReleaseDate, Developer, CoverImage
- **Developer**: Name, Country, FoundedDate
- **Genre**: Name, Description
- **Platform**: Name, Manufacturer
- **User**: Managed via ASP.NET Core Identity
- **Review**: GameId, UserId, Content, Rating, CreatedAt
- **GameGenre**: Join table for many-to-many relationship between Games and Genres
