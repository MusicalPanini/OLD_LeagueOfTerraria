using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Whisper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whisper");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Fire 4 shots before having to reload" +
                "\nThe 4th shot will deal 2x damage and crit";
        }

        public override void SetDefaults()
        {
            item.damage = 140;
            item.ranged = true;
            item.width = 44;
            item.height = 20;
            item.useAnimation = 80;
            item.useTime = 80;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true; 
            item.knockBack = 4;
            item.value = 100000;
            item.rare = ItemRarityID.Pink;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 1f;
            item.useAmmo = AmmoID.Bullet;
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new DancingGrenade(this));
            abilityItem.ChampQuote = "Prepare... for your finale";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.WhisperShotsLeft == 0)
                return false;
            return base.CanUseItem(player);
        }

        public override float UseTimeMultiplier(Player player)
        {
            return 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.WhisperShotsLeft == 1)
            {
                type = ProjectileType<Whisper_ForthShot>();
                damage *= 2;
            }
            else
            {
                type = ProjectileType<Whisper_Shot>();
            }
            SetStatsPostShoot(player);
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }

        public void SetStatsPostShoot(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.WhisperShotsLeft--;
            if (modPlayer.WhisperShotsLeft == 0)
                modPlayer.ReloadTimer = 160;
            else
                modPlayer.ReloadTimer = 320;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HarmonicBar>(), 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
