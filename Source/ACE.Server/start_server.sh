#!/usr/bin/env bash
cd "$(dirname "$0")"
if [ ! -f ACE.Server.dll ]; then
  echo "please run the copy of this file residing in the output folder: ./bin/x64/XXXXX/net8.0/"
  read -p "Press enter to continue..." dummy
fi
exec dotnet ACE.Server.dll
