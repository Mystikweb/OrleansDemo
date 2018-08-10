#!/bin/bash
dotnet restore ./src/DemoCluster.sln
dotnet build ./src/DemoCluster.sln
docker build -t mystikweb/orleans-demo-silo:development -f DockerCluster .
docker build -t mystikweb/orleans-demo-configuration:development -f DockerConfigApi .