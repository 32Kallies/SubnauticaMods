using System.IO;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.Utility;

namespace ModStructureHelperPlugin.UI.Menus;

public class StructureMainMenu : StructureHelperMenuBase
{
    public void OnButtonStopEditing()
    {
        if (StructureInstance.Main)
        {
            ui.promptHandler.Ask("Stop editing the current structure? All unsaved changes will be lost.",
                new PromptChoice("Yes", StopEditingStructure), new PromptChoice("No"));
        }
        else
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
        }
    }

    private void StopEditingStructure()
    {
        if (StructureInstance.Main)
        {
            Destroy(StructureInstance.Main.gameObject);
        }
    }

    public void OnButtonSaveChanges()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }

        StructureInstance.TrySave();
    }

    public void ViewCurrentStructureFolder()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }
        
        FileExplorerUtils.OpenFolderInExplorer(Path.GetDirectoryName(StructureInstance.Main.StructureFilePath));
    }

    public void OnButtonInstantiateCurrentStructure()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }

        ui.promptHandler.Ask(
            "Would you like to repair the current structure? ONLY use this if the area it spawns at is FULLY LOADED. Otherwise, you risk corruption",
            new PromptChoice("Yes", InstantiateCurrentStructure), new PromptChoice("No"));
    }

    private void InstantiateCurrentStructure()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }

        UWE.CoroutineHost.StartCoroutine(EntityUtility.SpawnEntitiesFromStructure(StructureInstance.Main.Data));
    }

    public void OnButtonGoToStructureCenter()
    {
        if (!StructureInstance.Main)
        {
            ui.promptHandler.Ask("There is no structure being edited currently!");
            return;
        }

        if (StructureInstance.Main.TryGetStructureCenterPosition(out var centerPosition))
        {
            Player.main.SetPosition(centerPosition);
        }
        else
        {
            ErrorMessage.AddMessage("Cannot teleport to the center of an empty structure!");
        }
    }
}