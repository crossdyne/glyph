FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["backend/Directory.Packages.props", "./"]

COPY ["backend/src/apps/Glyph.Bff/Glyph.Bff.csproj", "backend/src/apps/Glyph.Bff/"]

COPY ["backend/src/clients/FileService.Client/FileService.Client.csproj", "backend/src/clients/FileService.Client/"]

COPY ["backend/src/shared/contracts/Shared.Contracts/Shared.Contracts.csproj", "backend/src/shared/contracts/Shared.Contracts/"]
COPY ["backend/src/shared/core/Shared.Kernel/Shared.Kernel.csproj", "backend/src/shared/core/Shared.Kernel/"]
COPY ["backend/src/shared/core/Shared.Logging/Shared.Logging.csproj", "backend/src/shared/core/Shared.Logging/"]
COPY ["backend/src/shared/infrastructure/Shared.Http/Shared.Http.csproj", "backend/src/shared/infrastructure/Shared.Http/"]
COPY ["backend/src/shared/infrastructure/Shared.Redis/Shared.Redis.csproj", "backend/src/shared/infrastructure/Shared.Redis/"]
COPY ["backend/src/shared/web/Shared.Web/Shared.Web.csproj", "backend/src/shared/web/Shared.Web/"]

RUN dotnet restore "backend/src/apps/Glyph.Bff/Glyph.Bff.csproj"

COPY . .

WORKDIR "/src/backend/src/apps/Glyph.Bff"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0  AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Glyph.Bff.dll"]