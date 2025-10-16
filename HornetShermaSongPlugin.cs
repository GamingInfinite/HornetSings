using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Needleforge;
using UnityEngine.SceneManagement;
using HornetShermaSong.Patches;
using Needleforge.Data;

namespace HornetShermaSong
{
    // TODO - adjust the plugin guid as needed
    [BepInAutoPlugin(id: "voidbaroness.hornetshermasong")]
    public partial class HornetShermaSongPlugin : BaseUnityPlugin
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

            modBundle = ModHelper.LoadBundleFromAssembly("HornetShermaSong.Resources.AssetBundles.hornetshermasing");

            Anim = modBundle.LoadAsset<GameObject>("assets/hornetshermasong/hornetshermaanim.prefab");

            Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
            harmony.PatchAll(typeof(PatchNeedolin));
            harmony.PatchAll(typeof(AddAnims));

            foreach(var asset in modBundle.GetAllAssetNames())
            {
                ModHelper.Log(asset);
            }

            Texture2D bellTex = ModHelper.LoadTexFromAssembly("HornetShermaSong.Resources.Images.hornetBell.png");
            Sprite bellSprite = Sprite.Create(bellTex, new(0,0,bellTex.width, bellTex.height), new(0.5f, 0.5f), 420f);
            shermaBell = NeedleforgePlugin.AddTool(bellSprite, ToolItemType.Yellow, "Sherma's Bell");


            SceneManager.activeSceneChanged += OnSceneChange;
        }

        private void OnSceneChange(Scene prev, Scene next)
        {
            if (next.name != "Menu_Title")
            {
                harmony.PatchAll(typeof(Localization));
            }
        }
    }
}