#!/bin/bash
dotnet restore
msbuild -p:Configuration=Release VehicleCleanup.csproj