FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app
COPY ./FlutterApp.Core/*.csproj ./FlutterApp.Core/
COPY ./FlutterApp.Data/*.csproj ./FlutterApp.Data/
COPY ./FlutterApp.Api/*.csproj ./FlutterApp.Api/
COPY *.sln .
RUN dotnet restore
COPY . .
RUN dotnet publish ./FlutterApp.Api/*.csproj -o /publish/
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /publish .
ENV ASPNETCORE_URLS="http://*:3000"
ENTRYPOINT ["dotnet","FlutterApp.Api.dll"]