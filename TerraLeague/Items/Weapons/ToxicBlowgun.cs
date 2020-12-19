using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ToxicBlowgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Blowgun");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Blowgun);
            item.damage = 24;
            item.ranged = true;
            item.width = 24;
            item.height = 54;
            item.useAnimation = 35;
            item.useTime = 35;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 200000;
            item.rare = ItemRarityID.Lime;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurificationPowder;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new ToxicShot(this));
            abilityItem.SetAbility(AbilityType.R, new NoxiousTrap(this));
            abilityItem.ChampQuote = "Never underestimate the power of the Scout's code";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = new Vector2(position.X, position.Y - 6);

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Blowgun, 1);
            recipe.AddIngredient(ItemID.VialofVenom, 10);
            recipe.AddIngredient(ItemID.Mushroom, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, -10);
        }
    }
}
