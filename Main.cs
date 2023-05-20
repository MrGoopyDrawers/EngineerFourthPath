using MelonLoader;
using BTD_Mod_Helper;
using EngineerFourthPath;
using PathsPlusPlus;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using JetBrains.Annotations;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using UnityEngine;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using System.Runtime.CompilerServices;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using static sub;
namespace EngineerFourthPathMain;

public class EngineerFourthPathMain : BloonsTD6Mod
{
    public class EngineerBuffIcon : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => VanillaSprites.SuperRangeTacksUpgradeIcon;
        public override int MaxStackSize => 3;
    }
    public class EngineerBuffIcon2 : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => VanillaSprites.SprocketsUpgradeIcon;
        public override int MaxStackSize => 3;
    }
    public class EngineerBuffIcon3 : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => VanillaSprites.TackSprayerUpgradeIcon;
        public override int MaxStackSize => 3;
    }
    public class EngineerBuffIcon4 : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => VanillaSprites.MoreTacksUpgradeIcon;
        public override int MaxStackSize => 3;
    }
    public class FourthPath2 : PathPlusPlus
    {
        public override string Tower => TowerType.EngineerMonkey;
        public override int UpgradeCount => 5;


    }
    public class HotNails : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 450;
        public override int Tier => 1;
        public override string Icon => VanillaSprites.HotShotsUpgradeIcon;

        public override string Description => "Nails penetrate through lead bloons. Does double damage.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage *= 2;
        }
    }
    public class DoubleNail : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 550;
        public override int Tier => 2;
        public override string Icon => VanillaSprites.MoreTacksUpgradeIcon;

        public override string Description => "Twice the nails for twice the power";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 2, 0, 20, null, false);
        }
    }
    public class HardShots : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 2000;
        public override int Tier => 3;
        public override string Icon => VanillaSprites.RazorSharpShotsUpgradeIcon;

        public override string Description => "Extra range for tack shooters. The Engineer's nails become more durable, having double pierce.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackmodel = towerModel.GetAttackModel();
           
            attackmodel.weapons[0].projectile.pierce *= 2;
            attackmodel.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Ceramic", "Ceramic",
                    1, 3, false, false));
            towerModel.AddBehavior(new RangeSupportModel("EngineerRange", true, 0.3f, 0f, "EngineerRange", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.TackShooter }))
                }), false, "EngineerPierce_", null));
            towerModel.GetBehavior<RangeSupportModel>().ApplyBuffIcon<EngineerBuffIcon>();
        }
    }
    public class MechanicalEngineering : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 6000;
        public override int Tier => 4;
        public override string Icon => VanillaSprites.OverdriveUpgradeIcon;

        public override string Description => "Tack Shooters have more pierce and damage. The Engineer's nails shoot out more nails on expiration.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackmodel = towerModel.GetAttackModel();
            var proj = Game.instance.model.GetTowerFromId("EngineerMonkey-032").GetAttackModel().weapons[0].Duplicate();
            attackmodel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", proj.projectile, new ArcEmissionModel("ArcEmissionModel_", 16, 0.0f, 360.0f, null, false), true, false, false));
            towerModel.AddBehavior(new PierceSupportModel("EngineerPierce", true, 5f, "EngineerPierce", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.TackShooter }))
                }), false, "EngineerPierce", null));
            towerModel.AddBehavior(new DamageSupportModel("EngineerDamage", true, 5f, "EngineerDamage", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.TackShooter }))
                }), false, false, 60f));
            towerModel.GetBehavior<PierceSupportModel>().ApplyBuffIcon<EngineerBuffIcon2>();
            towerModel.GetBehavior<DamageSupportModel>().ApplyBuffIcon<EngineerBuffIcon3>();
        }
    }
    public class MachineMaker : UpgradePlusPlus<FourthPath2>
    {
        public override int Cost => 40000;
        public override int Tier => 5;
        public override string Icon => VanillaSprites.BionicBoomerangUpgradeIcon;

        public override string Description => "ALL Tack shooters gain +100% attack rate and more damage to BADS. Engineer has seeking nails and ability to create his own tack shooters if not already building sentries.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackmodel = towerModel.GetAttackModel();
            var proj = Game.instance.model.GetTowerFromId("EngineerMonkey-032").GetAttackModel().weapons[0].Duplicate();
            towerModel.AddBehavior(new RateSupportModel("EngineerAttackSpeed",0.5f,true,"EngineerAttackSpeed",true,1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.TackShooter }))
                }), "EngineerAttackSpeed_",null));
            towerModel.GetBehavior<RateSupportModel>().ApplyBuffIcon<EngineerBuffIcon4>();
            towerModel.AddBehavior(new DamageModifierSupportModel("EngineerMOABIncrease", true, "EngineerMOABincrease", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.TackShooter }))
                }), true, new DamageModifierForTagModel("Bad", "Bad", 1f, 15f, false, true)));
            var tracker = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate<TrackTargetModel>();
            tracker.distance = 999;
            tracker.constantlyAquireNewTarget = true;
            attackmodel.weapons[0].projectile.AddBehavior(tracker);
            attackmodel.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 1.8f;

            AttackModel[] Avatarspawner = { Game.instance.model.GetTowerFromId("EngineerMonkey-200").GetAttackModels().First(a => a.name == "AttackModel_Spawner_").Duplicate() };
            Avatarspawner[0].weapons[0].rate = 15;
            Avatarspawner[0].weapons[0].projectile.RemoveBehavior<CreateTowerModel>();
            Avatarspawner[0].weapons[0].projectile.AddBehavior(new CreateTowerModel("CreateTower", GetTowerModel<TackShooter004>(), 0, true, false, false, true, false));
            Avatarspawner[0].weapons[0].projectile.display = new() { guidRef = "" };
            towerModel.AddBehavior(Avatarspawner[0]);
            towerModel.GetBehavior<DamageSupportModel>().increase += 10f;
        }
    }
}