FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5005

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DemoCluster.Runtime/DemoCluster.Runtime.csproj", "DemoCluster.Runtime/"]
COPY ["DemoCluster.GrainInterfaces/DemoCluster.GrainInterfaces.csproj", "DemoCluster.GrainInterfaces/"]
COPY ["DemoCluster/DemoCluster.csproj", "src/DemoCluster/"]
COPY ["DemoCluster.Hosting/DemoCluster.Hosting.csproj", "DemoCluster.Hosting/"]
COPY ["DemoCluster.DAL/DemoCluster.DAL.csproj", "DemoCluster.DAL/"]
RUN dotnet restore "DemoCluster.Runtime/DemoCluster.Runtime.csproj" -v q
COPY . .
WORKDIR "/src/DemoCluster.Runtime"
RUN dotnet build "DemoCluster.Runtime.csproj" -v q -c Release -o /app

FROM build AS publish
RUN dotnet publish "DemoCluster.Runtime.csproj" -v q -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DemoCluster.Runtime.dll"]