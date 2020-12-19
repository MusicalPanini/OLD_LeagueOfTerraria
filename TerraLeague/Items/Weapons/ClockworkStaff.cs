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
    public class ClockworkStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clockwork Staff");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        string GetWeaponTooltip()
        {
            return "Uses 3 minion slots" +
                "\nCan only have one";
        }

        public override void SetDefaults()
        {
            item.damage = 70;
            item.summon = true;
            item.mana = 20;
            item.width = 48;
            item.height = 48;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 100000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = new LegacySoundStyle(2, 113);
            item.shoot = ProjectileType<ClockworkStaff_TheBall>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new CommandProtect(this));
            abilityItem.ChampQuote = "Time tick-ticks away";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.ownedProjectileCounts[type] > 0)
            {
                Projectile projectile = Main.projectile.FirstOrDefault(x => x.type == ProjectileType<ClockworkStaff_TheBall>() || x.owner == player.whoAmI);

                return false;
            }
            else
            {
                position = Main.MouseWorld;

                if (player.altFunctionUse != 2)
                {
                    return true;
                }
                return false;
            }

        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            else
            {
                player.AddBuff(BuffType<Buffs.TheBall>(), 2);
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PerfectHexCore>());
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}