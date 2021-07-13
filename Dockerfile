FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

RUN curl -sL https://deb.nodesource.com/setup_14.x | bash - \
 && apt-get install -y --no-install-recommends nodejs \
 && echo "node version: $(node --version)" \
 && echo "npm version: $(npm --version)" \
 && rm -rf /var/lib/apt/lists/*

COPY . .
RUN dotnet restore

WORKDIR "/app/AureliaWithDotNet.Web"
RUN dotnet publish -c release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /out ./
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "AureliaWithDotNet.Web.dll"]
