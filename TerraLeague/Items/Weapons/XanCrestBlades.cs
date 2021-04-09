using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class XanCrestBlades : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xan Crest Blades");

            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Manipulate " + TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "MINIONS", Main.LocalPlayer.maxMinions, 150) + " flowing blades that deal damage based on their speed";
        }

        public override void SetDefaults()
        {
            item.damage = 34;
            item.width = 62;
            item.height = 26;
            item.summon = true;
            item.useTime = 25;
            item.mana = 20;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 0;
            item.value = 350000;
            item.rare = ItemRarityID.Lime;
            item.UseSound = new LegacySoundStyle(2, 82, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 15f;
            item.shoot = ProjectileType<XanCrestBlades_DancingBlade>();
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = false;
            item.noUseGraphic = true;
            item.channel = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new BladeSurge(this));
            abilityItem.ChampQuote = "Cut them down!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<XanCrestBlades_DancingBlade>()] < 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = (int)(player.maxMinions * 1.5) - 1; i >= 0; i--)
            {
                Projectile.NewProjectileDirect(position, Vector2.Zero, ProjectileType<XanCrestBlades_DancingBlade>(), damage, knockBack, player.whoAmI, i).Center = player.Center;
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 16);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
