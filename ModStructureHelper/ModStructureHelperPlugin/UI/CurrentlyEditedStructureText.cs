using System.IO;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.UI;

public class CurrentlyEditedStructureText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
        OnStructureInstanceChanged(StructureInstance.Main);
    }

    private void OnDisable()
    {
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
    }

    private void OnStructureInstanceChanged(StructureInstance newInstance)
    {
        text.text = GetTextForStructure(newInstance);
    }

    private string GetTextForStructure(StructureInstance instance)
    {
        if (instance == null)
            return "None";
        return instance.StructureName;
    }

    public void OnButtonPressed()
    {
        if (StructureInstance.Main != null)
        {
            FileExplorerUtils.OpenFolderInExplorer(Path.GetDirectoryName(StructureInstance.Main.StructureFilePath));
        }
    }
}