using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenFlipUp {

    public class FlipUpMenu<T> : KLMenu<T> {

        private static readonly List<bool> boolValues = new List<bool> { false, true };
        private static readonly List<string> boolLabels = new List<string> { "Off", "On" };

        public FlipUpMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int _) {
            Option<bool> option = new Option<bool>(boolValues, FlipUpPreferences.isIncludeInUpgrades, boolLabels);
            AddLabel("Include in upgrades for counter (will need to restart game)");
            AddSelect(option);
            option.OnChanged += delegate (object __, bool value) {
                FlipUpPreferences.setIncludeInUpgrades(value);
            };
            New<SpacerElement>();
            New<SpacerElement>();
            AddButton(Localisation["MENU_BACK_SETTINGS"], delegate { RequestPreviousMenu(); });
        }
    }
}