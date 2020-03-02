@echo on
dotnet test --diag ACETests-Server.txt --no-build "Source\ACE.Server.Tests" --test-adapter-path:. --logger:Appveyor
dotnet test --diag ACETests-Database.txt --no-build "Source\ACE.Database.Tests" --test-adapter-path:. --logger:Appveyor
