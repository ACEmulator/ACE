FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
ARG TARGETARCH
WORKDIR /Source

# copy csproj and restore as distinct layers
COPY ./Source/*.sln ./
COPY ./Source/ACE.Adapter/*.csproj ./ACE.Adapter/
COPY ./Source/ACE.Common/*.csproj ./ACE.Common/
COPY ./Source/ACE.Database/*.csproj ./ACE.Database/
COPY ./Source/ACE.Database.Tests/*.csproj ./ACE.Database.Tests/
COPY ./Source/ACE.DatLoader/*.csproj ./ACE.DatLoader/
COPY ./Source/ACE.DatLoader.Tests/*.csproj ./ACE.DatLoader.Tests/
COPY ./Source/ACE.Entity/*.csproj ./ACE.Entity/
COPY ./Source/ACE.Server/*.csproj ./ACE.Server/
COPY ./Source/ACE.Server.Tests/*.csproj ./ACE.Server.Tests/

RUN dotnet restore -a $TARGETARCH

# copy and publish app and libraries
COPY . ../.
RUN dotnet publish ./ACE.Server/ACE.Server.csproj -a $TARGETARCH -c release -o /ace --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:8.0-jammy
ARG DEBIAN_FRONTEND="noninteractive"
WORKDIR /ace

# install net-tools (netstat for health check) & cleanup
RUN apt-get update && \
    apt-get install --no-install-recommends -y \
    net-tools && \
    apt-get clean && \
    rm -rf \
    /tmp/* \
    /var/lib/apt/lists/* \
    /var/tmp/*

# add app from build
COPY --from=build /ace .

# run app
ENTRYPOINT ["dotnet", "ACE.Server.dll"]

# ports and volumes
EXPOSE 9000-9001/udp
VOLUME /ace/Config /ace/Content /ace/Dats /ace/Logs /ace/Mods

# health check
HEALTHCHECK --start-period=5m --interval=1m --timeout=3s \
  CMD netstat -an | grep 9000 > /dev/null; if [ 0 != $? ]; then exit 1; fi;
