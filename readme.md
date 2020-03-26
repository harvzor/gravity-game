# Gravity

## Touch controls

If [this](https://github.com/godotengine/godot/pull/36953) is implemented, I'll be able to setup pinch to zoom controls easily.

## Exporting

### Android

Follow this guide: https://docs.godotengine.org/en/3.2/getting_started/workflow/export/exporting_for_android.html

Export to APK, then install to connected phone:

```
godot --export-debug "Android" bin/gravity.apk && adb install bin/gravity.apk
```
