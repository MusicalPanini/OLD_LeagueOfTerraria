using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class TerrorOfTheVoid : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror of the Void");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 55;
            item.width = 48;
            item.height = 48;
            item.magic = true;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.mana = 40;
            item.value = 35000;
            item.rare = ItemRarityID.Lime;
            item.shoot = ProjectileType<TerrorOfTheVoid_RuptureControl>();
            item.shootSpeed = 1f;
            item.UseSound = SoundID.Item8;
            item.noMelee = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new Feast(this));
            abilityItem.ChampQuote = "Your souls will feed the void";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<TerrorOfTheVoid_RuptureControl>()] > 0)
                return false;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (speedX > 0)
                speedX = 3;
            else
                speedX = -3;

            speedY = 0;

            Projectile.NewProjectile(player.Top.X, player.Top.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, player.GetModPlayer<PLAYERGLOBAL>().feast3 ? 1 : 0);

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
