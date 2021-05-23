﻿<div align="center"><img src="https://raw.githubusercontent.com/arjankuijpers/SpotifyRemote/297aadc6d2ca8b99afcb631bd2b4c1132a89fc31/VSIXSpotifyRemote/SpotifyRemoteLogo.png" width="200"></div align="center">
<div align="center">

# SpotifyRemote
<img src="https://arjankuijpers.gallerycdn.vsassets.io/extensions/arjankuijpers/spotifyremote/2.0/1510793706623/271382/1/2017-07-16_00-24-42.gif">

## For Visual Studio (2015/2017/2019)
</div align="center">

License:
![License MIT](https://img.shields.io/badge/license-MIT-blue.svg)  
Join the conversation on:
[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg)](https://gitter.im/SpotifyRemoteForVisualStudio/Lobby#)  
Master: [![Build status](https://ci.appveyor.com/api/projects/status/nkoom7kwayhiolbx/branch/master?svg=true)](https://ci.appveyor.com/project/arjankuijpers/spotifyremote/branch/master)



* [Getting Started & Installation](#getting-started)
* [Tips](#tips)
* [FAQ](#frequent-asked-questions)
* [About](#about-spotifyremote)
* [Bug report & feature request](https://github.com/arjankuijpers/SpotifyRemote/issues)

## 🚀 Getting started

#### 🔻 Installation:
1. [Download](https://marketplace.visualstudio.com/items?itemName=ArjanKuijpers.SpotifyRemote) the installer package (VSIX) from the [Visual Studio marketplace](https://marketplace.visualstudio.com/items?itemName=ArjanKuijpers.SpotifyRemote#review-details). **Note: Marketplace version compatible with 2015/2017**
2. Run the Visual Studio extension installer (VSIX).
3. Restart Visual Studio.
4. Go to **view** *>* **Toolbars** and Select **SpotifyRemote**  
<img src="https://raw.githubusercontent.com/arjankuijpers/SpotifyRemote/081ea5748298841b9385c684de59b0f88dfd9399/SpotifyRemote/docs/enable_tb_from_view.png" width="350">  

[Click here](https://raw.githubusercontent.com/arjankuijpers/SpotifyRemote/081ea5748298841b9385c684de59b0f88dfd9399/SpotifyRemote/docs/enable_tb_from_view.png) for full sized screenshot

## 💡 Tips

**Can it start Spotify from Visual Studio?**  
It will, when you click a command and Spotify is not running.

## ❓ Frequent Asked Questions
**Does it work on Visual Studio 2019?**  
*Yes it does, try it.*  

**What can SpotifyRemote do?**  
1. Currently you can do the basic commands such as:
 * Open Spotify,
 * Play
 * Pause
 * Next
 * Previous  

2. Showing Track name & Artist when it changes.  

*But that's not where it stops, See [Features](#features) to know what is planned.*
You can also make a feature request (at github).

**Can I help with development?**  
*Sure you can, please fork and do your developer magic. Afterwards create a [pull request](https://github.com/arjankuijpers/SpotifyRemote/pulls).  
And if your changes are stable it will be merged in the release version*

**What version of Windows do I need?**  
*At least* Windows 7 *, Visual Studio 2015 is only supported on Windows 7 and later*

**What version of Visual Studio do I need?**  
*At least* Visual Studio 2015

## ⚡ Features
### Supported
- [x] Start Spotify
- [x] Open Spotify
- [x] Play / Pause
- [x] Skip / Previous
- [x] Shows track title upon change
- [ ] Playlist browsing

### Planned
- [x] Create better interface (update icons).
- [x] Hide Text of play control buttons when spotify is not active.
- [x] Settings in tools menu.
- [ ] Multiple themes.
- [ ] Awesome features people request.

Do you miss a feature? Submit a request here: [Github](https://github.com/arjankuijpers/SpotifyRemote/issues)

## 🔫 Troubleshooting

**SpotifyRemote is not visible in the Visual studio**  
Go to **view** *>* **Toolbars** and Select **SpotifyRemote** it should show up at the top of Visual Studio.

The toolbar will be displayed over here.
<img src="https://raw.githubusercontent.com/arjankuijpers/SpotifyRemote/081ea5748298841b9385c684de59b0f88dfd9399/SpotifyRemote/docs/Enable_toolbar1.png" >

Go to toolbars and select the SpotifyRemote toolbar.  
<img src="https://raw.githubusercontent.com/arjankuijpers/SpotifyRemote/081ea5748298841b9385c684de59b0f88dfd9399/SpotifyRemote/docs/Enable_toolbar2.png" width="200">

The toolbar should now be visible. (icons may be different).
<img src="https://raw.githubusercontent.com/arjankuijpers/SpotifyRemote/081ea5748298841b9385c684de59b0f88dfd9399/SpotifyRemote/docs/Enable_toolbar3.png">



## 🔌 About SpotifyRemote

SpotifyRemote is a attempt to recreate the awesome [vscode-spotify](https://marketplace.visualstudio.com/items?itemName=shyykoserhiy.vscode-spotify) plugin from [shyykoserhiy](https://github.com/ShyykoSerhiy/vscode-spotify).  
This project is open source and I encourage forks and pull requests.  
Please submit bug reports and feature requests at [Github](https://github.com/arjankuijpers/SpotifyRemote/issues).

## 🛠 Building
To build Spotify Remote you need to have:

* Visual studio (2015/2017/2019)
* Visual studio extention development module
* Git

First to clone the repo, Launch a terminal and type:
```bash
git clone https://github.com/YT-GameWorks/SpotifyRemote.git
```
on your preferred directory.

Next Open the solution in visual studio and then Build it!

## 📄 License

[SpotifyRemote is released under the MIT License](https://raw.githubusercontent.com/arjankuijpers/SpotifyRemote/master/LICENSE).


