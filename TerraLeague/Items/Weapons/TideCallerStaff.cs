using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
	public class TideCallerStaff : ModItem
	{
        int healing = 8;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidecaller Staff");
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
                tooltips.Insert(tt2 + 1, new TooltipLine(TerraLeague.instance, "Healing", TerraLeague.CreateScalingTooltip(DamageType.NONE, (int)(healing * modPlayer.magicDamageLastStep), 100, true) + " magic healing"));
            }
        }

        string GetWeaponTooltip()
        {
            return "Hititng a stunned or bubbled enemy will heal a nearby ally" +
                "\nAfter healing, it will then strike another enemy";
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.mana = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.useTime = 36;
            item.useAnimation = 36;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 4000;
            item.rare = ItemRarityID.Green;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 21, Terraria.Audio.SoundType.Sound);
            item.shoot = ProjectileType<TideCallerStaff_EbbandFlow>();
            //item.shoot = ProjectileType<TideCallerStaff_WaterShot>();
            item.shootSpeed = 11f;
            healing = 8;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new AquaPrison(this));
            abilityItem.SetAbility(AbilityType.E, new TidecallersBlessing(this));
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.ChampQuote = "I decide what the tide will bring";
            abilityItem.IsAbilityItem = true;

            base.SetDefaults();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            item.mana = 20;
            Vector2 velocity = new Vector2(speedX, speedY);

            int scaledHealing = modPlayer.ScaleValueWithHealPower(healing * player.magicDamage);

            Projectile.NewProjectileDirect(position, velocity, type, damage, knockBack, player.whoAmI, damage, scaledHealing);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 10);
            recipe.AddIngredient(ItemID.Seashell, 5);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
