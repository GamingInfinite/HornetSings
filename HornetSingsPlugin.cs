using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Needleforge;
using Needleforge.Data;
using UnityEngine;

namespace HornetSings;

[BepInAutoPlugin(id: "io.github.gaminginfinite.hornetsings")]
public partial class HornetSingsPlugin : BaseUnityPlugin
{
    public static ManualLogSource logSource;
    public static AssetBundle modBundle;
    public static ToolData shermaBell;
    public static GameObject Anim;
    public FsmTemplate _trackedNeedolin;
    public static Harmony harmony;
    
    private void Awake()
    {
        // Put your initialization logic here
        logSource = Logger;
        harmony = new("voidbaroness.hornetshermasong");

        modBundle = ModHelper.LoadBundleFromAssembly("HornetSings.Resources.AssetBundles.hornetshermasing");

        Anim = modBundle.LoadAsset<GameObject>("assets/hornetshermasong/hornetshermaanim.prefab");

        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
        harmony.PatchAll();

        foreach(var asset in modBundle.GetAllAssetNames())
        {
            ModHelper.Log(asset);
        }

        Texture2D bellTex = ModHelper.LoadTexFromAssembly("HornetSings.Resources.Images.hornetBell.png");
        Sprite bellSprite = Sprite.Create(bellTex, new(0,0,bellTex.width, bellTex.height), new(0.5f, 0.5f), 420f);
        shermaBell = NeedleforgePlugin.AddTool("Sherma's Bell", ToolItemType.Yellow, 
            new() {Key = "ShermaBellTool", Sheet = $"Mods.{Id}"}, 
            new() {Key = "ShermaBellToolDesc", Sheet = $"Mods.{Id}"}, 
            bellSprite);
    }
}