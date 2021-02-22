using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Gravitum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gravitum");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Uses 5% Gravitum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nYour attacks mark and slow your target";
        }

        public override void SetDefaults()
        {
            item.damage = 100;
            item.magic = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 66;
            item.height = 38;
            item.useAnimation = 30;
            item.useTime = 30;
            item.shootSpeed = 12f;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 310000 * 5;
            item.rare = ItemRarityID.Purple;
            item.shoot = ProjectileType<Gravitum_Orb>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 111);
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new BindingEclipse(this));
            abilityItem.SetAbility(AbilityType.W, new Phase(this, LunariGunType.Grv));
            abilityItem.ChampQuote = "Darkness will weigh upon them";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo < 5)
            {
                if (Main.mouseLeftRelease)
                {
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                    CombatText.NewText(player.Hitbox, new Color(200, 37, 255), "NO AMMO");
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo -= 5;
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX - 20, speedY - 20)) * 20;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, -1);
            return false;
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
            return new Vector2(-20, -10);
        }
    }
}
