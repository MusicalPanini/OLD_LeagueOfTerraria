using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class TheFallenCelestialsDarkMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Fallen Celestials Dark Magic");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Deals increased damage the lower the enemies life";
        }

        public override void SetDefaults()
        {
            item.damage = 24;
            item.noMelee = true;
            item.magic = true;
            item.useTime = 80;
            item.useAnimation = 80;
            item.mana = 40;
            item.rare = ItemRarityID.Yellow;
            item.value = 300000;
            item.width = 28;
            item.height = 32;
            item.knockBack = 0;
            item.UseSound = SoundID.Item20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ProjectileType<TheFallenCelestialsDarkMagic_TormentedShadow>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new SoulShackles(this));
            abilityItem.ChampQuote = "I am bound, but I will not break";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectileDirect(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
            recipe.AddIngredient(ItemID.Chain, 10);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemType<CelestialBar>(), 20);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
