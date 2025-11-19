# --- Base image for running the app ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5202

# Tell ASP.NET Core to listen on port 5202
ENV ASPNETCORE_URLS=http://+:5202
ENV ASPNETCORE_ENVIRONMENT=Development

# --- SDK image for build + hot reload ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev
WORKDIR /src

# Copy only csproj first (faster restores)
COPY Api.csproj .
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Hot reload command for local development
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5202"]
