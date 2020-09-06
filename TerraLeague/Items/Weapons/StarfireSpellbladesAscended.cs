﻿using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class StarfireSpellbladesAscended : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Spellblade");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "You have ascended" +
                "\nFire a wave of starfire that deals " + (int)(item.damage * 0.75) + " + [c/" + TerraLeague.MELColor + ":" + (int)(Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MEL * 0.3) + "] + [c/" + TerraLeague.SUMColor + ":" + (int)(Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().SUM * 0.50) + "] ranged damage";
        }

        public override string GetQuote()
        {
            return "As evil grows, so shall I";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Divine Judgement";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/DivineJudgement";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Grant an ally or yourself invulnerability for 2 seconds." +
                    "\nWhen the shield wears off, 7 celestial swords will fall from the sky and explode on the ground";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return item.damage * 2;
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.R)
            {
                if (dam == DamageType.MEL)
                    return 100;
                else if (dam == DamageType.SUM)
                    return 80;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 150;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MEL) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " summon damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 120;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                int target = TerraLeague.PlayerMouseIsHovering(30, -1, player.team);
                if (target != -1)
                {
                    if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                    {
                        Vector2 position = Main.player[target].position;
                        Vector2 velocity = Vector2.Zero;
                        int projType = ProjectileType<StarfireSpellblades_DivineJudgement>();
                        int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MEL) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                        int knockback = 10;

                        DoEfx(Main.player[target], type);
                        Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, target);
                        SetCooldowns(player, type);
                    }
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), player.Center);
            }

            base.Efx(player, type);
        }

        public override void SetDefaults()
        {
            item.damage = 110;
            item.width = 56;
            item.height = 56;       
            item.melee = true;
            item.useTime = 36;
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = 200000;
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<StarfireSpellblades_Firewave>();
            item.shootSpeed = 8;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 16f);
            damage = (int)(item.damage * 0.75) + (int)(player.GetModPlayer<PLAYERGLOBAL>().MEL * 0.3) + (int)(player.GetModPlayer<PLAYERGLOBAL>().SUM * 0.5);
            int numberProjectiles = 24;
            float startingAngle = 24;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(startingAngle));
                startingAngle -= 2f;
                Projectile proj = Projectile.NewProjectileDirect(position, perturbedSpeed, type, damage, knockBack, player.whoAmI, i);
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 60);
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, 87, 0, 0, 100, default(Color), 0.7f);
            dust.noGravity = true;
            base.MeleeEffects(player, hitbox);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks != 6)
            {
                byte prefix = item.prefix;
                item.SetDefaults(ModContent.ItemType<StarfireSpellblades>());
                item.prefix = prefix;
            }

            base.UpdateInventory(player);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            byte prefix = item.prefix;
            item.SetDefaults(ModContent.ItemType<StarfireSpellblades>());
            item.prefix = prefix;

            base.Update(ref gravity, ref maxFallSpeed);
        }

        public override void AddRecipes()
        {
           
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }
    }
}
