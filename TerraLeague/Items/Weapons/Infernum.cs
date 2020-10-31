using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Infernum : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernum");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Uses 5% Infernum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nYour attacks will create a splash of flame";
        }

        public override string GetQuote()
        {
            return "Cosmic flame will fill the night";
        }

        public override void SetDefaults()
        {
            item.damage = 100;
            item.ranged = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 96;
            item.height = 36;
            item.useAnimation = 26;
            item.useTime = 26;
            item.shootSpeed = 12f;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 310000 * 5;
            item.rare = ItemRarityID.Purple;
            item.scale = 0.8f;
            item.shoot = ProjectileType<Infernum_Flame>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 45);
            item.autoReuse = true;

            Abilities[(int)AbilityType.Q] = new Duskwave(this);
            Abilities[(int)AbilityType.W] = new Phase(this, LunariGunType.Inf);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().infernumAmmo < 5)
            {
                if (Main.mouseLeftRelease)
                {
                    CombatText.NewText(player.Hitbox, new Color(0, 148, 255), "NO AMMO");
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.GetModPlayer<PLAYERGLOBAL>().infernumAmmo -= 5;

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY + 3)) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-30, 10);
        }
    }
}
