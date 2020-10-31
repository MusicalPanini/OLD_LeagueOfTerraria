using Microsoft.Xna.Framework;
using System.Linq;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class RadiantStaff : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Staff");
            Tooltip.SetDefault("");
            Item.staff[item.type] = false;
        }

        public override string GetWeaponTooltip()
        {
            string magic = TerraLeague.CreateScalingTooltip(DamageType.MAG, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MAG, 20);
            return "Shots have a chance to apply 'Illuminated'" +
                "\nDeal 40 + " + magic + " magic On Hit damage to 'Illuminated' enemies";
        }

        public override string GetQuote()
        {
            return "Shine with me";
        }

        public override void SetDefaults()
        {
            item.damage = 35;
            item.mana = 4;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.useTime = 34;
            item.useAnimation = 34;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 55000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = new LegacySoundStyle(2, 8, Terraria.Audio.SoundType.Sound);
            item.shoot = ProjectileType<RadiantStaff_LightShot>();
            item.shootSpeed = 10f;
            item.autoReuse = true;

            Abilities[(int)AbilityType.E] = new LucentSingularity(this);
            Abilities[(int)AbilityType.R] = new FinalSpark(this);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 20);
            recipe.AddIngredient(ItemID.DiamondGemsparkBlock, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
