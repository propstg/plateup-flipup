﻿using KitchenLib;
using KitchenMods;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenFlipUp {

    public class FlipUpMod : BaseMod {

        public const string MOD_ID = "blargle.FlipUp";
        public const string MOD_NAME = "FlipUp!";
        public const string MOD_VERSION = "0.0.0";
        public const string MOD_AUTHOR = "blargle";

        public static AssetBundle bundle;

        public FlipUpMod() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, "1.1.4", Assembly.GetExecutingAssembly()) { }

        protected override void OnPostActivate(Mod mod) {
            Log($"v{MOD_VERSION} initialized");
            Log($"{MOD_VERSION} Loading asset bundle...");
            bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            Log($"Asset bundle loaded.");

            AddGameDataObject<FlipUpCounter>();
        }

        public static void Log(object message) {
            Debug.Log($"[{MOD_ID}] {message}");
        }
    }
}