using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
    public class RadiantStaff : ModItem
    {
        int shielding = 20;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Staff");
            Tooltip.SetDefault("");
            Item.staff[item.type] = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            int tt2 = tooltips.FindIndex(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt2 != -1)
            {
                tooltips.Insert(tt2 + 1, new TooltipLine(TerraLeague.instance, "Shielding", TerraLeague.CreateScalingTooltip(DamageType.NONE, modPlayer.ScaleValueWithHealPower(shielding * (float)modPlayer.magicDamageLastStep, true), 100, true) + " magic shielding"));
            }
        }

        string GetWeaponTooltip()
        {
            string magic = TerraLeague.CreateScalingTooltip(DamageType.MAG, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MAG, 20);
            return "Send out a returning refraction of your staff, shielding allies and damaging enemies" +
                "\nHas a chance to apply 'Illuminated' to enemies" +
                "\n'Illuminated' enemies take 40 + " + magic + " magic On Hit damage from Radiant Staff";
        }

        public override void SetDefaults()
        {
            item.damage = 35;
            item.mana = 18;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.useTime = 34;
            item.useAnimation = 34;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 55000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = new LegacySoundStyle(2, 8, Terraria.Audio.SoundType.Sound);
            item.shoot = ProjectileType<RadiantStaff_PrismaticBarrier>();
            item.shootSpeed = 12f;
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new LucentSingularity(this));
            abilityItem.SetAbility(AbilityType.R, new FinalSpark(this));
            abilityItem.ChampQuote = "Shine with me";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] == 0;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(shielding * (float)player.GetModPlayer<PLAYERGLOBAL>().magicDamageLastStep, true));
            return false;
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
