cp ./BudgetBringer/Dockerfile ./

backup_tag=$(date +"%Y%m%d%H%M%S")

# "sgs/budget-bringer-ui:latest"를 제외한 모든 "sgs/budget-bringer-ui" 이미지를 삭제
sudo docker images | grep 'sgs/budget-bringer-ui' | grep -v 'latest' | awk '{print $3}' | xargs -r docker rmi

# 실행 중인 "sgs/budget-bringer-ui" 컨테이너를 찾아 정지하고 삭제
container_id=$(docker ps | grep 'sgs/budget-bringer-ui' | awk '{print $1}')
if [ ! -z "$container_id" ]; then
    sudo docker stop $container_id
    sudo docker rm $container_id

    sudo docker tag sgs/budget-bringer-ui:latest sgs/budget-bringer-ui:$backup_tag
fi

# 기존 최신 이미지 삭제
sudo docker rmi sgs/budget-bringer-ui:latest

# 이미지를 새로 빌드
sudo docker build -t sgs/budget-bringer-ui .
sleep 5


# builds 폴더가 없으면 생성
if [ ! -d "builds" ]; then
    echo "Creating builds directory..."
    mkdir builds
fi

# builds 폴더 내의 기존 budget-bringer-ui-*.tar 파일 삭제
echo "Removing existing budget-bringer-ui-*.tar files..."
rm -f builds/budget-bringer-ui-*.tar

# 현재 날짜와 시간을 포함하는 새 파일 이름을 생성
FILENAME="budget-bringer-ui.tar"
echo "Creating new file: $FILENAME"

# Docker 이미지를 '.tar' 파일로 저장합니다.
sudo docker save sgs/budget-bringer-ui:latest -o "builds/$FILENAME"

echo "File saved to builds/$FILENAME"

docker run --name budget-bringer-ui -e TZ=Asia/Seoul --restart=unless-stopped --network=sgs-net --ip=172.28.0.10 -d sgs/budget-bringer-ui:latest

