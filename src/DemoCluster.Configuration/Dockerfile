FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DemoCluster.Configuration/DemoCluster.Configuration.csproj", "DemoCluster.Configuration/"]
COPY ["DemoCluster.DAL/DemoCluster.DAL.csproj", "DemoCluster.DAL/"]
RUN dotnet restore "DemoCluster.Configuration/DemoCluster.Configuration.csproj"
COPY . .
WORKDIR "/src/DemoCluster.Configuration"
RUN dotnet build "DemoCluster.Configuration.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DemoCluster.Configuration.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DemoCluster.Configuration.dll"]