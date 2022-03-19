FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-dotnet
WORKDIR /src

COPY ["./DeKreyConsulting.StaticFileServer.csproj", "./"]
RUN dotnet restore

# COPY ./schemas/ ./schemas/
COPY ./ ./
# COPY ./Directory.Build.props .
# RUN dotnet build -p:SkipUiBuild=true
# RUN dotnet test --no-build
RUN dotnet build "DeKreyConsulting.StaticFileServer.csproj" -c Release -o /app/build

FROM build-dotnet AS publish-dotnet
RUN dotnet publish "DeKreyConsulting.StaticFileServer.csproj" -c Release -o /app/publish -p:SkipUiBuild=true

FROM base AS final

WORKDIR /app
COPY --from=publish-dotnet /app/publish .
ENTRYPOINT ["dotnet", "DeKreyConsulting.StaticFileServer.dll"]
