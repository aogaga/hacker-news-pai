# Stage 1: Build + restore
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and projects
COPY ../HackerNews.sln ./
COPY ../Api ./Api
COPY ../Api.Test.Unit ./Api.Test.Unit
COPY ../Api.Test.Integration ./Api.Test.Integration

# Restore projects
RUN dotnet restore HackerNews.sln

# Stage 2: Build and run integration tests
FROM build AS dev
WORKDIR /src

# Mount local source code (so you can edit without rebuilding)
VOLUME ["/src/Api", "/src/Api.Test.Integration"]

# Default command: run tests
CMD ["dotnet", "test", "Api.Test.Integration/Api.Test.Integration.csproj", "--logger:trx"]
