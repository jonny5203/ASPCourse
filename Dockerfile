# Create a base image with dotnet environment, and configure the workdir as well as the port
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# The build stage begins by copying the project file and restoring the necessary NuGet packages for the application.
# Next, it copies the remaining project files and sets the working directory to the project's location.
# Finally, it builds the project using the specified build configuration and places the compiled output in the /app/build directory within the container.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProjectMicroservices/ProjectMicroservices.csproj", "ProjectMicroservices/"]
RUN dotnet restore "./ProjectMicroservices/ProjectMicroservices.csproj"
COPY . .
WORKDIR "/src/ProjectMicroservices"
RUN dotnet build "./ProjectMicroservices.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publishes the the project in the path /app/publish inside the container
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProjectMicroservices.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This this the final stage, using the base for the foundation
# It copies the published code into the working directory and sets the entrypoint,
# basically running the newly created dll file
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectMicroservices.dll"]