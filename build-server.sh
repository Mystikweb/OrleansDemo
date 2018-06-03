#!/bin/bash
pwd
cd ./src
dotnet restore ./DemoCluster.sln
dotnet build ./DemoCluster.sln
docker build -t mystikweb/orleans-demo-server .