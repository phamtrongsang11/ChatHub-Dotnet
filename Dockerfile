FROM mcr.microsoft.com/dotnet/aspnet:8.0-runtime AS base

WORKDIR /app

COPY . .

FROM mcr.microsoft.com/dotnet/aspnet:8.0-sdk AS build

WORKDIR /src

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM base

COPY --from=build /src/out/ out

ENTRYPOINT ["dotnet", "teamchat.dll"]