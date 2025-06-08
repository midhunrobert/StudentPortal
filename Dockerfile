# Use the ASP.NET 8 runtime image as the base for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET 8 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire source code (all projects and solution) into the container
COPY . .

# Restore dependencies for the entire solution
RUN dotnet restore StudentPortal.sln

# Publish the Web project only, using Release configuration, output to /app/publish
RUN dotnet publish StudentPortal.Web/StudentPortal.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final stage: build runtime image
FROM base AS final
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Set the startup command to run your web app
ENTRYPOINT ["dotnet", "StudentPortal.Web.dll"]
