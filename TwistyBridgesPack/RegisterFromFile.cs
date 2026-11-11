using System;
using System.IO;

namespace TwistyBridgesPack;

public class RegisterFromFile
{
    private readonly string _path;
    private readonly Action<string> _registerEntry;

    public RegisterFromFile(string path, Action<string> registerEntry)
    {
        _path = path;
        _registerEntry = registerEntry;
    }

    public void RegisterAll()
    {
        if (!File.Exists(_path))
        {
            throw new FileNotFoundException($"Could not find file of Twisty Bridges props at path '{_path}'");
        }

        using var reader = new StreamReader(new FileStream(_path, FileMode.Open, FileAccess.Read));

        int found = 0;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;
            found++;
            _registerEntry.Invoke(line);
        }

        if (found == 0) throw new Exception("Twisty Bridges prop pack failed to load; 0 prefabs found");
        
        Plugin.Logger.LogInfo($"Loaded Twisty Bridges prop pack successfully. {found} prefabs registered.");
    }
}