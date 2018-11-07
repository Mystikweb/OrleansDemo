#!/bin/bash

echo "Creating staging folder for docker build of the runtime"

STAGING_DIR="./runtime-staging/"

if [ -d "$STAGING_DIR" ]; then rm -Rf "$STAGING_DIR"; fi

mkdir -p "$STAGING_DIR"

rsync -av --exclude 'bin/' --exclude 'obj/' src/DemoCluster "$STAGING_DIR"
rsync -av --exclude 'bin/' --exclude 'obj/' src/DemoCluster.DAL "$STAGING_DIR"
rsync -av --exclude 'bin/' --exclude 'obj/' src/DemoCluster.Hosting "$STAGING_DIR"
rsync -av --exclude 'bin/' --exclude 'obj/' --exclude 'Dockerfile' src/DemoCluster.Runtime "$STAGING_DIR"
rsync src/DemoCluster.Runtime/Dockerfile "$STAGING_DIR"

echo "Files copied to $STAGING_DIR starting Docker build process"
cd "$STAGING_DIR"
docker build -t ${TRAVIS_REPO_SLUG,,}runtime:${TRAVIS_BRANCH} .

echo "Docker image built successfully pushing to $REGISTRY_LOCATION"

echo ${REGISTRY_PASSWORD} | docker login https://${REGISTRY_LOCATION}/v2 --username ${REGISTRY_USER} --password-stdin

docker tag ${TRAVIS_REPO_SLUG,,}runtime:${TRAVIS_BRANCH} ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}runtime:${TRAVIS_BRANCH}
docker push ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}runtime:${TRAVIS_BRANCH}