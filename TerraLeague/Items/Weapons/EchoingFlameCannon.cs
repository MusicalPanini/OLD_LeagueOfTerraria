using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class EchoingFlameCannon : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoing Flame Cannon");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "You are surrounded by 6 markers." +
                "\nShooting towards one will fire a shotgun of cursed flame." +
                "\nEach mark recharges every 7 seconds";
        }

        public override string GetQuote()
        {
            return "You cannot know strength... Until you are broken.";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Corrosive Charge";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/CorrosiveCharge";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Launch a canister of corrosive gas that detonates on impact with the ground";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)(item.damage * 0.6);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.RNG)
                    return 30;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 20;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " ranged damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 10;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 16);
                    int projType = ProjectileType<EchoingFlameCannon_CorrosiveCharge>();
                    int damage = (GetAbilityBaseDamage(player, type)) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int knockback = 0;

                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
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
            item.damage = 52;
            item.ranged = true;
            item.noMelee = true;
            item.width = 40;
            item.height = 32; 
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 5f;
            item.value = 100000;
            item.rare = ItemRarityID.Pink;
            item.shootSpeed = 12f;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 36);
            item.useAmmo = AmmoID.Bullet;
            item.shoot = 10;
            item.autoReuse = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            EchoingFlamesEffect(player, damage, new Vector2(speedX, speedY));
            return true;
        }

        public void EchoingFlamesEffect(Player player, int damage, Vector2 velocity)
        {
            float angle = velocity.ToRotation();
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            int cooldown = 7;
            float[] projectileAngles = null;
            if (angle > -MathHelper.PiOver2 && angle <= -MathHelper.PiOver2 + MathHelper.ToRadians(60))
            {
                if (modPlayer.echoingFlames_RT <= 0)
                {
                    modPlayer.echoingFlames_RT = cooldown * 60;
                    projectileAngles = new float[] { -85, -75, -65, -55, -45, -35 };
                }
            }
            else if (angle > -MathHelper.ToRadians(30) && angle <= MathHelper.ToRadians(30))
            {
                if (modPlayer.echoingFlames_RM <= 0)
                {
                    modPlayer.echoingFlames_RM = cooldown * 60;
                    projectileAngles = new float[] { -25, -15, -5, 5, 15, 25 };
                }
            }
            else if (angle > MathHelper.ToRadians(30) && angle <= MathHelper.PiOver2)
            {
                if (modPlayer.echoingFlames_RB <= 0)
                {
                    modPlayer.echoingFlames_RB = cooldown * 60;
                    projectileAngles = new float[] { 35, 45, 55, 65, 75, 85};
                }
            }
            else if (angle > MathHelper.PiOver2 && angle <= MathHelper.PiOver2 + MathHelper.ToRadians(60))
            {
                if (modPlayer.echoingFlames_LB <= 0)
                {
                    modPlayer.echoingFlames_LB = cooldown * 60;
                    projectileAngles = new float[] { 95, 105, 115, 125, 135, 145 };
                }
            }
            else if ((angle > MathHelper.Pi - MathHelper.ToRadians(30) && angle <= MathHelper.Pi) || angle >= -MathHelper.Pi && angle <= -MathHelper.Pi + MathHelper.ToRadians(30))
            {
                if (modPlayer.echoingFlames_LM <= 0)
                {
                    modPlayer.echoingFlames_LM = cooldown * 60;
                    projectileAngles = new float[] { 155, 165, 175, -175, -165, -155 };
                }
            }
            else if (angle > -MathHelper.Pi + MathHelper.ToRadians(30) && angle <= -MathHelper.PiOver2)
            {
                if (modPlayer.echoingFlames_LT <= 0)
                {
                    modPlayer.echoingFlames_LT = cooldown * 60;
                    projectileAngles = new float[] { -145, -135, -125, -115, -105, -95 };
                }
            }

            if (projectileAngles != null)
            {
                for (int i = 0; i < projectileAngles.Length; i++)
                {
                    Projectile.NewProjectileDirect(player.MountedCenter, new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(projectileAngles[i])), ProjectileType<EchoingFlameCannon_EchoingFlames>(), damage/2, 5, player.whoAmI);
                }
            }
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
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
