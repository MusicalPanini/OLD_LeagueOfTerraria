using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ChemCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chem Crossbow");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        string GetWeaponTooltip()
        {
            return "Fires toxic arrows that apply 'Deadly Venom'" +
                "\n'Deadly Venom' stacks 5 times dealing more damage over time per stack";
        }

        public override void SetDefaults()
        {
            item.damage = 5;
            item.ranged = true;
            item.noMelee = true;
            item.width = 58;
            item.height = 34;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 0f;
            item.value = 1000;
            item.rare = ItemRarityID.Blue;
            item.shootSpeed = 10f;
            item.shoot = ProjectileID.WoodenArrowFriendly;
            item.UseSound = SoundID.Item5;
            item.useAmmo = AmmoID.Arrow;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new ToxicCask(this));
            abilityItem.SetAbility(AbilityType.E, new Contaminate(this));
            abilityItem.ChampQuote = "I dealt it! It was meeee!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ProjectileType<ChemCrossbow_ToxicArrow>();
            return true;
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(ItemID.WoodenArrow, 150);

            base.OnCraft(recipe);
        }
    }
}
