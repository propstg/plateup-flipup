using Kitchen;

namespace KitchenFlipUp {

    public class FlipUpPreferences {

        public static readonly Pref IncludeInUpgrades = new Pref(FlipUpMod.MOD_ID, nameof(IncludeInUpgrades));

        public static void register() {
            Preferences.AddPreference<bool>(new BoolPreference(IncludeInUpgrades, true));
            Preferences.Load();
        }

        public static bool isIncludeInUpgrades => Preferences.Get<bool>(IncludeInUpgrades);
        public static void setIncludeInUpgrades(bool value) => Preferences.Set<bool>(IncludeInUpgrades, value);
    }
}