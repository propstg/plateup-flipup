using Kitchen;
using Unity.Entities;
using KitchenMods;
using HarmonyLib;

namespace KitchenFlipUp {

    [HarmonyPatch(typeof(LayoutView), "UpdateData")]
    public class InitializeHatchDespawningSystemPatch {

        public static void Postfix(LayoutBuilder ___Builder) {
            HatchDespawningSystem.builder = ___Builder;
        }
    }

    public class HatchDespawningSystem : GameSystemBase, IModSystem {

        public static LayoutBuilder builder;
        
        protected override void OnUpdate() {
            if (builder != null) {
                foreach(var door in builder.Doors) {
                    Entity entity1 = GetOccupant(door.Tile1.ToWorld());
                    Entity entity2 = GetOccupant(door.Tile2.ToWorld());

                    if (HasComponent<CFlipUpCounterState>(entity1) || HasComponent<CFlipUpCounterState>(entity2)) {
                        door.HatchGameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
