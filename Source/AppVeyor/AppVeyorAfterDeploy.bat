@echo on
del *.zip
del *.sql
del *.txt

docker login -u="%DOCKER_USER%" -p="%DOCKER_PASS%"

docker build -t acemulator/ace:latest -t acemulator/ace:%APPVEYOR_BUILD_VERSION% .

docker push acemulator/ace
