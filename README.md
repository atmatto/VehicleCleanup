# AutoVehicleClear
Simple Rocket plugin to automatically clear empty vehicles on unturned servers.

Installation is simple, just download the DLL and move it to your Server's `/Rocket/Plugins` directory. Upon starting the server, a default config wil be generated.

The config contains six values:  
`ClearInterval` - 600 by default, seconds between vehicle clears.  
`SendClearMessage` - true by default, if false, a clear message will not be broadcasted.  
`ClearAll` - false by default, if true, all empty vehicles will be cleared, enabling this overrides the three options below.  
`ClearExploded` - true by default, clears vehicle corpses.  
`ClearDrowned` - true by default, clears underwater vehicles that do not have a Bouyancy object (will not clear vehicles like boats, seaplanes, amphibious cars, etc)  
`ClearNoTires` - true by default, clears vehicles that have tires in a healthy state but no longer have tires.
