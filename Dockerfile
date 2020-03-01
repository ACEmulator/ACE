FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /Source

# copy csproj and restore as distinct layers (credit: https://code-maze.com/aspnetcore-app-dockerfiles/)
COPY ./Source/*.sln ./
COPY ./Source/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore

# copy and publish app and libraries
COPY . ../.
RUN dotnet publish ./ACE.Server/ACE.Server.csproj -c release -o /ace --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /ace
COPY --from=build /ace .
RUN apt-get update \
  && DEBIAN_FRONTEND=noninteractive apt-get install -y \
    net-tools \
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/*
ENTRYPOINT ["dotnet", "ACE.Server.dll"]

# ports and volumes
EXPOSE 9000/udp
EXPOSE 9001/udp
VOLUME /ace/Logs
VOLUME /ace/Dats

# health check
HEALTHCHECK --start-period=5m --interval=1m --timeout=3s \
  CMD netstat -an | grep 9000 > /dev/null; if [ 0 != $? ]; then exit 1; fi;
