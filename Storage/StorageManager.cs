using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

// Vaguely based off http://codetuto.com/2017/08/save-load-game-godot-engine/
public class StorageManager
{
    private string StoragePath = "res://storage.json";

    public StorageV1 Storage;

    public StorageManager()
    {
        this.LoadOrDefault();

        if (!this.SaveExists())
            this.Save();
    }

    public bool SaveExists()
    {
        var f = new File();

        var exists = f.FileExists(this.StoragePath);

        f.Close();

        return exists;
    }

    public void LoadOrDefault()
    {
        if (this.SaveExists())
        {
            var f = new File();

            f.Open(this.StoragePath, File.ModeFlags.Read);

            var parsed = JsonConvert.DeserializeObject<StorageV1>(f.GetAsText());

            this.Storage = parsed;

            f.Close();
        }
        else
        {
            this.Storage = new StorageV1();
        }
    }

    public void Save()
    {
        if (this.Storage == null)
            throw new Exception("Storage has not been set.");

        var f = new File();

        f.Open(this.StoragePath, File.ModeFlags.Write);

        var json = JsonConvert.SerializeObject(this.Storage);

        f.StoreString(json);

        f.Close();
    }
}
