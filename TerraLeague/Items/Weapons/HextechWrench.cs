using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class HextechWrench : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Wrench");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "Create a H-28G Evolution Turret to fight for you!";
        }

        public override string GetQuote()
        {
            return "Stand back! I am about to do...science!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Hextech Micro-Rockets";
            else if (type == AbilityType.E)
                return "CH-2 Electron Storm Grenade";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/MicroRockets";
            else if (type == AbilityType.E)
                return "AbilityImages/StormGrenade";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Fire 5 rockets in a cone towards your cursor";
            }
            else if (type == AbilityType.E)
            {
                return "Throw a granade that stuns hit enemies";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)(0.6 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rocketDamageLastStep * item.damage);
            else if (type == AbilityType.E)
                return (int)(1.5 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.W)
            {
                if (dam == DamageType.SUM)
                    return 25;
            }
            else if (type == AbilityType.E)
            {
                if (dam == DamageType.SUM)
                    return 60;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 20;
            else if (type == AbilityType.E)
                return 20;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " summon damage per rocket";
            else if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " summon damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 8;
            else if (type == AbilityType.E)
                return 10;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    DoEfx(player, type);
                    int projType = ProjectileType<HextechWrench_MicroRocket>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                    int knockback = 1;

                    int numberProjectiles = 5;
                    int distance = 24;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 relPosition = new Vector2(0 - (distance * 2) + (i * distance), 0).RotatedBy(TerraLeague.CalcAngle(player.Center, Main.MouseWorld) + MathHelper.PiOver2);
                        Vector2 position = new Vector2(player.MountedCenter.X + relPosition.X, player.MountedCenter.Y + relPosition.Y);
                        Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15f);

                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    }
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 16f);
                    int projType = ProjectileType<HextechWrench_StormGrenade>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                    int knockback = 0;

                    
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    DoEfx(player, type);
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
            item.damage = 8;
            item.sentry = true;
            item.summon = true;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 1000;
            item.rare = ItemRarityID.Blue;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 113);
            item.shoot = ProjectileType<HextechWrench_EvolutionTurret>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.FindSentryRestingSpot(item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile.NewProjectile((float)xPos, (float)(yPos - yDis) - 24, 0f, 0f, type, damage, knockBack, player.whoAmI, 10, -1);
            player.UpdateMaxTurrets();

            return false;
        }

        public override bool UseItem(Player player)
        {
            return base.UseItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
            else if (type == AbilityType.E)
                Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }
    }
}