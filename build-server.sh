#!/bin/bash
dotnet restore ./src/DemoCluster.sln
dotnet build ./src/DemoCluster.sln

docker login https://${REGISTRY_LOCATION} -u ${REGISTRY_USER} -p ${REGISTRY_PASSWORD}

docker build -t ${TRAVIS_REPO_SLUG,,}silo:${TRAVIS_BRANCH} -f DockerCluster .
docker tag ${TRAVIS_REPO_SLUG,,}silo:${TRAVIS_BRANCH} ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}silo:${TRAVIS_BRANCH}
# docker build -t mystikweb/orleans-demo-configuration:development -f DockerConfigApi .
# docker push mystikweb/orleans-demo-configuration
