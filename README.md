# Movie API
The Movie API is a RESTful web service that allows users to manage and retrieve information about movies and genres. This API is built using ASP.NET Core and Entity Framework Core, providing a robust and extensible platform for movie-related operations.


# Features
Movie Management: Create, read, update, and delete movies.
Genre Management: Create, read, update, and delete genres.
Filtering: Get movies by genre.
Swagger Documentation: Interactive API documentation using Swagger.
JWT Authentication: Secure endpoints using JWT Bearer authentication.
Database Integration: Store movie and genre data in an SQL Server database.
Data Validation: Implement data validation using DataAnnotations.
Cross-Origin Resource Sharing (CORS): Allow requests from any origin.
Getting Started
Prerequisites
Before running the project, make sure you have the following prerequisites installed:

.NET SDK (version X.X.X)
SQL Server
Installation
Clone the repository to your local machine:

shell
Copy code
git clone https://github.com/your-username/movie-database-api.git
Navigate to the project directory:

shell
Copy code
cd movie-database-api
Configure the connection string:

Open the appsettings.json file.
Update the DefaultConnection connection string to point to your SQL Server database.
Run the application:

shell
Copy code
dotnet run
The API will be accessible at https://localhost:5001 by default.

# Usage
You can use this API to perform various movie and genre-related operations. The API documentation, available via Swagger, provides detailed information on the available endpoints and how to use them.

# Endpoints
GET /api/movies: Get a list of all movies.
GET /api/movies/{id}: Get a movie by its ID.
POST /api/movies: Create a new movie.
PUT /api/movies/{id}: Update an existing movie.
DELETE /api/movies/{id}: Delete a movie by its ID.
GET /api/genres: Get a list of all genres.
GET /api/genres/{id}: Get a genre by its ID.
POST /api/genres: Create a new genre.
PUT /api/genres/{id}: Update an existing genre.
DELETE /api/genres/{id}: Delete a genre by its ID.
GET /api/movies/GetMoviesByGenre/{genreId}: Get movies by genre.
For detailed documentation, run the project and access the Swagger UI at https://localhost:5001/swagger.

# Authentication
This API uses JWT Bearer authentication. To access secured endpoints, you need to include a valid JWT token in the Authorization header of your requests. You can obtain a token by following the authentication process documented in Swagger.

Contributing
Contributions to this project are welcome! If you find any issues or have suggestions for improvement, please create a GitHub issue or fork the repository and submit a pull request.
