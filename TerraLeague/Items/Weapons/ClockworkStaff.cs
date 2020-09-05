using Microsoft.Xna.Framework;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ClockworkStaff : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clockwork Staff");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "Uses 3 minion slots" +
                "\nCan only have one";
        }

        public override string GetQuote()
        {
            return "Time tick-ticks away";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Command: Protect";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/CommandProtect";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Shield yourself and nearby allies and increase their armor and resist by 15 for 6 seconds";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.E)
                return (int)(item.damage * 0.4 * modPlayer.healPowerLastStep);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.E)
            {
                if (dam == DamageType.SUM)
                    return (int)(60 * modPlayer.healPowerLastStep);
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 75;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " shielding";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 24;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                int shield = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                int duration = 60 * 6;

                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    DoEfx(player, type);
                    player.AddBuff(BuffType<CommandProtect>(), duration);
                    player.GetModPlayer<PLAYERGLOBAL>().AddShield(shield, duration, new Color(102, 243, 255), ShieldType.Basic);

                    // For Server
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                        var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, 300, player.whoAmI, player.team);

                        for (int i = 0; i < players.Count; i++)
                        {
                            modPlayer.SendShieldPacket(shield, players[i], ShieldType.Basic, duration, -1, player.whoAmI, new Color(102, 243, 255));
                            modPlayer.SendBuffPacket(BuffType<CommandProtect>(), duration, players[i], -1, player.whoAmI);
                        }
                    }

                    SetCooldowns(player, type);
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
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

        public override bool UseItem(Player player)
        {

            return base.UseItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            else
            {
                player.AddBuff(BuffType<TheBall>(), 2);
            }
            return base.CanUseItem(player);
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), player.Center);
                TerraLeague.DustBorderRing(300, player.MountedCenter, 226, default(Color), 2);
                TerraLeague.DustRing(226, player, default(Color));
            }
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