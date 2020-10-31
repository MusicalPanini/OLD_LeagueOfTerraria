﻿using Microsoft.Xna.Framework;
using TerraLeague.Items.Ammo;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class MagicalPlumage : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Plumage");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Uses Razor Feathers as ammo";
        }

        public override string GetQuote()
        {
            return "Feathers fly!";
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.width = 32;
            item.height = 32;
            item.ranged = true;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 1;
            item.value = 54000;
            item.rare = ItemRarityID.Orange;
            item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 20f;
            item.shoot = ProjectileID.PurificationPowder;
            item.useAmmo = ItemType<RazorFeather>();
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;

            Abilities[(int)AbilityType.W] = new DeadlyPlumage(this);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ManaBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
