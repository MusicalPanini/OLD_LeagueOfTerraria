using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
	public class CelestialStaff : AbilityItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Staff");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 14;
            item.mana = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.useTime = 35;
            item.useAnimation = 35;
            item.noMelee = true; 
            item.knockBack = 0;
            item.value = 4000;
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item8;
            item.shoot = ProjectileType<CelestialStaff_CelestialHeal>();
            item.shootSpeed = 12f;
            item.autoReuse = true;

            Abilities[(int)AbilityType.Q] = new Starcall(this);
            Abilities[(int)AbilityType.R] = new Wish(this);
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "Stars, hear me";
        }

        public override bool CanUseItem(Player player)
        {
            if (player.statLife <= player.statLifeMax2 / 20)
            {
                return false;
            }
            else
            {
                item.UseSound = SoundID.Item8;
                return true;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            TooltipLine tt2 = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt2 != null)
            {
                tt2.text = System.Math.Round(Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(item.damage, true) * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().magicDamageLastStep) + " magic healing";
            }

            tooltips.FirstOrDefault(x => x.Name == "Knockback" && x.mod == "Terraria").text = "";
            tooltips.FirstOrDefault(x => x.Name == "UseMana" && x.mod == "Terraria").text += " and 5% of max life";
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.statLife -= player.statLifeMax2 / 20;
            damage = player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(damage);
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemType<CelestialBar>(), 10);
            recipe2.AddIngredient(ItemID.PurificationPowder, 5);
            recipe2.AddIngredient(ItemID.FallenStar, 5);
            recipe2.AddIngredient(ItemID.Topaz, 1);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}
