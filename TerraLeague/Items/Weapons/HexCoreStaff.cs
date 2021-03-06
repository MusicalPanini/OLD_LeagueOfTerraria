using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class HexCoreStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex Core Staff");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Control a Chaos Storm";
        }

        public override void SetDefaults()
        {
            item.damage = 48;
            item.width = 48;
            item.height = 48;
            item.magic = true;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 0;
            item.value = 100000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = new LegacySoundStyle(2, 82, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 8f;
            item.shoot = ProjectileType<HexCoreStaff_ChaosStorm>();
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = false;
            item.mana = 40;
            item.channel = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new GravityField(this));
            abilityItem.ChampQuote = "Join the glorious evolution";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<HexCoreStaff_ChaosStorm>()] < 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PerfectHexCore>());
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
