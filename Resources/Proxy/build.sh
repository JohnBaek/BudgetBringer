#!/bin/bash

# 실행 중인 sgs/budget-bringer-proxy:latest 컨테이너 ID
container_id=$(docker ps -q -f ancestor=sgs/budget-bringer-proxy:latest)

# 컨테이너 ID가 존재하면 해당 컨테이너를 멈추고 삭제합니다.
if [ ! -z "$container_id" ]; then
    echo "Stopping and removing existing container..."
    docker stop $container_id
    docker rm $container_id
fi

# Docker 이미지를 빌드
echo "Building new Docker image..."
docker build -t sgs/budget-bringer-proxy .

echo "Starting new container..."
docker run -d --name budget-bringer-proxy -p --network=sgs-net --restart=unless-stopped --ip=172.28.0.20 -p 8000:80 sgs/budget-bringer-proxy:latest

echo "Deployment completed successfully."
