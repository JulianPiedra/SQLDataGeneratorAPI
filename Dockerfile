# Adjust DOTNET_OS_VERSION as desired
ARG DOTNET_SDK_VERSION=8.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK_VERSION}${DOTNET_OS_VERSION} AS build
WORKDIR /src

# Here you can copy other linked projects, like a Data or Test project.
# COPY ["Project.Api.Tests/Project.Api.Tests.csproj", "./Project.Api.Tests/"]

# copy everything
COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_SDK_VERSION}
ENV ASPNETCORE_URLS http://+:8080
ENV ASPNETCORE_ENVIRONMENT Production
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "SQLDataGeneratorAPI.Endpoints.dll" ]