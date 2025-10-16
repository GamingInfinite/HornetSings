using HarmonyLib;

namespace HornetShermaSong.Patches
{
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.Start))]
    internal class AddAnims
    {
        [HarmonyPostfix]
        public static void Postfix(HeroController __instance)
        {
            tk2dSpriteAnimator animator = __instance.GetComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation newAnim = HornetShermaSongPlugin.Anim.GetComponent<tk2dSpriteAnimation>();
            animator.Library.clips = [..animator.Library.clips, ..newAnim.clips];
        }
    }
}
