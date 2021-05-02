# VehicleCleanup
Simple LDM/Rocket plugin to manually or automatically clear empty vehicles on unturned servers. This is a modification of the original plugin, [AutoVehicleClear](https://github.com/PhaserArray/AutoVehicleClear) by PhaserArray.

## Installation
Installation is simple, just download the DLL and move it to your Server's `/Rocket/Plugins` directory. Upon starting the server, a default config wil be generated.

## Config
`Automatic` - false by default, if false, then the only way to clear vehicles is using the command.

`ClearInterval` - 600 by default, seconds between vehicle clears.

`SendClearMessage` - true by default, if false, a clear message will not be broadcasted.

`SendWarningMessage` - true by default, if false, there will be no warning shown on chat.

`WarningTime` - 30 by default, seconds between displaying the warning and clearing vehicles.

`ProtectLocked` - true by default, if true, locked vehicles won't be cleared.

`ClearAll` - false by default, if true, all empty vehicles will be cleared. Enabling this overrides the three options below.

`ClearExploded` - true by default, clears vehicle corpses.  

`ClearDrowned` - true by default, clears underwater vehicles that do not have a Bouyancy object (will not clear vehicles like boats, seaplanes, amphibious cars, etc)

`ClearNoTires` - true by default, clears vehicles that have tires in a healthy state but no longer have tires.

## Command
`/clearvehicles` (`/cv`) - Force a vehicle cleanup. Config variables are respected (including warning).

I haven't set up the permissions, because of insufficient docummentation of LDM. (I'm not sure how to do it properly, if you know how, please submit an issue or pull request)

## Building from source
In order to build the plugin on Linux, make sure you have installed `dotnet` and `mono`. Then run these commands (`build.sh`):
```
dotnet restore
msbuild -p:Configuration=Release VehicleCleanup.csproj
```
On Windows, please follow a tutorial on YouTube, or build the plugin yourself.
