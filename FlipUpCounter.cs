﻿using ApplianceLib.Api;
using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenFlipUp {

    public class FlipUpCounter : CustomAppliance {

        public override GameObject Prefab => FlipUpMod.bundle.LoadAsset<GameObject>("FlipUpCounter");
        public override PriceTier PriceTier => PriceTier.Medium;
        public override ShoppingTags ShoppingTags => ShoppingTags.Cooking;
        public override bool IsNonInteractive => false;
        public override bool IsPurchasable => true;
        public override bool SellOnlyAsDuplicate => false;
        public override string UniqueNameID => "FlipUp - Counter";

        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>() {
            new CItemHolder(),
            new CFlipUpCounterState(),
            new CItemTransferRestrictions {
                AllowWhenInactive = false,
                AllowWhenActive = true,
            },
        };

        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)> {
            (Locale.English, new ApplianceInfo() {
                Name = "FlipUp! Counter",
                Description = "Counter that can be flipped up out of the way, for when you need to have a face-to-face discussion with someone."
            })
        };

        public override List<Appliance.ApplianceProcesses> Processes => new List<Appliance.ApplianceProcesses> {
            new Appliance.ApplianceProcesses() {
                Process = (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
                Speed = 1f,
                IsAutomatic = false
            },
            new Appliance.ApplianceProcesses() {
                Process = (Process)GDOUtils.GetExistingGDO(ProcessReferences.Knead),
                Speed = 1f,
                IsAutomatic = false
            },
        };

        public override List<Appliance> Upgrades => new List<Appliance>() {
            GDOUtils.GetExistingGDO(ApplianceReferences.Countertop) as Appliance
        };

        public override void OnRegister(GameDataObject gameDataObject) {
            setupMaterials();
            setupHoldPoint();
            setupCustomView();
            ApplianceGroups.AddApplianceToGroup(ApplianceGroup.AllCounters, GDOUtils.GetCastedGDO<Appliance, FlipUpCounter>());
        }

        private void setupMaterials() {
            var wood = MaterialUtils.GetExistingMaterial("Wood - Default");
            var woodEdge = MaterialUtils.GetExistingMaterial("Wood - Dark");
            var metal = MaterialUtils.GetExistingMaterial("Metal- Shiny");
            var road = MaterialUtils.GetExistingMaterial("Road");
            var cabinet = MaterialUtils.GetExistingMaterial("Wood 4 - Painted");
            var doorGlass = MaterialUtils.GetExistingMaterial("Door Glass");
            var metalBlack = MaterialUtils.GetExistingMaterial("Metal Black");

            MaterialUtils.ApplyMaterial(Prefab, "base/cabinet", new Material[] { cabinet });
            MaterialUtils.ApplyMaterial(Prefab, "base/counter", new Material[] { wood });
            MaterialUtils.ApplyMaterial(Prefab, "base/lift mechanism", new Material[] { road });
            MaterialUtils.ApplyMaterial(Prefab, "base/rim", new Material[] { woodEdge });
            MaterialUtils.ApplyMaterial(Prefab, "closed/arm", new Material[] { metal });
            MaterialUtils.ApplyMaterial(Prefab, "closed/counter", new Material[] { doorGlass });
            MaterialUtils.ApplyMaterial(Prefab, "closed/rim", new Material[] { metalBlack });
            MaterialUtils.ApplyMaterial(Prefab, "open/arm", new Material[] { metal });
            MaterialUtils.ApplyMaterial(Prefab, "open/counter", new Material[] { doorGlass });
            MaterialUtils.ApplyMaterial(Prefab, "open/rim", new Material[] { metalBlack });
        }

        private void setupHoldPoint() {
            HoldPointContainer container = Prefab.AddComponent<HoldPointContainer>();
            container.HoldPoint = GameObjectUtils.GetChildObject(Prefab, "closed/HoldPoint").transform;
        }

        private void setupCustomView() {
            Prefab.AddComponent<FlipUpCounterView>().Setup(Prefab);
        }
    }
}
