using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Projectiles;
using TerraLeague.Buffs;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class LastBreath : AbilityItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Last Breath");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "No cure for fools";
        }

        public override void SetDefaults()
        {
            item.damage = 22;
            item.melee = true;
            item.width = 58;
            item.height = 56;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.value = 48000;
            item.rare = ItemRarityID.Orange;
            item.UseSound = SoundID.Item1;
            item.crit = 25;
            item.autoReuse = true;
            item.shootSpeed = 8f;

            Abilities[(int)AbilityType.Q] = new SteelTempest(this);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Katana, 1);
            recipe.AddIngredient(ItemID.AnkletoftheWind, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
