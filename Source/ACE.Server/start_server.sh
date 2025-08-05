#!/usr/bin/env bash
cd "$(dirname "$0")"
if [ ! -f "ACE.Server.dll" ]; then
    echo "please run the copy of this file residing in the build output directory, e.g. ./bin/x64/<Configuration>/net8.0/"
    read -r -p "Press enter to exit..." _
    exit 1
fi
exec dotnet ACE.Server.dll
