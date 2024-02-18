FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TeamChat.csproj", "."]
RUN dotnet restore "./TeamChat.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TeamChat.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "TeamChat.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamChat.dll"]