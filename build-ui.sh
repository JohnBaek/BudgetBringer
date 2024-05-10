#!/bin/bash
cp ./BudgetBringer/Dockerfile ./
cp ./BudgetBringer/default.conf ./

# Step 1: 실행 중인 "sgs/budget-bringer-ui" 컨테이너를 찾아 정지하고 삭제
container_id=$(docker ps -q -f name=budget-bringer-ui)
if [ ! -z "$container_id" ]; then
    echo "Stopping and removing container: $container_id"
    sudo docker stop $container_id
    sudo docker rm $container_id
fi

# Step 2: "sgs/budget-bringer-ui:latest"를 제외한 모든 태그가 달린 이미지를 삭제
sudo docker images | grep 'sgs/budget-bringer-ui' | grep -v 'latest' | awk '{print $1 ":" $2}' | xargs -r sudo docker rmi

# Step 3: "sgs/budget-bringer-ui:latest" 이미지에 오늘의 날짜와 시간으로 태그 달기
backup_tag=$(date +"%Y%m%d%H%M%S")
sudo docker tag sgs/budget-bringer-ui:latest sgs/budget-bringer-ui:$backup_tag
echo "Tagged image with backup tag: $backup_tag"

# Step 4: "latest" 태그가 달린 이미지 삭제하여 태그 달린 이미지만 남기기
sudo docker rmi sgs/budget-bringer-ui:latest
echo "Removed the 'latest' tag from the image, backup retained as $backup_tag"

# Step 5: Docker 이미지를 새로 빌드하고 "latest"로 태그 달기
sudo docker build -t sgs/budget-bringer-ui .

# Step 6: 컨테이너를 올리기
sudo docker run --name budget-bringer-ui -e TZ=Asia/Seoul --restart=unless-stopped --network=sgs-net --ip=172.28.0.10 -d sgs/budget-bringer-ui:latest
echo "Container 'budget-bringer-ui' is up and running."
