@echo off
REM docker login -u="%DOCKER_USER%" -p="%DOCKER_PASS%"
REM @echo on
REM docker push acemulator/ace
@echo off

git config --global user.email "%GIT_A%"
git config --global user.name "%GIT_B%"

@echo on
git checkout master
git add -f Source\ACE.Server\ServerBuildInfo_Dynamic.cs
git commit -m "[ci skip] Updating ServerBuildInfo_Dynamic with latest build and version information"
git push origin master
