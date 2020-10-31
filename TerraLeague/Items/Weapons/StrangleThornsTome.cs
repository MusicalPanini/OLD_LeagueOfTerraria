using Microsoft.Xna.Framework;
using System;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class StrangleThornsTome : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strangle Thorn Tome");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Enemies hit with Strangle Thorns have a chance to drop a seed on death." +
                "\nIf Strangle Thorns passes near a bulb it will grow into a Thorn Spitter, attacking near by enemies" +
                "\n(WIP)";
                
        }

        public override string GetQuote()
        {
            return "Feel the thorns embrace";
        }

        public override void SetDefaults()
        {
            item.damage = 30;
            item.width = 56;
            item.height = 56;       
            item.summon = true;
            item.noMelee = true;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 1;
            item.value = 140000;
            item.rare = ItemRarityID.Lime;
            item.UseSound = SoundID.Item8;
            item.mana = 16;
            item.shootSpeed = 32;
            item.shoot = ProjectileType<StrangleThornsTome_StrangleThorns>();

            Abilities[(int)AbilityType.W] = new RampantGrowth(this);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.NettleBurst, 1);
            recipe.AddIngredient(ItemID.Stinger, 12);
            recipe.AddIngredient(ItemID.JungleSpores, 20);
            recipe.AddIngredient(ItemID.JungleRose, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 12);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
