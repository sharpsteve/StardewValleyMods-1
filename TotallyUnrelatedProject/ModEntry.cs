﻿using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using System;

namespace CustomFixedDialogue 
{
    public class ModEntry : Mod
    {

        public override void Entry(IModHelper helper)
        {
            DialoguePatches.Initialize(Monitor, helper);

            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Constructor(typeof(Dialogue), new Type[] { typeof(string), typeof(NPC) }),
                prefix: new HarmonyMethod(typeof(DialoguePatches), nameof(DialoguePatches.Dialogue_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(Dialogue), nameof(Dialogue.convertToDwarvish)),
                prefix: new HarmonyMethod(typeof(DialoguePatches), nameof(DialoguePatches.convertToDwarvish_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(LocalizedContentManager), nameof(LocalizedContentManager.LoadString), new Type[] { typeof(string) }),
                postfix: new HarmonyMethod(typeof(DialoguePatches), nameof(DialoguePatches.LocalizedContentManager_LoadString_Postfix))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), nameof(NPC.showTextAboveHead)),
                prefix: new HarmonyMethod(typeof(DialoguePatches), nameof(DialoguePatches.NPC_showTextAboveHead_Prefix))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), nameof(NPC.getHi)),
                postfix: new HarmonyMethod(typeof(DialoguePatches), nameof(DialoguePatches.NPC_getHi_Postfix))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), nameof(NPC.getTermOfSpousalEndearment)),
                postfix: new HarmonyMethod(typeof(DialoguePatches), nameof(DialoguePatches.NPC_getTermOfSpousalEndearment_Postfix))
            );
            harmony.Patch(
                original: AccessTools.Method(typeof(Summit), nameof(Summit.GetSummitDialogue)),
                postfix: new HarmonyMethod(typeof(DialoguePatches), nameof(DialoguePatches.GetSummitDialogue_Patch))
            );

            //helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;


        }
        
        private void GameLoop_DayStarted(object sender, StardewModdingAPI.Events.DayStartedEventArgs e)
        {
            string text = "This is a test";
            DialoguePatches.AddWrapperToString("Data\\ExtraDialogue:SummitEvent_Dialogue2_Spouse", ref text);
            Monitor.Log($"prefixed: {text}");
            DialoguePatches.FixString(Game1.getCharacterFromName("Shane"), ref text);
            Monitor.Log($"fixed: {text}");
        }
    }
}