@echo on
del *.zip
del *.sql
del *.txt
@echo off
docker login -u="%DOCKER_USER%" -p="%DOCKER_PASS%"
@echo on

docker push acemulator/ace
