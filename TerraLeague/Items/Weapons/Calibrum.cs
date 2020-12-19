using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Calibrum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Calibrum");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Uses 5% Calibrum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nDeals more damage to far away enemies";
        }

        public override void SetDefaults()
        {
            item.damage = 200;
            item.ranged = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 100;
            item.height = 26;
            item.useAnimation = 35;
            item.useTime = 35;
            item.shootSpeed = 2f;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 310000 * 5;
            item.rare = ItemRarityID.Purple;
            //item.scale = 0.8f;
            item.shoot = ProjectileType<Calibrum_Shot>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 75);
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new Moonshot(this));
            abilityItem.SetAbility(AbilityType.W, new Phase(this, LunariGunType.Cal));
            abilityItem.ChampQuote = "Moonlight will guide your aim";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().calibrumAmmo < 5)
            {
                if (Main.mouseLeftRelease)
                {
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                    CombatText.NewText(player.Hitbox, new Color(141, 252, 245), "NO AMMO");
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.GetModPlayer<PLAYERGLOBAL>().calibrumAmmo -= 5;

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
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 0);
        }
    }
}
