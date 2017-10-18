# KF2MapListTool
KF2MapListTool, or KF2MLT, is a simple tool that helps server owners with their configs in regards to custom maps from the Steam Workshop. It was quickly made in C# + WPF after my previous tool was scrapped for this more "elegant" solution.


## What's the point of this?
While being able to use maps directly from the Steam Workshop on your server is fantastic, it's a bit of a pain with how the current implementation is. Every map you wish to add to your server needs to have its Workshop ID placed in a file. Like so:
```
[OnlineSubsystemSteamworks.KFWorkshopSteamworks]
ServerSubscribedWorkshopItems=1086468006
ServerSubscribedWorkshopItems=1102122008
```
And in another file you need the map's filename (bar the extension) placed in a block of text. Like so:
```
[KF-SlipgateComplex KFMapSummary]
MapName=KF-SlipgateComplex
ScreenshotPathName=UI_MapPreview_TEX.UI_MapPreview_Nuked

[KF-ZedsDiner KFMapSummary]
MapName=KF-ZedsDiner
ScreenshotPathName=UI_MapPreview_TEX.UI_MapPreview_Nuked
```
For server owners that have a large collection of maps, or for someone that simply changes their map list frequently, this can be a huge pain. The solution was to automate it and remove some of the grunt work. KF2MLT does this by looking through your KF2 cache (where Workshop maps are downloaded) and gathering all the needed info. It then generates two files with the needed info for easy copy and paste.


## How do I use this?
Those that have hosted a KF2 server before will likely understand most of this already, but I'll try to explain this as cleanly as I can for any one new.

#### Generating the files
Simply download KF2MLT and run it. If the KF2 cache is found you will be greeted with a list of your subscribed maps and if not you'll be asked to point to its directory. The default location is `%userprofile%\Documents\My Games\KillingFloor2\KFGame\Cache`.

![KF2MLT](Screenshots/img1.png?raw=true)

You can use the checkboxes to exclude specific maps from being exported. Once ready click "Save" and the info will be placed into 2 files.

![KF2MLT](Screenshots/img2.png?raw=true)

#### KFEngine.txt and KFGame.txt
Copy the contents of `KFEngine.txt` and place them at the bottom of your server's `PCServer-KFEngine.ini`. If you have any custom map info from before, be sure to remove it first. Next do the same with `KFGame.txt` copied to the bottom of `PCServer-KFGame.ini`. Like with the previous config file, remove any custom map info you may have already had here. 

#### The map cycle
You'll need to update your map cycle via the web admin or in the [config](http://wiki.tripwireinteractive.com/index.php?title=Dedicated_Server_(Killing_Floor_2)#Maps). I highly recommend just using the web admin as it's fantastic for managing your server, setting up map cycles, and changing settings on the fly.

#### The download manager
If you've hosted workshop maps before you likely have done this part already. If not, you'll need to go into `PCServer-KFEngine.ini` and find the section `[IpDrv.TcpNetDriver]`. Right under the section marker you will need to place `DownloadManagers=OnlineSubsystemSteamworks.SteamWorkshopDownload`. It needs to be above all of the other DownloadManagers.


## Something isn't quite right, save me!
#### My cache is empty or is missing maps!
You may need to launch Killing Floor 2 for your cache to fill up with new or missing maps.

#### I'm seeing maps I'm not subscribed to!
If you join other servers with custom maps they will also be in your cache. If you do not wish to have these maps exported you can either uncheck them from the list or delete the cache and launch Killing Floor 2. After you do this only your subscribed maps will be present.


## Useful Links
* [Setting Up Steam Workshop For Servers](http://wiki.tripwireinteractive.com/index.php?title=Dedicated_Server_(Killing_Floor_2)#Setting_Up_Steam_Workshop_For_Servers)
* [Setting Up Web Admin](http://wiki.tripwireinteractive.com/index.php?title=Dedicated_Server_(Killing_Floor_2)#Setting_Up_Web_Admin)
* [Killing Floor 2 Steam Workshop](https://steamcommunity.com/workshop/browse/?appid=232090)

## License
KF2MLT is licensed under the MIT license. 