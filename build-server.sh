#!/bin/bash
dotnet restore ./src/DemoCluster.sln
dotnet build ./src/DemoCluster.sln
docker build -t mystikweb/orleans-demo-silo -f DockerCluster .