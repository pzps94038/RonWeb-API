FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

ARG RON_WEB_MONGO_DB_CONSTR
ARG ORIGINS
ARG JWTKEY
ARG ISSUER
ARG AUDIENCE
ARG AESIV
ARG AESKEY

ENV RON_WEB_MONGO_DB_CONSTR=$RON_WEB_MONGO_DB_CONSTR \
    ORIGINS=$ORIGINS \
    JWTKEY=$JWTKEY \
    ISSUER=$ISSUER \
    AUDIENCE=$AUDIENCE \
    AESIV=$AESIV \
    AESKEY=$AESKEY
# Copy File
COPY ["RonWeb.API/RonWeb.API.csproj", "RonWeb.API/"]
COPY ["RonWeb.Database/RonWeb.Database.csproj", "RonWeb.Database/"]
COPY ["RonWeb.Core/RonWeb.Core.csproj", "RonWeb.Core/"]
RUN dotnet restore "RonWeb.API/RonWeb.API.csproj"
RUN dotnet restore "RonWeb.Database/RonWeb.Database.csproj"
RUN dotnet restore "RonWeb.Core/RonWeb.Core.csproj"
# copy everything else and build app
COPY . .

RUN dotnet build "RonWeb.API/RonWeb.API.csproj" -c Release -o /app/build
# Publish
FROM build AS publish
RUN dotnet publish "RonWeb.API/RonWeb.API.csproj" -c Release -o /app/publish
# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet RonWeb.API.dll