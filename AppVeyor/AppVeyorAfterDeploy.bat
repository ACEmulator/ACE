@echo off
REM docker login -u="%DOCKER_USER%" -p="%DOCKER_PASS%"
REM @echo on
REM docker push acemulator/ace
@echo off

git config --global user.email "%GIT_A%"
git config --global user.name "%GIT_B%"

@echo on
REM git add -A
REM git commit -m "[ci skip] Updating project files with latest version information"
REM git push origin master
