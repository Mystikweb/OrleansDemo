FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DemoCluster.Messaging/DemoCluster.Messaging.csproj", "DemoCluster.Messaging/"]
COPY ["DemoCluster.GrainInterfaces/DemoCluster.GrainInterfaces.csproj", "DemoCluster.GrainInterfaces/"]
COPY ["DemoCluster.DAL/DemoCluster.DAL.csproj", "DemoCluster.DAL/"]
RUN dotnet restore "DemoCluster.Messaging/DemoCluster.Messaging.csproj"
COPY . .
WORKDIR "/src/DemoCluster.Messaging"
RUN dotnet build "DemoCluster.Messaging.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DemoCluster.Messaging.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DemoCluster.Messaging.dll"]