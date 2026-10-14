# QuickRun

A modern reimagining of the classic Windows Run dialog.

QuickRun brings the familiar Windows "Run" experience into the Windows 11 era with a clean interface, modern design language, and a lightweight user experience.

![Platform](https://img.shields.io/badge/platform-Windows-blue)
![Status](https://img.shields.io/badge/status-Active%20Development-orange)

## Overview

QuickRun is a lightweight launcher inspired by the classic Windows Run dialog (`Win + R`).

The goal of the project is not to replace advanced launchers such as PowerToys Run, but to provide a modern, native-feeling alternative that respects the simplicity and immediacy of the original Windows experience.

## Features

### Current Features

- Modern Windows 11 inspired user interface
- Lightweight and fast startup
- Execute system commands
- Launch applications and executables
- Execute commands with arguments
- Built-in calculator
- Real-time command suggestions
- Keyboard-focused workflow
- Automatic input focus on startup
- Native WinUI 3 experience
- Support for Windows Shell shortcuts
- Minimal design with familiar behavior

### Supported Commands

#### System Tools

```text
notepad
calc
cmd
powershell
wt
explorer
regedit
taskmgr
control
msconfig
```

#### Administrative Tools

```text
services.msc
devmgmt.msc
diskmgmt.msc
compmgmt.msc
eventvwr.msc
gpedit.msc
```

#### Shell Shortcuts

```text
shell:startup
shell:downloads
shell:documents
shell:desktop
```

#### Control Panel / Settings

```text
ms-settings:
appwiz.cpl
ncpa.cpl
sysdm.cpl
```

### Calculator

QuickRun can evaluate mathematical expressions directly from the input box.

Examples:

```text
2+2
10*5
100/4
(12+3)*2
```

Results are displayed instantly in the launcher.

### Command Arguments

Applications can be started with command-line arguments.

Examples:

```text
notepad test.txt
cmd /k dir
powershell -NoExit
```

## Planned Features

- Command history
- Favorites and pinned commands
- Installed applications discovery
- Better autocomplete ranking
- URL handling
- Web search integration
- Global hotkey support
- Custom themes
- Plugin architecture

## Why QuickRun?

The Windows Run dialog has been part of Windows for decades and remains one of the fastest ways to launch commands and applications.

QuickRun modernizes this experience while preserving its original philosophy:

- Fast
- Simple
- Familiar
- Lightweight

Unlike feature-heavy launchers, QuickRun focuses on doing a few things exceptionally well.

## Screenshots

docs//screenshot.png


## Building

### Requirements

- Visual Studio 2022
- .NET 8 SDK
- Windows App SDK

### Build

```bash
git clone https://github.com/GiosuOffice/QuickRun.git
cd QuickRun
```

Open the solution in Visual Studio and build the project.

## Technology

QuickRun is built using:

- C#
- .NET 8
- WinUI 3
- Windows App SDK
- Fluent Design principles

## Roadmap

- [x] Modern WinUI 3 interface
- [x] Command execution
- [x] AutoSuggest support
- [x] Built-in calculator
- [x] Command argument support
- [ ] Command history
- [ ] Installed applications discovery
- [ ] Favorites
- [x] URL support (pre alpha)
- [ ] Search integration
- [ ] Global hotkey
- [ ] Plugin support
- [ ] Fluent Design improvements

## Version

### Version 0.7 Alpha

#### What's New

- Added built-in calculator support
- Added command argument support
- Improved command execution logic
- Improved autocomplete experience
- Added support for Windows Shell folders
- Various bug fixes and stability improvements

## Acknowledgments

Inspired by the classic Windows Run dialog and modern Windows 11 design guidelines.
