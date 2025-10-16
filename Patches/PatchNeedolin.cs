using System;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Silksong.FsmUtil;
using Silksong.FsmUtil.Actions;
using UnityEngine;

namespace HornetShermaSong.Patches
{
    [HarmonyPatch(typeof(PlayMakerFSM), nameof(PlayMakerFSM.Start))]
    internal class PatchNeedolin
    {
        [HarmonyPostfix]
        public static void Postfix(PlayMakerFSM __instance)
        {
            if (__instance is { name: "Hero_Hornet(Clone)", FsmName: "Silk Specials" })
            {
                Fsm SilkSpecials = __instance.Fsm;
                Fsm NeedolinFsm = SilkSpecials.GetAction<RunFSM>("Needolin Sub", 2).fsmTemplateControl.RunFsm;

                FsmState startNeedolin = NeedolinFsm.GetState("Start Needolin");
                FsmState startNeedolinProper = NeedolinFsm.GetState("Start Needolin Proper");
                FsmState needolinCancel = NeedolinFsm.GetState("Cancelable");
                FsmState setTime = NeedolinFsm.GetState("Set Silk Drain Time");
                FsmState playNeedolin = NeedolinFsm.GetState("Play Needolin");

                FsmState ShermaBellQ = NeedolinFsm.AddState("Sherma Bell?");

                ShermaBellQ.AddTransition("FINISHED", startNeedolinProper.Name);

                setTime.ChangeTransition("FINISHED", ShermaBellQ.Name);

                AudioClip shermaSong = HornetShermaSongPlugin.modBundle.LoadAsset<AudioClip>("assets/hornetshermasong/hornet_sing.wav");
                AudioClip defaultHornetNeedolin = (AudioClip)startNeedolinProper.GetAction<StartNeedolinAudioLoop>(6).DefaultClip.Value;

                FsmBool AtBench = NeedolinFsm.GetFsmBool("At Bench");

                DelegateAction<Action> cancelNeedolin = new()
                {
                    Method = (action) =>
                    {
                        bool bellEquipped = HornetShermaSongPlugin.shermaBell.IsEquipped;
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
                        bool bellEquipped = HornetShermaSongPlugin.shermaBell.IsEquipped;
                        FsmString needolinClip = NeedolinFsm.GetFsmString("Play Clip");
                        if (AtBench.Value)
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
                        bool bellEquipped = HornetShermaSongPlugin.shermaBell.IsEquipped;
                        FsmString needolinClip = NeedolinFsm.GetFsmString("Play Clip");
                        if (AtBench.Value)
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

                ShermaBellQ.AddLambdaMethod((action) =>
                {
                    bool bellEquipped = HornetShermaSongPlugin.shermaBell.IsEquipped;
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
