@echo on
docker build -t acemulator/ace:latest -t acemulator/ace:%APPVEYOR_BUILD_VERSION% .
source\AppVeyor\MySqlInstall.bat
