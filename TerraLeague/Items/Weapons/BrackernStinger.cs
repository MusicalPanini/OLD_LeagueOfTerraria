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
    public class BrackernStinger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brackern Stinger");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 9;
            item.width = 32;
            item.height = 32;
            item.melee = true;
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.UseSound = new LegacySoundStyle(2, 101);
            item.shootSpeed = 1f;
            item.shoot = ProjectileType<BrackernStinger_Whip>();
            item.noMelee = true;
            item.noUseGraphic = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new CrystallineExoskeleton(this));
            abilityItem.ChampQuote = "Feel my sting!";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<BrackernStinger_Whip>()] < 1)
                return base.CanUseItem(player);
            return false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0,
                Main.rand.Next(-100, 100) * 0.001f * player.gravDir);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AntlionMandible, 6);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 10);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
