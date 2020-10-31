using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkinScythe : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Scythe");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Attacks will mark enemies";
        }

        public override string GetQuote()
        {
            return "From the shadow comes the slaughter";
        }

        public override void SetDefaults()
        {
            item.damage = 32;
            item.width = 60;
            item.height = 54;       
            item.melee = true;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3;
            item.value = 100000;
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;

            Abilities[(int)AbilityType.Q] = new ReapingSlash(this);
            Abilities[(int)AbilityType.R] = new Abilities.UmbralTrespass(this);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Buffs.UmbralTrespass>(), 300);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sickle, 1);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 20);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
