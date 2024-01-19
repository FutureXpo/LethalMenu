docker build -t lethal-menu-build-image -f Dockerfile .
docker run --name lethal-menu-temp lethal-menu-build-image
docker cp lethal-menu-temp:/App/LethalMenu.dll ./build/LethalMenu.dll
docker rm lethal-menu-temp
pause
pause