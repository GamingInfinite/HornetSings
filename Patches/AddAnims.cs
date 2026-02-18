using HarmonyLib;
using HornetSings;

namespace HornetSings.Patches
{
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.Start))]
    internal class AddAnims
    {
        [HarmonyPostfix]
        public static void Postfix(HeroController __instance)
        {
            tk2dSpriteAnimation newAnim = HornetSingsPlugin.Anim.GetComponent<tk2dSpriteAnimation>();
            newAnim.isValid = false;
            newAnim.ValidateLookup();
            
            tk2dSpriteAnimation heroAnim = __instance.AnimCtrl.animator.Library;
            
            heroAnim.clips = [.. heroAnim.clips, .. newAnim.clips];
            heroAnim.isValid = false;
            heroAnim.ValidateLookup();
        }
    }
}
