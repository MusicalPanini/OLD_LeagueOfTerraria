using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Drakebane : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drakebane");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.width = 64;
            item.height = 64;
            item.melee = true;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 2;
            item.value = 6000;
            item.rare = ItemRarityID.Orange;
            item.UseSound = new LegacySoundStyle(2, 101);
            item.shootSpeed = 1f;
            item.shoot = ProjectileType<Drakebane_Whip>();
            item.noMelee = true;
            item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new DemacianStandard(this));
            abilityItem.ChampQuote = "Righteous retribution!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<Drakebane_Whip>()] < 1)
                return base.CanUseItem(player);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
