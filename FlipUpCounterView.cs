using Kitchen;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using MessagePack;
using KitchenLib.Utils;

namespace KitchenFlipUp {

    public class FlipUpCounterView : UpdatableObjectView<FlipUpCounterView.ViewData> {

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
        }

        public class UpdateView : IncrementalViewSystemBase<VariableProviderView.ViewData> {
            private EntityQuery viewsQuery;

            protected override void Initialise() {
                base.Initialise();
                viewsQuery = GetEntityQuery(new QueryHelper()
                    .All(typeof(CLinkedView), typeof(CFlipUpCounterState)));
            }

            protected override void OnUpdate() {
                var entities = viewsQuery.ToEntityArray(Allocator.TempJob);
                var views = viewsQuery.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                var components = viewsQuery.ToComponentDataArray<CFlipUpCounterState>(Allocator.Temp);

                for (var i = 0; i < views.Length; i++) {
                    var entity = entities[i];
                    var view = views[i];
                    var data = components[i];

                    SendUpdate(view, new ViewData { open = data.open }, MessageType.SpecificViewUpdate);

                    if (data.open) {
                        EntityManager.AddComponent<CIsInactive>(entity);
                    } else {
                        EntityManager.RemoveComponent<CIsInactive>(entity);
                    }
                }

                views.Dispose();
                components.Dispose();
            }
        }

        [MessagePackObject(false)]
        public struct ViewData : ISpecificViewData, IViewData, IViewResponseData, IViewData.ICheckForChanges<ViewData> {

            [Key(1)]
            public bool open;

            public bool IsChangedFrom(ViewData check) {
                FlipUpMod.Log($"checking if changed. open != check.open = {open} != {check.open} = {open != check.open}");
                return open != check.open;
            }

            public IUpdatableObject GetRelevantSubview(IObjectView view) {
                return view.GetSubView<FlipUpCounterView>();
            }
        }
    }
}
