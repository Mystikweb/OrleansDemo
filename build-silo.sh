#!/bin/bash
dotnet build ./src/DemoCluster.sln

# docker build -t mystikweb/orleans-demo-configuration:development -f DockerConfigApi .
# docker push mystikweb/orleans-demo-configuration
