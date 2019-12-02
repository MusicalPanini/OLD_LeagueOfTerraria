using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ChemCrossbow : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chem Crossbow");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "Fires poison arrows that apply 'Deadly Venom'" +
                "\n'Deadly Venom' stacks 5 times dealing more damage over time per stack";
        }

        public override string GetQuote()
        {
            return "I dealt it! It was meeee!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Toxic Cask";
            else if (type == AbilityType.E)
                return "Contaminate";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/VenomCask";
            else if (type == AbilityType.E)
                return "AbilityImages/Contaminate";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Throw a cask that releases clouds of venom that apply stacks of 'Deadly Venom'";
            }
            else if (type == AbilityType.E)
            {
                return "Damage all enemies inflicted with 'Deadly Venom'";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return (int)(7 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rangedDamageLastStep);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.E)
            {
                if (dam == DamageType.RNG)
                    return 35;
                else if (dam == DamageType.MAG)
                    return 20;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 20;
            else if (type == AbilityType.E)
                return 5;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return "Enemies with 'Deadly Venom' take " + GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " ranged damage per stack";
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
                return 11;
            else if (type == AbilityType.E)
                return 14;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanCurrentlyBeCast(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                for (int i = 0; i < Main.npc.Length - 1; i++)
                {
                    NPC npc = Main.npc[i];
                    float distance = 700;
                    if (player.Distance(Main.npc[i].Center) < distance)
                    {
                        if (npc.HasBuff(BuffType<DeadlyVenom>()) && npc.active && !npc.immortal)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            return base.CanCurrentlyBeCast(player, type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.Center;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                    int projType = ProjectileType<VenomCask>();
                    int damage = 5;
                    int knockback = 0;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);

                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type)))
                {
                    player.CheckMana(GetBaseManaCost(type), true);
                    for (int i = 0; i < Main.npc.Length - 1; i++)
                    {
                        NPC npc = Main.npc[i];
                        float distance = 700;

                        if (player.Distance(Main.npc[i].Center) < distance)
                        {
                            if (npc.HasBuff(BuffType<DeadlyVenom>()) && npc.active && !npc.immortal)
                            {
                                Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ProjectileType<Projectiles.Contaminate>(), GetAbilityBaseDamage(player, type), 0, player.whoAmI, i);
                                SetCooldowns(player, type);
                            }
                        }
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
            item.damage = 5;
            item.ranged = true;
            item.noMelee = true;
            item.width = 58;
            item.height = 34;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 5;
            item.knockBack = 0f;
            item.value = 1000;
            item.rare = 1;
            item.shootSpeed = 10f;
            item.shoot = ProjectileType<ToxicArrow>();
            item.UseSound = SoundID.Item5;
            item.useAmmo = AmmoID.Arrow;
        }

        public override bool CanUseItem(Player player)
        {
            item.shoot = player.inventory.Where(x => x.ammo == AmmoID.Arrow).First().shoot;
            return base.CanUseItem(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ProjectileType<ToxicArrow>();
            return true;
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(ItemID.WoodenArrow, 150);

            base.OnCraft(recipe);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                Main.PlaySound(SoundID.Item20, player.Center);
            }
            else if (type == AbilityType.E)
            {

            }
        }
    }
}
