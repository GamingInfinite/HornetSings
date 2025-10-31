using System;
using HarmonyLib;
using HornetSings;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Silksong.FsmUtil;
using Silksong.FsmUtil.Actions;
using UnityEngine;

namespace HornetSings.Patches
{
    [HarmonyPatch(typeof(PlayMakerFSM), nameof(PlayMakerFSM.Start))]
    internal class PatchNeedolin
    {
        [HarmonyPostfix]
        public static void Postfix(PlayMakerFSM __instance)
        {
            if (__instance is { name: "Hero_Hornet(Clone)", FsmName: "Silk Specials" })
            {
                Fsm silkSpecials = __instance.Fsm;
                Fsm? needolinFsm = silkSpecials.GetAction<RunFSM>("Needolin Sub", 2)?.fsmTemplateControl.RunFsm;

                FsmState startNeedolin = needolinFsm.GetState("Start Needolin");
                FsmState startNeedolinProper = needolinFsm.GetState("Start Needolin Proper");
                FsmState needolinCancel = needolinFsm.GetState("Cancelable");
                FsmState setTime = needolinFsm.GetState("Set Silk Drain Time");
                FsmState playNeedolin = needolinFsm.GetState("Play Needolin");

                FsmState shermaBellQ = needolinFsm.AddState("Sherma Bell?");

                shermaBellQ.AddTransition("FINISHED", startNeedolinProper.Name);

                setTime.ChangeTransition("FINISHED", shermaBellQ.Name);

                AudioClip shermaSong = HornetSingsPlugin.modBundle.LoadAsset<AudioClip>("assets/hornetshermasong/hornet_sing.wav");
                AudioClip defaultHornetNeedolin = (AudioClip)startNeedolinProper.GetAction<StartNeedolinAudioLoop>(6).DefaultClip.Value;

                FsmBool atBench = needolinFsm.GetFsmBool("At Bench");

                DelegateAction<Action> cancelNeedolin = new()
                {
                    Method = (action) =>
                    {
                        bool bellEquipped = HornetSingsPlugin.shermaBell.IsEquipped;
                        if (bellEquipped)
                        {
                            needolinCancel.GetAction<Tk2dPlayAnimationWithEvents>(2).clipName = "";
                        }
                        action.Invoke();
                    }
                };

                DelegateAction<Action> decideStartAnim = new()
                {
                    Method = (action) =>
                    {
                        bool bellEquipped = HornetSingsPlugin.shermaBell.IsEquipped;
                        FsmString needolinClip = needolinFsm.GetFsmString("Play Clip");
                        if (atBench.Value)
                        {
                            needolinClip.Value = "NeedolinSit Start";
                        }
                        else
                        {
                            if (bellEquipped)
                            {
                                needolinClip.Value = "hornetShermaSingStart";
                            }
                            else
                            {
                                needolinClip.Value = "Needolin Start";
                            }
                        }
                        action.Invoke();
                    }
                };
                decideStartAnim.Arg = decideStartAnim.Finish;

                DelegateAction<Action> decideMainAnim = new()
                {
                    Method = (action) =>
                    {
                        bool bellEquipped = HornetSingsPlugin.shermaBell.IsEquipped;
                        FsmString needolinClip = needolinFsm.GetFsmString("Play Clip");
                        if (atBench.Value)
                        {
                            needolinClip.Value = "NeedolinSit Play";
                        }
                        else
                        {
                            if (bellEquipped)
                            {
                                needolinClip.Value = "hornetShermaSing";
                            }
                            else
                            {
                                needolinClip.Value = "Needolin Play";
                            }
                        }
                        action.Invoke();
                    }
                };
                decideMainAnim.Arg = decideMainAnim.Finish;

                startNeedolin.ReplaceAction(decideStartAnim, 6);
                playNeedolin.ReplaceAction(decideMainAnim, 4);

                shermaBellQ.AddLambdaMethod((action) =>
                {
                    bool bellEquipped = HornetSingsPlugin.shermaBell.IsEquipped;
                    if (bellEquipped)
                    {
                        startNeedolinProper.GetAction<StartNeedolinAudioLoop>(6).DefaultClip.Value = shermaSong;
                    }
                    else
                    {
                        startNeedolinProper.GetAction<StartNeedolinAudioLoop>(6).DefaultClip.Value = defaultHornetNeedolin;
                    }
                    action.Invoke();
                });
            }
        }
    }
}
