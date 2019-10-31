using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
	public class CelestialStaff : AbilityItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Staff");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 14;
            item.mana = 25;
            item.useStyle = 5;
            item.magic = true;
            item.useTime = 35;
            item.useAnimation = 35;
            item.noMelee = true; 
            item.knockBack = 0;
            item.value = 4000;
            item.rare = 4;
            item.UseSound = SoundID.Item8;
            item.shoot = ProjectileType<CelestialHeal>();
            item.shootSpeed = 12f;
            item.autoReuse = true;
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "Stars, hear me";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Wish";
            else if (type == AbilityType.Q)
                return "Starcall";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/Wish";
            else if (type == AbilityType.Q)
                return "AbilityImages/Starcall";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Heal all players, wherever they are." +
                    "\nHealing is increased by 50% if target is below 40% life.";
            }
            else if (type == AbilityType.Q)
            {
                return "Call down a star to impact the surface." +
                    "\nGain 'Rejuvenation' for 5 seconds if you damage an enemy.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.R)
                return (int)(item.damage * 3 * modPlayer.healPowerLastStep);
            else if (type == AbilityType.Q)
                return (int)item.damage * 4;
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return (int)(50 * modPlayer.healPowerLastStep);
            }
            else if (type == AbilityType.Q)
            {
                if (dam == DamageType.MAG)
                    return 35;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 200;
            else if (type == AbilityType.Q)
                return 20;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {

            if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " healing";
            else if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.R || type == AbilityType.Q)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 90;
            else if (type == AbilityType.Q)
                return 6;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        if (Main.player[i].active && i != player.whoAmI)
                        {
                            if (player.whoAmI == Main.myPlayer)
                            {
                                PLAYERGLOBAL modPlayerTarget = Main.player[i].GetModPlayer<PLAYERGLOBAL>();

                                if (modPlayerTarget.GetRealHeathWithoutShield(false) < modPlayerTarget.GetRealHeathWithoutShield(true) * 0.4)
                                    modPlayerTarget.SendHealPacket((int)((GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG)) * 1.5), i, -1, player.whoAmI);
                                else
                                    modPlayerTarget.SendHealPacket(GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG), i, -1, player.whoAmI);
                            }
                        }
                        else if (Main.player[i].active)
                        {
                            if (modPlayer.GetRealHeathWithoutShield(false) < modPlayer.GetRealHeathWithoutShield(true) * 0.4)
                                modPlayer.lifeToHeal += (int)((GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG)) * 1.5);
                            else
                                modPlayer.lifeToHeal += GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                        }
                    }

                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.Center;
                    position.Y -= 600;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                    int projType = ProjectileType<Starcall>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);

                    Projectile.NewProjectile(position, velocity, projType, damage, 0, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.statLife <= player.statLifeMax2 / 20)
            {
                return false;
            }
            else
            {
                item.UseSound = SoundID.Item8;
                return true;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            TooltipLine tt2 = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt2 != null)
            {
                tt2.text = System.Math.Round(item.damage * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().magicDamageLastStep) + " magic healing";
            }

            tooltips.FirstOrDefault(x => x.Name == "Knockback" && x.mod == "Terraria").text = "";
            tooltips.FirstOrDefault(x => x.Name == "UseMana" && x.mod == "Terraria").text += " and 5% of max life";
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.statLife -= player.statLifeMax2 / 20;
            damage = (int)(damage * player.GetModPlayer<PLAYERGLOBAL>().healPower);
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-9, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemType<CelestialBar>(), 10);
            recipe2.AddIngredient(ItemID.PurificationPowder, 5);
            recipe2.AddIngredient(ItemID.FallenStar, 5);
            recipe2.AddIngredient(ItemID.Topaz, 1);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R || type == AbilityType.Q)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    if (Main.player[i].active)
                    {
                        for (int j = 0; j < 18; j++)
                        {
                            int num = Dust.NewDust(new Vector2(Main.rand.Next((int)Main.player[i].position.X - 8, (int)Main.player[i].position.X + 8), Main.player[i].position.Y + 16), Main.player[i].width, Main.player[i].height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 0, 0), Main.rand.Next(2, 6));
                            Main.dust[num].noGravity = true;
                        }
                    }
                }

                Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f));
            }
        }
    }
}
