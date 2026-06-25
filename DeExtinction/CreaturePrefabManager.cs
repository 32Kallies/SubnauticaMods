using DeExtinction.Prefabs.Creatures;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using UnityEngine;

namespace DeExtinction;

internal static class CreaturePrefabManager
{
    public static FiltorbPrefab Filtorb { get; private set; }
    public static FiltorbPrefab FloralFiltorb { get; private set; }
    public static AxetailPrefab Axetail { get; private set; }
    public static JellySpinnerPrefab JellySpinner { get; private set; }
    public static RibbonRayPrefab RibbonRay { get; private set; }
    public static TriangleFishPrefab TriangleFish { get; private set; }
    public static ClownPincherPrefab RubyClownPincher { get; private set; }
    public static ClownPincherPrefab SapphireClownPincher { get; private set; }
    public static ClownPincherPrefab EmeraldClownPincher { get; private set; }
    public static ClownPincherPrefab AmberClownPincher { get; private set; }
    public static ClownPincherPrefab CitrineClownPincher { get; private set; }
    public static ThalassaceanPrefab StellarThalassacean { get; private set; }
    public static ThalassaceanPrefab JasperThalassacean { get; private set; }
    public static ThalassaceanPrefab SnowThalassacean { get; private set; }
    public static GrandGliderPrefab GrandGlider { get; private set; }
    public static TwisteelPrefab Twisteel { get; private set; }
    public static TwisteelPrefab TwisteelJuvenile { get; private set; }
    public static GulperLeviathanPrefab GulperPrefab { get; private set; }
    public static GulperLeviathanPrefab GulperJuvenilePrefab { get; private set; }
    public static GulperLeviathanBabyPrefab GulperBabyPrefab { get; private set; }
    public static DragonflyPrefab Dragonfly { get; private set; }
    public static PyrambassisPrefab Pyrambassis { get; private set; }
    
    public static PrefabInfo GrandGliderEgg { get; private set; }
    public static PrefabInfo StellarThalassaceanEgg { get; private set; }
    public static PrefabInfo JasperThalassaceanEgg { get; private set; }
    public static PrefabInfo TwisteelEgg { get; private set; }
    public static PrefabInfo GulperLeviathanEgg { get; private set; }
    public static PrefabInfo PyrambassisEgg { get; private set; }

    private static AssetBundle Bundle => Plugin.AssetBundle;

    private static Sprite LoadIcon(string name) => Bundle.LoadAsset<Sprite>(name);

    public static void RegisterCreatures()
    {
        Filtorb = new FiltorbPrefab(PrefabInfo.WithTechType("Filtorb")
            .WithIcon(LoadIcon("Filtorb_Item")),
            Plugin.AssetBundle.LoadAsset<GameObject>("Filtorb_Prefab"));
        Filtorb.Register();
        
        FloralFiltorb = new FiltorbPrefab(PrefabInfo.WithTechType("FloralFiltorb")
                .WithIcon(LoadIcon("FloralFiltorb_Item")),
            Plugin.AssetBundle.LoadAsset<GameObject>("FloralFiltorb_Prefab"));
        FloralFiltorb.Register();
        
        Axetail = new AxetailPrefab(PrefabInfo.WithTechType("Axetail")
            .WithIcon(LoadIcon("Axetail_Item")));
        Axetail.Register();
        
        JellySpinner = new JellySpinnerPrefab(PrefabInfo.WithTechType("JellySpinner")
            .WithIcon(LoadIcon("JellySpinner_Item")));
        JellySpinner.Register();
        
        RibbonRay = new RibbonRayPrefab(PrefabInfo.WithTechType("RibbonRay")
            .WithIcon(LoadIcon("RibbonRay_Item"))
            .WithSizeInInventory(new Vector2int(2, 1)));
        RibbonRay.Register();
        
        TriangleFish = new TriangleFishPrefab(PrefabInfo.WithTechType("TriangleFish")
            .WithIcon(LoadIcon("Trianglefish_Item")));
        TriangleFish.Register();
        
        RubyClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("RubyClownPincher")
                .WithIcon(LoadIcon("RCP_Item")),
            () => Bundle.LoadAsset<GameObject>("RCP_Prefab"),
            "RCP_Ency",
            "RCP_Popup");
        RubyClownPincher.Register();

        SapphireClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("SapphireClownPincher")
            .WithIcon(LoadIcon("SCP_Item")),
            () => Bundle.LoadAsset<GameObject>("SCP_Prefab"),
            "SCP_Ency",
            "SCP_Popup");
        SapphireClownPincher.Register();
        
        EmeraldClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("EmeraldClownPincher")
                .WithIcon(LoadIcon("ECP_Item")),
            () => Bundle.LoadAsset<GameObject>("ECP_Prefab"),
            "ECP_Ency",
            "ECP_Popup");
        EmeraldClownPincher.Register();

        AmberClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("AmberClownPincher")
                .WithIcon(LoadIcon("ACP_Item")),
            () => Bundle.LoadAsset<GameObject>("ACP_Prefab"),
            "ACP_Ency",
            "ACP_Popup");
        AmberClownPincher.Register();

        CitrineClownPincher = new ClownPincherPrefab(PrefabInfo.WithTechType("CitrineClownPincher")
                .WithIcon(LoadIcon("CCP_Item")),
            () => Bundle.LoadAsset<GameObject>("CCP_Prefab"),
            "CCP_Ency",
            "CCP_Popup"
            );
        CitrineClownPincher.Register();
        
        StellarThalassacean = new ThalassaceanPrefab(PrefabInfo.WithTechType("StellarThalassacean")
            .WithIcon(LoadIcon("Stellar_Item"))
            .WithSizeInInventory(new Vector2int(4, 4)),
            () => Plugin.AssetBundle.LoadAsset<GameObject>("StellarThalassaceanPrefab"),
            "Stellar_Ency",
            "Stellar_Popup");
        StellarThalassaceanEgg = CreateEggPrefab("StellarThalassaceanEgg", "StellarThalassaceanEggPrefab",
            "StellarThalassaceanEgg_Icon", new Vector2int(3, 3), StellarThalassacean.PrefabInfo.TechType, 2);
        StellarThalassacean.EggInfo = StellarThalassaceanEgg;
        StellarThalassacean.Register();
        
        JasperThalassacean = new ThalassaceanPrefab(PrefabInfo.WithTechType("JasperThalassacean")
                .WithIcon(LoadIcon("Jasper_Item"))
                .WithSizeInInventory(new Vector2int(4, 4)),
            () => Plugin.AssetBundle.LoadAsset<GameObject>("JasperThalassaceanPrefab"),
            "Jasper_Ency",
            "Jasper_Popup");
        JasperThalassaceanEgg = CreateEggPrefab("JasperThalassaceanEgg", "JasperThalassaceanEggPrefab",
            "JasperThalassaceanEgg_Icon", new Vector2int(3, 3), JasperThalassacean.PrefabInfo.TechType, 2, true);
        JasperThalassacean.EggInfo = StellarThalassaceanEgg;
        JasperThalassacean.Register();
        CreatureDataUtils.SetAcidImmune(JasperThalassacean.PrefabInfo.TechType);

        SnowThalassacean = new ThalassaceanPrefab(PrefabInfo.WithTechType("SnowThalassacean")
                //.WithIcon(LoadIcon("Jasper_Item"))
                .WithSizeInInventory(new Vector2int(4, 4)),
            () => Plugin.AssetBundle.LoadAsset<GameObject>("SnowThalassaceanPrefab"),
            "Jasper_Ency", // MUST CHANGEMUST CHANGEMUST CHANGEMUST CHANGEMUST CHANGEMUST CHANGE
            "Jasper_Popup",
            true); // MUST CHANGE MUST CHANGEMUST CHANGEMUST CHANGEMUST CHANGEMUST CHANGE
        //JasperThalassaceanEgg = CreateEggPrefab("JasperThalassaceanEgg", "JasperThalassaceanEggPrefab",
        //    "JasperThalassaceanEgg_Icon", new Vector2int(3, 3), JasperThalassacean.PrefabInfo.TechType, 2, true);
        //JasperThalassacean.EggInfo = StellarThalassaceanEgg;
        SnowThalassacean.Register();

        GrandGlider = new GrandGliderPrefab(PrefabInfo.WithTechType("GrandGlider")
            .WithIcon(LoadIcon("GrandGlider_Item"))
            .WithSizeInInventory(new Vector2int(3, 3)));
        GrandGliderEgg = CreateEggPrefab("GrandGliderEgg", "GGEggPrefab",
            "GGEgg_Item", new Vector2int(2, 2), GrandGlider.PrefabInfo.TechType, 1);
        GrandGlider.EggInfo = GrandGliderEgg;
        GrandGlider.Register();

        Twisteel = new TwisteelPrefab(PrefabInfo.WithTechType("Twisteel")
            .WithIcon(LoadIcon("Twisteel_Item"))
            .WithSizeInInventory(new Vector2int(3, 3)),
            Plugin.AssetBundle.LoadAsset<GameObject>("Twisteel_Prefab"),
            false
            );
        TwisteelEgg = CreateEggPrefab("TwisteelEgg", "TwisteelEgg_Prefab",
            "TwisteelEgg_Item", new Vector2int(2, 2), Twisteel.PrefabInfo.TechType, 1.5f);
        Twisteel.EggInfo = TwisteelEgg;
        Twisteel.Register();
        
        TwisteelJuvenile = new TwisteelPrefab(PrefabInfo.WithTechType("TwisteelJuvenile")
                .WithIcon(LoadIcon("Twisteel_Item"))
                .WithSizeInInventory(new Vector2int(2, 2)),
            Plugin.AssetBundle.LoadAsset<GameObject>("TwisteelJuvenile_Prefab"),
            true
        );
        TwisteelJuvenile.Register();
        
        GulperPrefab = new GulperLeviathanPrefab(PrefabInfo.WithTechType("GulperLeviathan"),
            () => Plugin.AssetBundle.LoadAsset<GameObject>("Gulper_Prefab"));
        GulperPrefab.Register();
        
        GulperJuvenilePrefab = new GulperLeviathanPrefab(PrefabInfo.WithTechType("GulperLeviathanJuvenile"),
            () => Plugin.AssetBundle.LoadAsset<GameObject>("GulperJuvenile_Prefab"));
        GulperJuvenilePrefab.Register();
        
        GulperBabyPrefab = new GulperLeviathanBabyPrefab(PrefabInfo.WithTechType("GulperLeviathanBaby")
            .WithIcon(LoadIcon("GulperBaby_Sprite"))
            .WithSizeInInventory(new Vector2int(4, 4)));
        GulperLeviathanEgg = CreateEggPrefab("GulperEgg", "GulperEgg_Prefab",
            "GulperEgg_Sprite", new Vector2int(3, 3), GulperBabyPrefab.PrefabInfo.TechType, 2);
        GulperBabyPrefab.EggInfo = GulperLeviathanEgg;
        GulperBabyPrefab.Register();

        Dragonfly = new DragonflyPrefab(PrefabInfo.WithTechType("Dragonfly"));
        Dragonfly.Register();

        Pyrambassis = new PyrambassisPrefab(PrefabInfo.WithTechType("Pyrambassis")
                .WithIcon(LoadIcon("PyrambassisIcon"))
                .WithSizeInInventory(new Vector2int(4, 4))
        );
        PyrambassisEgg = CreateEggPrefab("PyrambassisEgg", "PyrambassisEggPrefab",
            "PyrambassisEggIcon", new Vector2int(3, 3), Pyrambassis.PrefabInfo.TechType, 2f);
        Pyrambassis.EggInfo = PyrambassisEgg;
        Pyrambassis.Register();

    }

    public static void RegisterFood()
    {
        CookedCreatureHandler.RegisterAllCreatureFood(
            Filtorb,
            null,
            null,
            LoadIcon("Filtorb_Cooked"),
            null,
            null,
            LoadIcon("Filtorb_Cured"),
            new EdibleData(5, 20, true),
            new VFXFabricatingData("CraftModel", -0.3f, 1f, new Vector3(0, 0.14f, 0f), 10, new Vector3(270, 90, 0))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            FloralFiltorb,
            null,
            null,
            LoadIcon("Filtorb_Cooked"),
            null,
            null,
            LoadIcon("Filtorb_Cured"),
            new EdibleData(26, 18, true),
            new VFXFabricatingData("CraftModel", -0.3f, 1f, new Vector3(0, 0.14f, 0f), 10, new Vector3(270, 90, 0))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            Axetail,
            null,
            null,
            LoadIcon("Axetail_Cooked"),
            null,
            null,
            LoadIcon("Axetail_Cured"),
            new EdibleData(20, 13, true),
            new VFXFabricatingData("CraftModel", -0.2f, 0.3f, new Vector3(0, 0.03f, 0f), 30, new Vector3(270, 90, 0))
        );

        CookedCreatureHandler.RegisterAllCreatureFood(
            JellySpinner,
            null,
            null,
            LoadIcon("JellySpinner_Cooked"),
            null,
            null,
            LoadIcon("JellySpinner_Cured"),
            new EdibleData(9, 2, true),
            new VFXFabricatingData("CraftModel", -0.2f, 0.3f, new Vector3(0, 0.06f, 0f), 1, new Vector3(0, 270, 180))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            RibbonRay,
            null,
            null,
            LoadIcon("RibbonRay_Cooked"),
            null,
            null,
            LoadIcon("RibbonRay_Cured"),
            new EdibleData(36, 7, true),
            new VFXFabricatingData("CraftModel", -0.2f, 0.3f, new Vector3(0.1f, 0.03f, 0f), 30, new Vector3(270, 90, 0))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            TriangleFish,
            null,
            null,
            LoadIcon("Trianglefish_Cooked"),
            null,
            null,
            LoadIcon("Trianglefish_Cured"),
            new EdibleData(22, 3, true),
            new VFXFabricatingData("CraftModel", -0.2f, 0.3f, new Vector3(0, 0.03f, 0f), 30, new Vector3(0, 0, 90))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            RubyClownPincher,
            null,
            null,
            LoadIcon("RCP_Cooked"),
            null,
            null,
            LoadIcon("RCP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", -0.2f, 0.3f, new Vector3(0, 0.03f, 0f), 30, new Vector3(270, 90, 0))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            SapphireClownPincher,
            null,
            null,
            LoadIcon("SCP_Cooked"),
            null,
            null,
            LoadIcon("SCP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", -0.2f, 0.3f, new Vector3(0, 0.03f, 0f), 30, new Vector3(270, 90, 0))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            EmeraldClownPincher,
            null,
            null,
            LoadIcon("ECP_Cooked"),
            null,
            null,
            LoadIcon("ECP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", -0.2f, 0.3f, new Vector3(0, 0.03f, 0f), 30, new Vector3(270, 90, 0))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            AmberClownPincher,
            null,
            null,
            LoadIcon("ACP_Cooked"),
            null,
            null,
            LoadIcon("ACP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f, new Vector3(0, 0.03f, 0f), 30, new Vector3(270, 90, 0))
        );
        
        CookedCreatureHandler.RegisterAllCreatureFood(
            CitrineClownPincher,
            null,
            null,
            LoadIcon("CCP_Cooked"),
            null,
            null,
            LoadIcon("CCP_Cured"),
            new EdibleData(30, 9, true),
            new VFXFabricatingData("CraftModel", 0, 0.3f, new Vector3(0, 0.03f, 0f), 30, new Vector3(270, 90, 0))
        );
    }
    
    private static PrefabInfo CreateEggPrefab(string classId, string eggModelName, string eggIconName, Vector2int size, TechType hatchingCreature, float hatchingTime, bool acidImmune = false)
    {
        var info = PrefabInfo.WithTechType(classId)
            .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>(eggIconName))
            .WithSizeInInventory(size);
        var eggPrefab = new CustomPrefab(info);
        var eggTemplate = new EggTemplate(info, Plugin.AssetBundle.LoadAsset<GameObject>(eggModelName))
            .WithHatchingCreature(hatchingCreature)
            .WithCellLevel(LargeWorldEntity.CellLevel.Medium)
            .WithHatchingTime(hatchingTime)
            .SetUndiscoveredTechType();
        // wrote this part when I was very tired, not too concerned about it anyway
        if (classId == "TwisteelEgg")
        {
            eggTemplate.ModifyPrefab += go =>
            {
                var renderer = go.GetComponentInChildren<Renderer>();
                var eggMaterial = renderer.material;
                eggMaterial.color = new Color(1, 1,1, 2);
                eggMaterial.SetFloat("_SpecInt", 5);
                eggMaterial.SetFloat("_Shininess", 7);
                eggMaterial.SetFloat("_Fresnel", 0);
                eggMaterial.SetFloat("_MyCullVariable", 0);
                eggMaterial.EnableKeyword("MARMO_EMISSION");
                eggMaterial.SetFloat("_GlowStrength", 0);
                eggMaterial.SetFloat("_GlowStrengthNight", 0);
                eggMaterial.SetFloat("_EmissionLMNight", 0.01f);
            };
        }
        else if (classId == "PyrambassisEgg")
        {
            eggTemplate.ModifyPrefab += go =>
            {
                foreach (var renderer in go.GetComponentsInChildren<Renderer>())
                {
                    var material = renderer.material;
                    material.SetFloat("_SpecInt", 15);
                    material.SetFloat("_Shininess", 7);
                    material.SetFloat("_Fresnel", 0.5f);
                    material.SetFloat("_MyCullVariable", 0);

                }
            };
        }
        else if (classId == "GulperEgg")
        {
            eggTemplate.ModifyPrefab += go =>
            {
                foreach (var renderer in go.GetComponentsInChildren<Renderer>())
                {
                    var material = renderer.material;
                    if (material.name.Contains("GulperEggB_Transparent"))
                    {
                        material.SetFloat("_MyCullVariable", 0);
                    }
                }
            };
        }

        if (acidImmune)
        {
            CreatureDataUtils.SetAcidImmune(info.TechType);
        }
        
        WorldEntityDatabaseHandler.AddCustomInfo(info.ClassID, info.TechType, Vector3.one, false, LargeWorldEntity.CellLevel.Medium);
        eggPrefab.SetGameObject(eggTemplate);
        eggPrefab.Register();
        return info;
    }
}
