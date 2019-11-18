using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Projectiles;
using TerraLeague.Buffs;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class LastBreath : AbilityItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Last Breath");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "No cure for fools";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().gathering3)
                    return "Gathering Storm";
                else if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().gathering2)
                    return "Steel Wind Rising";
                else
                    return "Steel Tempest";
            }
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().gathering3)
                    return "AbilityImages/GatheringStorm";
                else
                    return "AbilityImages/SteelTempest";
            }
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().gathering3)
                {
                    return "Uses 2 stacks" +
                        "\nLaunch a tornado that knocks up enemies";
                }
                else
                {
                    return "Thrust your sword forward." +
                        "\nGain a stack of 'Gathering Storm' if you damage an enemy" +
                        "\nCreate a tornado at 2 stacks";
                }
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().gathering3)
                    return (int)System.Math.Round(item.damage * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().meleeDamageLastStep * 2);
                else
                    return (int)System.Math.Round(item.damage * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().meleeDamageLastStep);
            }
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MEL)
                {
                    return 100;
                }
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 0;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MEL) + " melee damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 4;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q && player.itemTime == 0)
            {
                if (Main.LocalPlayer.HasBuff(BuffType<LastBreath3>()))
                {
                    if (CheckIfNotOnCooldown(player, type) && player.GetModPlayer<PLAYERGLOBAL>().gathering3)
                    {
                        DoEfx(player, type);
                        Vector2 position = player.MountedCenter;
                        Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                        int projType = ProjectileType<LastBreathNado>();
                        int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MEL);
                        int knockback = 12;
                        player.ClearBuff(BuffType<LastBreath3>());

                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(position, velocity/4, ProjectileType<SteelTempest>(), damage, knockback, player.whoAmI, 0, 1);

                        SetCooldowns(player, type);
                    }
                }
                else
                {
                    if (CheckIfNotOnCooldown(player, type))
                    {
                        DoEfx(player, type);
                        Vector2 position = player.MountedCenter;
                        Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 3f);
                        int projType = ProjectileType<SteelTempest>();
                        int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MEL);
                        int knockback = 5;
                        player.ClearBuff(BuffType<LastBreath3>());

                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                        SetCooldowns(player, type);
                    }
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override void SetDefaults()
        {
            item.damage = 22;
            item.melee = true;
            item.width = 58;
            item.height = 56;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 1;
            item.knockBack = 2;
            item.value = 48000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.crit = 25;
            item.autoReuse = true;
            item.shootSpeed = 8f;
        }

        public override bool CanUseItem(Player player)
        {
            item.useTurn = false;

            return base.CanUseItem(player);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Katana, 1);
            recipe.AddIngredient(ItemID.AnkletoftheWind, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (Main.LocalPlayer.HasBuff(BuffType<LastBreath3>()))
            {
                
            }
            else
            {
               
            }
        }
    }
}
