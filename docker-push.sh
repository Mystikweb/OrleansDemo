#!/bin/bash

echo "Attempting registry login to $REGISTRY_LOCATION..."

echo ${REGISTRY_PASSWORD} | docker login https://${REGISTRY_LOCATION}/v2 --username ${REGISTRY_USER} --password-stdin

DOCKER_TAG="latest"

if [ ${TRAVIS_BRANCH} != "master" ]; then DOCKER_TAG=${TRAVIS_BRANCH}; fi

echo "Build $DOCKER_TAG images"

echo "Creating staging folders for docker build"

SILO_STAGING_DIR="./silo-staging/"
CONFIGURATION_STAGING_DIR="./configuration-staging/"
RUNTIME_STAGING_DIR="./runtime-staging"

if [ -d ${SILO_STAGING_DIR} ]; then rm -Rf ${SILO_STAGING_DIR}; fi
if [ -d ${CONFIGURATION_STAGING_DIR} ]; then rm -Rf ${CONFIGURATION_STAGING_DIR}; fi
if [ -d ${RUNTIME_STAGING_DIR} ]; then rm -Rf ${RUNTIME_STAGING_DIR}; fi

mkdir -p ${SILO_STAGING_DIR}
mkdir -p ${CONFIGURATION_STAGING_DIR}
mkdir -p ${RUNTIME_STAGING_DIR}

echo "Starting copy of files to staging folders..."

rsync -a src/DemoCluster ${SILO_STAGING_DIR}
rsync -a src/DemoCluster.DAL ${SILO_STAGING_DIR}
rsync -a src/DemoCluster.GrainImplementations ${SILO_STAGING_DIR}
rsync -a src/DemoCluster.GrainInterfaces ${SILO_STAGING_DIR}
rsync -a src/DemoCluster.Hosting ${SILO_STAGING_DIR}
rsync -a src/Orleans.Storage.Redis ${SILO_STAGING_DIR}
rsync -a --exclude 'Dockerfile' src/DemoCluster.Silo ${SILO_STAGING_DIR}
rsync src/DemoCluster.Silo/Dockerfile ${SILO_STAGING_DIR}

rsync -a src/DemoCluster ${CONFIGURATION_STAGING_DIR}
rsync -a src/DemoCluster.DAL ${CONFIGURATION_STAGING_DIR}
rsync -a src/DemoCluster.Hosting ${CONFIGURATION_STAGING_DIR}
rsync -a --exclude 'Dockerfile' src/DemoCluster.Configuration ${CONFIGURATION_STAGING_DIR}
rsync src/DemoCluster.Configuration/Dockerfile ${CONFIGURATION_STAGING_DIR}

rsync -a src/DemoCluster ${RUNTIME_STAGING_DIR}
rsync -a src/DemoCluster.DAL ${RUNTIME_STAGING_DIR}
rsync -a src/DemoCluster.Hosting ${RUNTIME_STAGING_DIR}
rsync -a src/DemoCluster.GrainInterfaces ${RUNTIME_STAGING_DIR}
rsync -a --exclude 'Dockerfile' src/DemoCluster.Runtime ${RUNTIME_STAGING_DIR}
rsync src/DemoCluster.Runtime/Dockerfile ${RUNTIME_STAGING_DIR}

echo "Files copied to staging folders successfully...starting Docker build process for silo container..."

cd ${SILO_STAGING_DIR}
docker build -t ${TRAVIS_REPO_SLUG,,}silo:${DOCKER_TAG} .

echo "Docker silo container built successfully...pushing to $REGISTRY_LOCATION..."

docker tag ${TRAVIS_REPO_SLUG,,}silo:${DOCKER_TAG} ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}silo:${DOCKER_TAG}
docker push ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}silo:${DOCKER_TAG}

echo "Docker image pushed successfully....switching to configuration container..."

cd ..
cd ${CONFIGURATION_STAGING_DIR}
docker build -t ${TRAVIS_REPO_SLUG,,}configuration:${DOCKER_TAG} .

echo "Docker configuration container built successfully...pushing to $REGISTRY_LOCATION..."

docker tag ${TRAVIS_REPO_SLUG,,}configuration:${DOCKER_TAG} ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}configuration:${DOCKER_TAG}
docker push ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}configuration:${DOCKER_TAG}

echo "Docker image pushed successfully....switching to runtime container..."

cd ..
cd ${RUNTIME_STAGING_DIR}
docker build -t ${TRAVIS_REPO_SLUG,,}runtime:${DOCKER_TAG} .

echo "Docker runtime container built successfully...pushing to $REGISTRY_LOCATION..."

docker tag ${TRAVIS_REPO_SLUG,,}runtime:${DOCKER_TAG} ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}runtime:${DOCKER_TAG}
docker push ${REGISTRY_LOCATION}/${TRAVIS_REPO_SLUG,,}runtime:${DOCKER_TAG}