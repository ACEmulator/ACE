@echo off
docker login -u="%DOCKER_USER%" -p="%DOCKER_PASS%"
@echo on
REM docker push acemulator/ace
