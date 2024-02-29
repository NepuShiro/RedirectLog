# RedirectLog

#### A Plugin for [BepInEx](https://github.com/BepInEx/BepInEx/) that is made to redirect the logs of the game [Resonite](https://store.steampowered.com/app/2519830/Resonite/) to the BepInEx Console

### Note you might have issues with some select mods when installing BepInEx.

### Installation
 Download and extract [BepInEx 5.4.22 x64](https://github.com/BepInEx/BepInEx/releases) into the main game folder and run the game once to generate the folders and config files

> BCE Is no longer needed on versions >1.0.2

 ~~You will also need to download [BCE](https://github.com/NepuShiro/BepInEx-Console-Extensions/releases/tag/1.1.0.0) (BepInEx Console Extensions) and place this into `/Resonite/BepInEx/plugins`~~

 Finally download the `RedirectLog.dll` from the [Releases](https://github.com/nepushiro/RedirectLog/releases) and place this into `/Resonite/BepInEx/plugins`

To get the Console to appear it is disabled by default in the BepInEx config `/Resonite/BepInEx/plugins`

Example GameDir - `C:\Program Files (x86)\Steam\steamapps\common\Resonite\BepInEx\config\BepInEx.cfg`
```
[Logging.Console]

## Enables showing a console for log output.
# Setting type: Boolean
# Default value: false
Enabled = true         <-- Change this to True
```

Now your Console should be displaying the logs!

#### For Enabling TimeStamps and/or FPS

This will be in the Config `/Resonite/BepInEx/config/org.Nep.RedirectLog.cfg`

```
[General]

## Whether or not to show TimeStamps in the Log
# Setting type: Boolean
# Default value: true
Enable TimeStamps = true

## Whether or not to show the FPS in the Log
# Setting type: Boolean
# Default value: false
Enable FPS Log = false
```
