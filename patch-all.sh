echo Init 
sudo sh ./init.sh

#echo build-database
#sudo sh ./build-database.sh

echo build-api
sudo sh ./build-api.sh

echo patch-ui 
sudo sh ./build-ui.sh

echo patch-proxy 
sudo sh ./build-proxy.sh

echo Patch Complete


