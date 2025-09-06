
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./
COPY Auth.Api/Auth.Api.csproj Auth.Api/
COPY . ./


ARG PROJECT=Auth.Api/Auth.Api.csproj

RUN dotnet restore "$PROJECT"
RUN dotnet publish "$PROJECT" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "Auth.Api.dll"]
