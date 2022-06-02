### restore & build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /sln

COPY ./*.sln ./src/*/*.csproj ./
RUN mkdir src
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
RUN dotnet restore "Auth.sln"
COPY . .
RUN dotnet build "Auth.sln" -c Release --no-restore
RUN dotnet publish "./src/Auth.Services/Auth.Services.csproj" -c Release --output /dist/services --no-restore
RUN dotnet publish "./src/Auth.Worker/Auth.Worker.csproj" -c Release --output /dist/worker --no-restore
RUN dotnet publish "./src/Auth.Migrator/Auth.Migrator.csproj" -c Release --output /dist/migrator --no-restore

### tests image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS tests
WORKDIR /tests
COPY --from=build /sln .
ENTRYPOINT ["dotnet", "test", "-c", "Release", "--no-build"]

### services image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS services
WORKDIR /app
COPY --from=build /dist/services ./
ARG revision=Unknown
ENTRYPOINT [ "dotnet", "Auth.Services.dll" ]

### worker image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS worker
WORKDIR /app
COPY --from=build /dist/worker ./
ARG revision=Unknown
ENTRYPOINT [ "dotnet", "Auth.Worker.dll" ]

### migrator image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS migrator
WORKDIR /app
COPY --from=build /dist/migrator ./
ARG revision=Unknown
ENTRYPOINT [ "dotnet", "Auth.Migrator.dll" ]