FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files for restore
COPY *.sln ./
COPY StudentPortal.Data/*.csproj ./StudentPortal.Data/
COPY StudentPortal.Business/*.csproj ./StudentPortal.Business/
COPY StudentPortal.Models/*.csproj ./StudentPortal.Models/
COPY StudentPortal.Web/*.csproj ./StudentPortal.Web/
COPY StudentPortal.Tests/*.csproj ./StudentPortal.Tests/
# (tests usually not needed in build container)

# Restore dependencies for the whole solution
RUN dotnet restore

# Copy everything else now
COPY . .

# Publish the Web project only
RUN dotnet publish StudentPortal.Web/StudentPortal.Web.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StudentPortal.Web.dll"]
