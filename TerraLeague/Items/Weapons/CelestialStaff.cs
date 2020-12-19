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
	public class CelestialStaff : ModItem
	{
        static readonly float baseRejuvChance = 0.1f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Staff");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.mana = 18;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.useTime = 35;
            item.useAnimation = 35;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 4000;
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item8;
            item.shoot = ProjectileType<CelestialStaff_Starcall>();
            item.shootSpeed = 12f;
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new AstralInfusion(this));
            abilityItem.SetAbility(AbilityType.R, new Wish(this));
            abilityItem.ChampQuote = "Stars, hear me";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        string GetWeaponTooltip()
        {
            return "Call down stars that have a chance to drop Rejuvenation Hearts on hit" +
                "\nDrop Chance: " + TerraLeague.CreateScalingTooltip(DamageType.NONE, (int)(baseRejuvChance * 100), 100, true, "%") + " + " + TerraLeague.CreateScalingTooltip(DamageType.MAG, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MAG, 15, true, "%");
        }

        public static float RejuvDropChance(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return modPlayer.ScaleValueWithHealPower((baseRejuvChance + (modPlayer.MAG * 0.15f * 0.01f)) * 100, true) * 0.01f; //(100 - baseRejuvChance) / (100 + modPlayer.ScaleValueWithHealPower(modPlayer.MAG, true));
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center;
            position.X += Main.rand.NextFloat(-300, 300);
            position.Y -= 600;
            Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 14f);
            item.damage = 26;
            Projectile.NewProjectile(position, velocity, type, damage, 0, player.whoAmI);
            return false;
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
