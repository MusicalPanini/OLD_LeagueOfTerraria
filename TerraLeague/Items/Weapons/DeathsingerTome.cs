using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class DeathsingerTome : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Death Singer's Tome");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Detonate the targeted area after 0.5 seconds" +
                "\nIf you only hit 1 enemy, the damage is doubled";
        }

        public override string GetQuote()
        {
            return "I am the Nightbringer~";
        }

        public override void SetDefaults()
        {
            item.damage = 28;
            item.noMelee = true;
            item.magic = true;
            item.mana = 10;
            item.rare = ItemRarityID.Orange;
            item.value = 10000;
            item.width = 28;
            item.height = 32;
            item.useTime = 35;
            item.useAnimation = 35;
            item.UseSound = new LegacySoundStyle(2,8);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ProjectileType<DeathsingerTome_LayWaste>();

            Abilities[(int)AbilityType.E] = new Defile(this);
            Abilities[(int)AbilityType.R] = new Abilities.Requiem(this);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectileDirect(Main.MouseWorld, Vector2.Zero, type, damage, 0, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
