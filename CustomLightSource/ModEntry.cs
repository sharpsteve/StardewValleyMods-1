﻿using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;

namespace CustomLightSource
{
    /// <summary>The mod entry point.</summary>
    public partial class ModEntry : Mod, IAssetLoader
    {

        public static IMonitor SMonitor;
        public static IModHelper SHelper;
        public static ModConfig Config;

        public static ModEntry context;

        public static readonly string dictPath = "custom_light_source_dictionary";
        public static readonly string lightPath = "custom_light_source_textures";
        public static Dictionary<string, LightData> lightDataDict = new Dictionary<string, LightData>();
        public static List<string> lightTextureList = new List<string>();

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();

            if (!Config.EnableMod)
                return;

            context = this;

            SMonitor = Monitor;
            SHelper = helper;

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;

            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.Patch(
               original: AccessTools.Method(typeof(Object), nameof(Object.initializeLightSource)),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.Object_initializeLightSource_Prefix))
            );
            harmony.Patch(
               original: AccessTools.Method(typeof(LightSource), "loadTextureFromConstantValue"),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.LightSource_loadTextureFromConstantValue_Prefix))
            );
        }


        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            SHelper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
        }

        private void GameLoop_UpdateTicked(object sender, StardewModdingAPI.Events.UpdateTickedEventArgs e)
        {
            lightDataDict = SHelper.Content.Load<Dictionary<string, LightData>>(dictPath, ContentSource.GameContent) ?? new Dictionary<string, LightData>();
            lightTextureList.Clear();
            foreach (var kvp in lightDataDict)
            {
                if (kvp.Value.texturePath != null && kvp.Value.texturePath.Length > 0)
                {
                    lightTextureList.Add(kvp.Value.texturePath);
                    kvp.Value.textureIndex = 8 + lightTextureList.Count;
                }
            }
            SMonitor.Log($"Loaded {lightDataDict.Count} custom light sources");
            SHelper.Events.GameLoop.UpdateTicked -= GameLoop_UpdateTicked;
        }

        /// <summary>Get whether this instance can load the initial version of the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanLoad<T>(IAssetInfo asset)
        {
            if (!Config.EnableMod)
                return false;

            return asset.AssetNameEquals(dictPath);
        }

        /// <summary>Load a matched asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public T Load<T>(IAssetInfo asset)
        {
            Monitor.Log("Loading dictionary");

            return (T)(object)new Dictionary<string, LightData>();
        }
    }
}