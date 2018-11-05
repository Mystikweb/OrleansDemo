#!/bin/bash
dotnet build -v m ./src/DemoCluster.sln

# docker build -t mystikweb/orleans-demo-configuration:development -f DockerConfigApi .
# docker push mystikweb/orleans-demo-configuration
