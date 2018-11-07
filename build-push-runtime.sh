#!/bin/bash

echo ${REGISTRY_PASSWORD} | docker login https://${REGISTRY_LOCATION}/v2 --username ${REGISTRY_USER} --password-stdin

docker build -t ${TRAVIS_REPO_SLUG,,}messaging:${TRAVIS_BRANCH} -f DockerMessaging .
docker tag ${TRAVIS_REPO_SLUG,,}messaging:${TRAVIS_BRANCH} ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}messaging:${TRAVIS_BRANCH}
docker push ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}messaging:${TRAVIS_BRANCH}