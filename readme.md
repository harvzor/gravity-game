# Gravity

## Requirements:

- Godot Mono 3.2.2
- .NET Framework v4.7 SDK
- NuGet 5.6.0.6489

## Getting started

- run `nuget restore`
- open with Godot and build
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
