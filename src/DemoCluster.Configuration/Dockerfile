FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DemoCluster/DemoCluster.csproj", "DemoCluster/"]
COPY ["DemoCluster.DAL/DemoCluster.DAL.csproj", "DemoCluster.DAL/"]
COPY ["DemoCluster.Hosting/DemoCluster.Hosting.csproj", "DemoCluster.Hosting/"]
COPY ["DemoCluster.Configuration/DemoCluster.Configuration.csproj", "DemoCluster.Configuration/"]
RUN dotnet restore "DemoCluster.Configuration/DemoCluster.Configuration.csproj" -v q
COPY . .
WORKDIR "/src/DemoCluster.Configuration"
RUN dotnet build "DemoCluster.Configuration.csproj" -v q -c Release -o /app

FROM build AS publish
RUN dotnet publish "DemoCluster.Configuration.csproj" -v q -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DemoCluster.Configuration.dll"]