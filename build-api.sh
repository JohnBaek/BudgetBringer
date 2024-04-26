#!/bin/bash

# 현재 시간을 기반으로 백업 태그 생성
backup_tag=$(date +"%Y%m%d%H%M%S")

# "sgs/budget-bringer-api:latest" 이미지를 백업 태그로 저장
sudo docker tag sgs/budget-bringer-api:latest sgs/budget-bringer-api:$backup_tag

# 실행 중인 "sgs/budget-bringer-api" 컨테이너를 찾아 정지하고 삭제
container_id=$(docker ps -q -f name=budget-bringer-api)
if [ ! -z "$container_id" ]; then
    sudo docker stop $container_id
    sudo docker rm $container_id
fi

# "sgs/budget-bringer-api:latest" 이미지 삭제
sudo docker rmi sgs/budget-bringer-api:latest

# Docker 이미지 새로 빌드
sudo docker build -t sgs/budget-bringer-api .

# builds 폴더가 없으면 생성
if [ ! -d "builds" ]; then
    echo "Creating builds directory..."
    mkdir builds
fi

# builds 폴더 내의 기존 budget-bringer-api-*.tar 파일 삭제
echo "Removing existing budget-bringer-api-*.tar files..."
rm -f builds/budget-bringer-api-*.tar

# 현재 날짜와 시간을 포함하는 새 파일 이름을 생성
FILENAME="budget-bringer-api-$backup_tag.tar"
echo "Creating new file: $FILENAME"

# Docker 이미지를 '.tar' 파일로 저장
sudo docker save sgs/budget-bringer-api:latest -o "builds/$FILENAME"
echo "File saved to builds/$FILENAME"

# 컨테이너 재생성
docker run --name budget-bringer-api -e TZ=Asia/Seoul --restart=unless-stopped -v ./mysql:/var/lib/mysql --network=sgs-net --ip=172.28.0.11 -d sgs/budget-bringer-api:latest

# "sgs/budget-bringer-api" 이미지 중 백업 태그를 제외하고 삭제
sudo docker images | grep 'sgs/budget-bringer-api' | grep -v $backup_tag | awk '{print $3}' | xargs -r sudo docker rmi
