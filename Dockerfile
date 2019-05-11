FROM microsoft/dotnet:2.2-sdk-alpine AS build
WORKDIR /app

### Copy everything from source
WORKDIR /src
COPY . / ./

### And build
FROM build AS publish
RUN dotnet publish -c Release -o /app

### Copy publish to app folder
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /app
COPY --from=publish /app ./

EXPOSE 30540/tcp
ENV ASPNETCORE_URLS http://*:30540
ENTRYPOINT ["dotnet", "ElasticErrorRates.API.dll", "http://*:30540"]