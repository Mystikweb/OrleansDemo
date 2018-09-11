#!/bin/bash
dotnet restore ./src/DemoCluster.sln
dotnet build ./src/DemoCluster.sln
# docker build -t mystikweb/orleans-demo-silo:development -f DockerCluster .
# docker push mystikweb/orleans-demo-silo
# docker build -t mystikweb/orleans-demo-configuration:development -f DockerConfigApi .
# docker push mystikweb/orleans-demo-configuration
