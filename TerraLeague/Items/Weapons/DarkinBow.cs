using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkinBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Bow");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Charge an arrow that gains damage and velocity the longer you charge, up to 1.5 seconds";
        }


        public override void SetDefaults()
        {
            item.damage = 34;
            item.ranged = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 24;
            item.height = 64;
            item.channel = true;
            item.useTime = 18;
            item.useAnimation = 18;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 100000;
            item.rare = ItemRarityID.LightRed;
            item.shoot = ProjectileType<DarkinBow_ArrowControl>();
            item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new RainOfArrows(this));
            abilityItem.ChampQuote = "The guilty will know agony";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ProjectileType<DarkinBow_DarkinArrow>();

            Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ProjectileType<DarkinBow_ArrowControl>(), damage, knockBack, player.whoAmI, type);
            proj.rotation = new Vector2(speedX, speedY).ToRotation();

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ShadowFlameBow, 1);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 20);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
