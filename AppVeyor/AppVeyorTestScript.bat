@echo on
dotnet test --diag AppVeyor\ACETests-Server.txt --no-build "Source\ACE.Server.Tests" --test-adapter-path:. --logger:Appveyor
dotnet test --diag AppVeyor\ACETests-Database.txt --no-build "Source\ACE.Database.Tests" --test-adapter-path:. --logger:Appveyor
