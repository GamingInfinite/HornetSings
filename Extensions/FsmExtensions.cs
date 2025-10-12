using System;
using System.Collections.Generic;
using System.Text;
using HutongGames.PlayMaker;

namespace HornetShermaSong.Extensions
{
    internal static class FsmExtensions
    {
        internal static T GetAction<T>(this Fsm fsm, string stateName, int index) where T : FsmStateAction => (T)fsm.GetState(stateName).Actions[index];
        internal static FsmState AddState(this Fsm Fsm, string stateName)
        {
            FsmState newState = new(Fsm) { Name = stateName };
            Fsm.states = [..Fsm.states,  newState];
            return newState;
        }
    }
}
