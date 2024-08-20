# .NET, Docker, and PostgreSQL: A Practical Guide to Building Microservices

This project demonstrates how to build a .NET microservice with a PostgreSQL database, utilizing Docker for containerization and best practices for secure configuration management.



## Introduction

This project serves as a practical guide to developing .NET microservices that interact with a PostgreSQL database. We leverage Docker Compose to containerize both the application and the database, streamlining the development and deployment process. Key features include:

Minimal API: A lightweight and efficient approach to building APIs in .NET.
Custom Middleware: Demonstrates how to create and apply middleware for tasks like API key validation.
Entity Framework Core: Facilitates database interaction and migrations using a code-first approach.
Secure Configuration: Utilizes environment variables stored in a .env file to protect sensitive database credentials.
Repository Pattern: Implements the repository pattern for better code organization and maintainability.
Dependency Injection: Leverages built-in .NET dependency injection to manage dependencies effectively.
Input Validation and Error Handling: Includes robust input validation and error handling to enhance application reliability.

## Getting Started
### Prerequisites

Docker: Ensure you have Docker installed and running on your system.
.NET SDK: The .NET SDK is required to build and run the .NET application.

### Set up environment variables:

Create a .env file in the root of your project directory.

    API_KEY=your_api_key
    POSTGRES_DB=your_database_name
    POSTGRES_USER=your_database_user
    POSTGRES_PASSWORD=your_database_password


### Build and run the containers:

docker-compose up -d
Build the necessary Docker images (if they don't already exist).
Start the webapi and db containers in detached mode (running in the background).
Automatically apply any pending database migrations using Entity Framework Core.

    Note: If you encounter permission errors during the build process, you may need to run the command with elevated privileges (e.g., "Run as Administrator" on Windows or sudo docker-compose build on Linux).

### Configuration

Database Credentials: Update the POSTGRES_DB, POSTGRES_USER, and POSTGRES_PASSWORD variables in your .env file to match your PostgreSQL database configuration.
API Key: The API key used for authentication can be modified within the .env file or directly in the ApiKeyValidation class.
Port Mapping: The webapi service exposes port 8080. You can adjust this mapping in the docker-compose.yml file if needed.

### Usage

Once your containers are up and running, you can interact with the Product API using your preferred HTTP client or tool.

Endpoints

The API provides the following endpoints for managing products:

    GET /product?id={id}: Retrieve a product from the database with matching id
    GET /product/all: Retrieve all product
    POST /product: Create a new product, have to provide json file
    PUT /product?Product={product}: Update an existing product
    DELETE /product?Id={id}: Delete a product

JSON format, id is optional and will just be set to zero for auto generated id
    
    {
        "Id": 0, (int)
        "Name": "Name", (string)
        "Description": "Description", (string)
        "Price": 2.3 (double)
    }

### Using postman to connect:

    You have to add "X-API-Key" with the api key value and for post you have to
    provide a json file like I state above, remember that it has to be application/json

### Authentication:

Include the ApiKey header with the correct API key value in your requests to authenticate.

### Project Structure and Design Choices

Minimal API: The project uses Minimal API for a streamlined and focused approach to building APIs.
Static Extension Methods: API endpoints are organized into separate files using static extension methods, promoting modularity and maintainability.
Repository Pattern: The ProductRepository class encapsulates database interaction logic, adhering to the repository pattern for cleaner code structure.
Dependency Injection: The ProductRepository and ApiKeyValidation classes are registered as scoped services, making them available for injection throughout the application.
Custom Middleware: A custom middleware component (ApiKeyValidationMiddleware) is implemented to handle API key validation before requests reach the API endpoints.
Entity Framework Core: EF Core is used for database access and migrations, allowing for a code-first approach to database schema management.
Error Handling: The API includes error handling mechanisms to provide informative responses to the user in case of database errors or validation failures.

### Security Considerations

Environment Variables: Storing sensitive information (database credentials, API key) in a .env file helps protect them from accidental exposure. However, for production environments, consider using more robust secrets management solutions like Docker secrets or external secrets managers.
Input Validation: The API endpoints implement basic input validation to prevent invalid or malicious data from reaching the database.