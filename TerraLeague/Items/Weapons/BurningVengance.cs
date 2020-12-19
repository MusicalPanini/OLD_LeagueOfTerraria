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
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class BurningVengance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burning Vengance");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 23;
            item.noMelee = true;
            item.magic = true;
            item.useTime = 5;
            item.useAnimation = 10;
            item.shootSpeed = 14;
            item.mana = 6;
            item.rare = ItemRarityID.Pink;
            item.value = 300000;
            item.width = 28;
            item.height = 32;
            item.knockBack = 1;
            item.UseSound = new LegacySoundStyle(2, 34);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ProjectileType<BurningVengance_Flame>();
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new PillarOfFlame(this));
            abilityItem.SetAbility(AbilityType.R, new Pyroclasm(this));
            abilityItem.ChampQuote = "The inferno begins";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.LivingFireBlock, 20);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
