cp ./Apis/Dockerfile ./

backup_tag=$(date +"%Y%m%d%H%M%S")

# 실행 중인 "sgs/budget-bringer-api" 컨테이너를 찾아 정지하고 삭제
container_id=$(docker ps | grep 'sgs/budget-bringer-api' | awk '{print $1}')
if [ ! -z "$container_id" ]; then
    sudo docker stop $container_id
    sudo docker rm $container_id

    sudo docker tag sgs/budget-bringer-api:latest sgs/budget-bringer-api:$backup_tag
fi

# "sgs/budget-bringer-api:latest"를 제외한 모든 "sgs/budget-bringer-api" 이미지를 삭제
sudo docker images | grep 'sgs/budget-bringer-api' | grep -v 'latest' | awk '{print $3}' | xargs -r sudo docker rmi

# 기존 최신 이미지 삭제
sudo docker rmi sgs/budget-bringer-api:latest

# 이미지를 새로 빌드
docker build -t sgs/budget-bringer-api .
sleep 5


# builds 폴더가 없으면 생성
if [ ! -d "builds" ]; then
    echo "Creating builds directory..."
    mkdir builds
fi

# builds 폴더 내의 기존 budget-bringer-api-*.tar 파일 삭제
echo "Removing existing budget-bringer-api-*.tar files..."
rm -f builds/budget-bringer-api-*.tar

# 현재 날짜와 시간을 포함하는 새 파일 이름을 생성
FILENAME="budget-bringer-api.tar"
echo "Creating new file: $FILENAME"

# Docker 이미지를 '.tar' 파일로 저장합니다.
sudo docker save sgs/budget-bringer-api:latest -o "builds/$FILENAME"

echo "File saved to builds/$FILENAME"

docker run --name budget-bringer-api -e TZ=Asia/Seoul --restart=unless-stopped -v ./mysql:/var/lib/mysql --network=sgs-net --ip=172.28.0.11 -d sgs/budget-bringer-api:latest

