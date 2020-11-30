using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Severum : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Severum");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Uses 2% Severum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\n+2 melee life steal while attacking";
        }

        public override string GetQuote()
        {
            return "Harvest death for life";
        }

        public override void SetDefaults()
        {
            item.damage = 115;
            item.melee = true;
            item.channel = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 48;
            item.height = 48;
            item.useAnimation = 14;
            item.useTime = 14;
            item.shootSpeed = 80;
            item.knockBack = 2;
            item.value = 310000 * 5;
            item.rare = ItemRarityID.Purple;
            item.scale = 0.7f;
            item.shoot = ProjectileType<Severum_Slash>();
            item.UseSound = null;
            item.autoReuse = true;
            item.noMelee = true;

            Abilities[(int)AbilityType.Q] = new Onslaught(this);
            Abilities[(int)AbilityType.W] = new Phase(this, LunariGunType.Sev);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().severumAmmo < 2)
            {
                if (Main.mouseLeftRelease)
                {
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                    CombatText.NewText(player.Hitbox, new Color(216, 0, 32), "NO AMMO");
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
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

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            else if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 10);
        }
    }
}
