#!/bin/bash

docker login https://${REGISTRY_LOCATION}/v2 -u ${REGISTRY_USER} -p ${REGISTRY_PASSWORD}

docker build -t ${TRAVIS_REPO_SLUG,,}silo:${TRAVIS_BRANCH} -f DockerCluster .
docker tag ${TRAVIS_REPO_SLUG,,}silo:${TRAVIS_BRANCH} ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}silo:${TRAVIS_BRANCH}
docker push ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}silo:${TRAVIS_BRANCH}