﻿using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class StarfireSpellblades : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Spellblade");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Gains attack speed and damage each second in combat" +
                "\nAfter 6 seconds, the sword will ascend and fire waves of starfire" +
                "\nThe wave deals " + item.damage + " + [c/" + TerraLeague.MELColor + ":" + (int)(Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MEL * 0.2) + "] + [c/" + TerraLeague.SUMColor + ":" + (int)(Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().SUM * 0.40) + "] ranged damage";
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
                    "When the shield wears off, 7 celestial swords will fall from the sky and explode on the ground" +
                "\nRecast to leave the enemy";
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
                int target = TerraLeague.PlayerMouseIsHovering();
                if (target != -1)
                {
                    if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                    {
                        Vector2 position = Main.player[target].position;
                        Vector2 velocity = Vector2.Zero;
                        int projType = ProjectileType<DivineJudgement>();
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
            item.damage = 45;
            item.width = 56;
            item.height = 56;       
            item.melee = true;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 200000;
            item.rare = 8;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            
            item.shoot = ProjectileID.AmberBolt;
            item.shootSpeed = 8;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.shootSpeed = 8;
            if (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks >= 6)
            {
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 13f);
                int projType = ProjectileType<TrueIceVolley>();
                damage = item.damage + (int)(player.GetModPlayer<PLAYERGLOBAL>().MEL * 0.2) + (int)(player.GetModPlayer<PLAYERGLOBAL>().SUM * 0.40);
                int numberProjectiles = 15;
                float startingAngle = 24;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(startingAngle));
                    startingAngle -= 4f;
                    Projectile proj = Projectile.NewProjectileDirect(position, perturbedSpeed, type, damage, knockBack, player.whoAmI);
                    proj.penetrate = 3;
                    proj.ranged = true;
                    proj.magic = false;
                    proj.timeLeft = 60;
                    proj.tileCollide = false;
                    proj.extraUpdates = 1;
                }


                
                
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {

        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult = 1 + (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks * 0.2f);

            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks >= 6)
            {

                Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, 87,0,0,100,default(Color), 0.7f);
                dust.noGravity = true;
            }
            else
            {

            }

            base.MeleeEffects(player, hitbox);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks >= 6)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/Weapons/StarfireSpellbladesAscended");
            }
            else
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/Weapons/StarfireSpellblades");

            }

            base.UpdateInventory(player);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
           Main.itemTexture[item.type] = mod.GetTexture("Items/Weapons/StarfireSpellblades");

            base.Update(ref gravity, ref maxFallSpeed);
        }

        public override float MeleeSpeedMultiplier(Player player)
        {
            return base.MeleeSpeedMultiplier(player) + (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks * 0.05f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 2);
            recipe.AddIngredient(ItemID.SoulofLight, 20);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemType<CelestialBar>(), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }
    }
}
