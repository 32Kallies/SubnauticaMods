using System.Collections.Generic;
using BepInEx;

namespace RobinPlayerModel.Audio;

/* EXAMPLE FORMAT:
 * This comes from Excel, and is saved as a .csv, so the format isn't very intuitive
 * 
 * Display name,FMOD Path,Sound files,…
 * Cough,"event:/player/coughing		",,
 * Infection terminal 1,"event:/player/gunterminal_first_use		",,
 */
public sealed class SoundReplacementData
{
    public List<Entry> Entries { get; }
    
    public SoundReplacementData(string csvFileData)
    {
        Entries = new List<Entry>();
        int l = 0;
        foreach (var line in csvFileData.Split('\n'))
        {
            if (line.IsNullOrWhiteSpace())
            {
                l++;
                continue;
            }
            
            // skip header
            if (l == 0)
            {
                l++;
                continue;
            }
            
            // read the main lines
            var values = line.Split(',');
            
            // Get sound clips
            string[] soundClips = new string[values.Length - 2];
            for (int i = 0; i < soundClips.Length; i++)
            {
                soundClips[i] = values[i + 2].Trim();
            }
            
            // Create entry
            Entries.Add(new Entry
            {
                DisplayName = values[0].Trim(),
                FmodPath = values[1].Trim(),
                SoundClipNames = soundClips
            });
            
            l++;
        }

        if (Entries.Count == 0)
        {
            Plugin.Logger.LogWarning("No sound replacements sound");
        }
    }

    public readonly struct Entry
    {
        public required string DisplayName { get; init; }
        public required string FmodPath { get; init; }
        // A given entry can be null or empty
        public required string[] SoundClipNames { get; init; }
    }
}