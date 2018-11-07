FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /silo
EXPOSE 8080
EXPOSE 11111
EXPOSE 30000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DemoCluster/DemoCluster.csproj", "DemoCluster/"]
COPY ["DemoCluster.DAL/DemoCluster.DAL.csproj", "DemoCluster.DAL/"]
COPY ["DemoCluster.GrainImplementations/DemoCluster.GrainImplementations.csproj", "DemoCluster.GrainImplementations/"]
COPY ["DemoCluster.GrainInterfaces/DemoCluster.GrainInterfaces.csproj", "DemoCluster.GrainInterfaces/"]
COPY ["Orleans.Storage.Redis/Orleans.Storage.Redis.csproj", "Orleans.Storage.Redis/"]
COPY ["DemoCluster.Hosting/DemoCluster.Hosting.csproj", "DemoCluster.Hosting/"]
COPY ["DemoCluster.Silo/DemoCluster.Silo.csproj", "DemoCluster.Silo/"]
RUN dotnet restore "DemoCluster.Silo/DemoCluster.Silo.csproj" -v q
COPY . .
WORKDIR "/src/DemoCluster.Silo"
RUN dotnet build "DemoCluster.Silo.csproj" -v q -c Release -o /silo

FROM build AS publish
RUN dotnet publish "DemoCluster.Silo.csproj" -v q -c Release -o /silo

FROM base AS final
WORKDIR /silo
COPY --from=publish /silo .
ENTRYPOINT ["dotnet", "DemoCluster.Silo.dll"]