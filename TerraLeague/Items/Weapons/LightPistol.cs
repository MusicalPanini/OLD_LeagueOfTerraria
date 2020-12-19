using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class LightPistol : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Pistol");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.ranged = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 52;
            item.height = 26;
            item.useAnimation = 16;
            item.reuseDelay = 20;
            item.useTime = 8;
            item.shootSpeed = 8f;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 6000;
            item.rare = ItemRarityID.Orange;
            item.scale = 0.9f;
            item.shoot = ProjectileType<LightPistol_Bullet>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 12);
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new PiercingLight(this));
            abilityItem.ChampQuote = "Everybody deserves a second shot";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 46f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
