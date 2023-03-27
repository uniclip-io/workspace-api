FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WorkspaceApi/WorkspaceApi.csproj", "WorkspaceApi/"]
RUN dotnet restore "WorkspaceApi/WorkspaceApi.csproj"
COPY . .
WORKDIR "/src/WorkspaceApi"
RUN dotnet build "WorkspaceApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkspaceApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkspaceApi.dll"]
