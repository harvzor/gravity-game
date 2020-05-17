using Godot;
using System;

public class Global : Node
{
    private StorageManager _storageManager;
    public StorageManager StorageManager
    {
        get
        {
            if (this._storageManager == null)
                this._storageManager = new StorageManager();

            return this._storageManager;
        }
    }

    public Vector2? Zoom;
    public int HighestLevelUnlocked
    {
        get => this.StorageManager.Storage.HighestLevelUnlocked;
        set
        {
            this.StorageManager.Storage.HighestLevelUnlocked = value;
            this.StorageManager.Save();
        }
    }

    public int MusicVolume
    {
        get => this.StorageManager.Storage.MusicVolume;
        set
        {
            this.StorageManager.Storage.MusicVolume = value;
            this.StorageManager.Save();

            this.Music.VolumeDb = this.CalculateDecibels(value);
        }
    }

	public AudioStreamPlayer CrashSound => base.GetNode<AudioStreamPlayer>("Sound/Crash");
	public AudioStreamPlayer Coin => base.GetNode<AudioStreamPlayer>("Sound/Coin");
	public AudioStreamPlayer Music => base.GetNode<AudioStreamPlayer>("Music");

    public override void _Ready()
    {
        this.Music.VolumeDb = this.CalculateDecibels(this.MusicVolume);
    }

    public void ChangeMusic(AudioStream audioStream)
    {
        if (this.Music.Stream != null && audioStream.ResourcePath == this.Music.Stream.ResourcePath)
            return;

        this.Music.Stream = audioStream;

        if (this.Music.Playing == false)
            this.Music.Play();
    }

    private float CalculateDecibels(float percentage)
    {
        // Minimum can be -80.

        if (percentage < 1)
            return -80;

        return 0.4f * (percentage - 100);
    }
}
