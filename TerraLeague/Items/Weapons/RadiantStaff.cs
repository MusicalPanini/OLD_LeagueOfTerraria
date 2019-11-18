using Microsoft.Xna.Framework;
using System.Linq;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
	public class RadiantStaff : AbilityItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Staff");
            Tooltip.SetDefault("");
            Item.staff[item.type] = false;
        }

        public override string GetWeaponTooltip()
        {
            return "Shots have a chance to apply 'Illuminated'" +
                "\nDeal 40 + [c/" + TerraLeague.MAGColor + ":" + (int)(Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MAG * 0.2) + "] magic On Hit damage to 'Illuminated' enemies";
        }

        public override string GetQuote()
        {
            return "Shine with me";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Lucent Singularity";
            else if (type == AbilityType.R)
                return "Final Spark";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/LucentSingularity";
            else if (type == AbilityType.R)
                return "AbilityImages/FinalSpark";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Cast a ball of light that creates a slowing zone and detonates after a delay, 'Illuminating' hit enemies." +
                    "\nRecast to detonate early";
            }
            else if (type == AbilityType.R)
            {
                return "Channel for 1 second and fire a large beam of pure light" +
                    "\nDeals double damage to 'Illuminated' enemies";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return (int)(item.damage * 2);
            else if (type == AbilityType.R)
                return (int)(item.damage * 3);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.E)
            {
                if (dam == DamageType.MAG)
                    return 60;
            }
            else if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return 100;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 40;
            else if (type == AbilityType.R)
                return 100;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E || type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W || CurrentlyHasSpecialCast(Main.LocalPlayer, type))
                return true;
            else
                return false;
        }

        public override bool CanBeCastWhileCCd(AbilityType type)
        {
            if (CurrentlyHasSpecialCast(Main.LocalPlayer, type))
                return true;
            else
                return false;
        }

        public override bool CurrentlyHasSpecialCast(Player player, AbilityType type)
        {
            if (type == AbilityType.E && Main.LocalPlayer.ownedProjectileCounts[ProjectileType<LucentSingularity>()] > 0)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 13;
            else if (type == AbilityType.R)
                return 40;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CurrentlyHasSpecialCast(Main.LocalPlayer, type))
                {
                    Projectile proj = Main.projectile.Where(x => x.type == ProjectileType<LucentSingularity>() && x.owner == player.whoAmI).FirstOrDefault();
                    if (proj.width != 8)
                        proj.timeLeft = 2;
                }
                else if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                    int projType = ProjectileType<LucentSingularity>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 1);

                    int projType = ProjectileType<FinalSpark>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 3;
                    player.AddBuff(BuffType<Buffs.FinalSparkChannel>(), 60);

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
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
            item.damage = 35;
            item.mana = 4;
            item.useStyle = 5;
            item.magic = true;
            item.useTime = 34;
            item.useAnimation = 34;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 55000;
            item.rare = 5;
            item.UseSound = new LegacySoundStyle(2, 8, Terraria.Audio.SoundType.Sound);
            item.shoot = ProjectileType<LightShot>();
            item.shootSpeed = 10f;
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.useTime = 34;
            item.useAnimation = 34;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
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

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E || type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                Main.PlaySound(new LegacySoundStyle(2, 8, Terraria.Audio.SoundType.Sound), player.Center);
        }
    }
}
