using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Utils;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
internal class sub
{
    public class TackShooter004 : ModTower
    {
        protected override int Order => 1;
        public override TowerSet TowerSet => TowerSet.Primary;
        public override string BaseTower => "TackShooter-004";
        public override int Cost => 0;
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;

        public override string Name => "Overdrive 2.0";
        public override bool DontAddToShop => true;
        public override string Description => "Shoots very fast";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.isSubTower = true;
            towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("TackShooter-004").portrait;
            towerModel.AddBehavior(Game.instance.model.GetTowerFromId("Marine").GetBehavior<TowerExpireModel>().Duplicate());
            towerModel.GetBehavior<TowerExpireModel>().lifespan = 25;
            towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 32, 0, 360, null, false);
            var tracker = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate<TrackTargetModel>();
            tracker.distance = 999;
            tracker.constantlyAquireNewTarget = true;
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(tracker);
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 5.8f;
            towerModel.range *= 2f;
            towerModel.GetAttackModel().range *= 2f;

        }
    }
}