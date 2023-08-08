using Kitchen;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using MessagePack;
using KitchenLib.Utils;
using HarmonyLib;

namespace KitchenFlipUp {

    [HarmonyPatch(typeof(LayoutView), "UpdateData")]
    public class InitializeLayoutBuilder {

        public static void Postfix(LayoutBuilder ___Builder) {
            FlipUpCounterView.BUILDER = ___Builder;
        }
    }

    public class FlipUpCounterView : UpdatableObjectView<FlipUpCounterView.ViewData> {

        public static LayoutBuilder BUILDER;

        [SerializeField]
        public GameObject openGdo;
        [SerializeField]
        public GameObject closedGdo;

        public void Setup(GameObject prefab) {
            openGdo = GameObjectUtils.GetChildObject(prefab, "open");
            closedGdo = GameObjectUtils.GetChildObject(prefab, "closed");
        }

        protected override void UpdateData(ViewData viewData) {
            openGdo.SetActive(viewData.open);
            closedGdo.SetActive(!viewData.open);

            if (BUILDER != null) {
                foreach(var door in BUILDER.Doors) {
                    if (viewData.position == door.Tile1.ToWorld() || viewData.position == door.Tile2.ToWorld()) {
                        door.HatchGameObject.SetActive(false);
                    }
                }
            }
        }

        public class UpdateView : IncrementalViewSystemBase<VariableProviderView.ViewData> {
            private EntityQuery viewsQuery;

            protected override void Initialise() {
                base.Initialise();
                viewsQuery = GetEntityQuery(new QueryHelper()
                    .All(typeof(CLinkedView), typeof(CFlipUpCounterState), typeof(CPosition)));
            }

            protected override void OnUpdate() {
                var entities = viewsQuery.ToEntityArray(Allocator.TempJob);
                var views = viewsQuery.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                var components = viewsQuery.ToComponentDataArray<CFlipUpCounterState>(Allocator.Temp);
                var positions = viewsQuery.ToComponentDataArray<CPosition>(Allocator.Temp);

                for (var i = 0; i < views.Length; i++) {
                    var entity = entities[i];
                    var view = views[i];
                    var data = components[i];
                    var position = positions[i];

                    SendUpdate(view, new ViewData { open = data.open, position = position.Position }, MessageType.SpecificViewUpdate);

                    if (data.open) {
                        EntityManager.AddComponent<CIsInactive>(entity);
                    } else {
                        EntityManager.RemoveComponent<CIsInactive>(entity);
                    }
                }

                entities.Dispose();
                views.Dispose();
                components.Dispose();
                positions.Dispose();
            }
        }

        [MessagePackObject(false)]
        public struct ViewData : ISpecificViewData, IViewData, IViewResponseData, IViewData.ICheckForChanges<ViewData> {

            [Key(1)]
            public bool open;
            [Key(2)]
            public Vector3 position;

            public bool IsChangedFrom(ViewData check) {
                FlipUpMod.Log($"checking if changed. open != check.open = {open} != {check.open} = {open != check.open}; {position} != {check.position} = {position != check.position}");
                return open != check.open && position != check.position;
            }

            public IUpdatableObject GetRelevantSubview(IObjectView view) {
                return view.GetSubView<FlipUpCounterView>();
            }
        }
    }
}
