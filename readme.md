# Gravity

## Requirements:

- Godot Mono v3.5.stable
- DotNet 6.0.302

## Getting started

- run `dotnet restore`
- open with Godot and build
    - you may need to set `dotnet cli` as the build tool in Godot
        - Editor -> Editor settings -> Mono -> Builds -> Build Tool -> dotnet CLI
    - if you run into an issue such as `The build method threw an exception.` and `Cannot find executable for 'dotnet CLI'. Fallback to MSBuild from Mono.`, this issue may be present: https://github.com/godotengine/godot/issues/38985
        - run Godot from the CLI, then build
- run

## Touch controls

If [this](https://github.com/godotengine/godot/pull/36953) is implemented, I'll be able to setup pinch to zoom controls easily.

## Exporting

### Android

Follow this guide: https://docs.godotengine.org/en/3.2/getting_started/workflow/export/exporting_for_android.html

Export to APK, then install to connected phone:

```
godot --export-debug "Android" bin/gravity.apk && adb install bin/gravity.apk
```
