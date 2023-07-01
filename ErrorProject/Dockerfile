FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ErrorProject/ErrorProject.csproj", "ErrorProject/"]
RUN dotnet restore "ErrorProject/ErrorProject.csproj"
COPY . .
WORKDIR "/src/ErrorProject"
RUN dotnet build "ErrorProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ErrorProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ErrorProject.dll"]
