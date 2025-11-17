#!/usr/bin/env bash
set -euo pipefail

rm -rf nuget
mkdir nuget

dotnet run --project src/build "$@"