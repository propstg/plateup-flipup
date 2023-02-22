using Kitchen;
using KitchenData;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenFlipUp {

    [UpdateInGroup(typeof(CreationGroup))]
    public class AddFlipUpDataToApplianceSystem : GenericSystemBase, IModSystem {

        private EntityQuery appliancesQuery;

        protected override void Initialise() {
            base.Initialise();
            appliancesQuery = GetEntityQuery(new QueryHelper().All(typeof(CCreateAppliance)));
        }

        protected override void OnUpdate() {
            var appliances = appliancesQuery.ToEntityArray(Allocator.TempJob);
            foreach (var appliance in appliances) {
                int applianceId = EntityManager.GetComponentData<CCreateAppliance>(appliance).ID;
                if (GameData.Main.TryGet(applianceId, out Appliance gdo)) {
                    foreach (var prop in gdo.Properties) {
                        if (prop is CFlipUpCounterState container) {
                            EntityManager.AddComponentData(appliance, container);
                        }
                    }
                }
            }
            appliances.Dispose();
        }
    }
}
