# Stage 1: Build environment using .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first (to take advantage of Docker cache)
COPY *.sln ./
COPY StudentPortal.Data/*.csproj ./StudentPortal.Data/
COPY StudentPortal.Business/*.csproj ./StudentPortal.Business/
COPY StudentPortal.Models/*.csproj ./StudentPortal.Models/
COPY StudentPortal.Tests/*.csproj ./StudentPortal.Tests/
COPY StudentPortal.Web/*.csproj ./StudentPortal.Web/

# Restore NuGet packages
RUN dotnet restore

# Copy the remaining source code
COPY . .

# Publish the Web project to /app/publish using Release config
RUN dotnet publish StudentPortal.Web/StudentPortal.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime environment using ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80

# Copy the published output from build stage
COPY --from=build /app/publish .

# Set the entry point to run your web app
ENTRYPOINT ["dotnet", "StudentPortal.Web.dll"]
