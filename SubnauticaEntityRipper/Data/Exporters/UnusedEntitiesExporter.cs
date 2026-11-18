using System.Collections.Generic;
using System.IO;
using System.Linq;
using SubnauticaEntityRipper.Data.Implementation;
using SubnauticaEntityRipper.Data.Interfaces;
using UWE;

namespace SubnauticaEntityRipper.Data.Exporters;

public class UnusedEntitiesExporter : IEntityExporter
{
    public void ExportData(IBatchParser parser, IEnumerable<BatchData> inputBatches, string outputFile)
    {
        HashSet<string> used = new();

        foreach (var input in inputBatches)
        {
            parser.SetCurrentBatch(input);
            
            foreach (var cell in parser.ReadCells())
            {
                foreach (var entity in cell.ReadEntities())
                {
                    if (string.IsNullOrEmpty(entity.CoreData.ClassId))
                    {
                        // Plugin.Logger.LogError($"Entity '{entity.CoreData.UniqueId}' has no Class ID!");
                        continue;
                    }

                    used.Add(entity.CoreData.ClassId);
                }
            }
        }

        using var writer = new StreamWriter(outputFile);
        
        foreach (var file in PrefabDatabase.prefabFiles)
        {
            if (!used.Contains(file.Key))
            {
                var friendlyName = file.Value.Split('/').Last().Split('.').First();
                writer.WriteLine(friendlyName.PadRight(60) + " - " + file.Key);
            }
        }
    }

    private class CountedEntity
    {
        public string ClassId { get; }
        public int Count { get; private set; }

        public int AddOne() => Count++;

        public CountedEntity(string classId)
        {
            ClassId = classId;
            Count = 0;
        }
    }
}