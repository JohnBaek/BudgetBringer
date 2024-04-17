# git fetch origin
# git reset --hard origin/main
# git clean -df

cp ./Resources/DataBase/Dockerfile ./

backup_tag=$(date +"%Y%m%d%H%M%S")

# "sgs/budget-bringer-database:latest"를 제외한 모든 "sgs/budget-bringer-database" 이미지를 삭제
sudo docker images | grep 'sgs/budget-bringer-database' | grep -v 'latest' | awk '{print $3}' | xargs -r docker rmi

# 실행 중인 "sgs/budget-bringer-database" 컨테이너를 찾아 정지하고 삭제
container_id=$(docker ps | grep 'sgs/budget-bringer-database' | awk '{print $1}')
if [ ! -z "$container_id" ]; then
    sudo docker stop $container_id
    sudo docker rm $container_id

    sudo docker tag sgs/budget-bringer-database:latest sgs/budget-bringer-database:$backup_tag
fi

# 기존 최신 이미지 삭제
sudo docker rmi sgs/budget-bringer-database:latest

# 이미지를 새로 빌드
docker build -t sgs/budget-bringer-database .
sleep 5


# builds 폴더가 없으면 생성
if [ ! -d "builds" ]; then
    echo "Creating builds directory..."
    mkdir builds
fi

# builds 폴더 내의 기존 budget-bringer-database-*.tar 파일 삭제
echo "Removing existing budget-bringer-database-*.tar files..."
rm -f builds/budget-bringer-database-*.tar

# 현재 날짜와 시간을 포함하는 새 파일 이름을 생성
FILENAME="budget-bringer-database.tar"
echo "Creating new file: $FILENAME"

# Docker 이미지를 '.tar' 파일로 저장합니다.
sudo docker save sgs/budget-bringer-database:latest -o "builds/$FILENAME"

echo "File saved to builds/$FILENAME"

sudo docker run --name budget-bringer-database -e TZ=Asia/Seoul --restart=unless-stopped -v /mnt/datadisk1/budget-data:/var/lib/mysql --network=sgs-net --ip=172.28.0.30 -p 3309:3306 -d sgs/budget-bringer-database:latest 
