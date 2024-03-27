# 'sgs/budget-bringer-ui' 태그를 가진 이미지가 있는지 확인
if docker images | grep -q 'sgs/budget-bringer-ui'; then
    # 이미지가 있으면 삭제
    echo 'Found existing image. Deleting...'
    docker rmi sgs/budget-bringer-ui
fi

git fetch origin
git reset --hard origin/main
git clean -df

# 이미지를 새로 빌드
docker build -t sgs/budget-bringer-ui .
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
docker save sgs/budget-bringer-ui:latest -o "builds/$FILENAME"

echo "File saved to builds/$FILENAME"