using UnityEngine;
using HarmonyLib;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(StoryMomentController))]
    internal class StoryMomentControllerPatch {
        // too hard and got lazy
        // this is supposed to have a custom win thing

        // shareholder(s): "what do YOU want?"
        // shareholder(s): "always breaking in!"
        // player (who won) can input text
        // check text using contain("")

        //// "freedom", "spare", "mercy"
        // shareholder(s): "no"
        // struck by lightning.

        //// "why"
        // shareholder(s): "money"
        // struck by lightning.

        //// "odey" <- just that
        // shareholder(s): "perfect!"
        // become boss

        //// UNKNOWN
        // shareholder(s): dont care!
        // shareholder(s): fired
        // struck by lightning.

        // more
    }
}
