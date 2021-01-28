using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class ArcaneEnergy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            DisplayName.SetDefault("Arcane Energy");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Gains 100% extra damage and range for each second you charge it" +
                    "\nMax of 1000%";
        }

        public override void SetDefaults()
        {
            item.damage = 75;
            item.width = 32;
            item.height = 32;
            item.magic = true;
            item.useAnimation = 32;
            item.useTime = 32;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.knockBack = 5;
            item.value = 40000;
            item.rare = ItemRarityID.LightRed;
            item.shootSpeed = 12f;
            item.shoot = ProjectileType<ArcaneEnergy_PulseControl>();
            item.useTurn = true;
            item.noUseGraphic = true;
            item.mana = 40;
            item.autoReuse = false;
            item.channel = true;
            item.noMelee = true;
            item.UseSound = new LegacySoundStyle(2, 82, Terraria.Audio.SoundType.Sound);

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.E, new ShockingOrb(this));
            abilityItem.SetAbility(AbilityType.R, new RiteOfTheArcane(this));
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.ChampQuote = "Behold my power";
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Sunstone>(), 20);
            recipe.AddIngredient(ItemID.Chain, 4);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
