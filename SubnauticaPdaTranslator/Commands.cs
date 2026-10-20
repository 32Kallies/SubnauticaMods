using Nautilus.Commands;

namespace SubnauticaPdaTranslator;

public static class Commands
{
    [ConsoleCommand("exportlanguage")]
    private static void ExportCurrentLanguage()
    {
        new LanguageExporter().ExportAll(Language.main.GetCurrentLanguage() + "-logs");
    }
}