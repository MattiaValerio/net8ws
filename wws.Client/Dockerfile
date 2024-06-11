FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
EXPOSE 80
EXPOSE 443
WORKDIR /app
COPY . .
RUN dotnet restore wws.Client.csproj
COPY . .
RUN dotnet build wws.Client.csproj -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish wws.Client.csproj -c Release -o /app/publish
 
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .