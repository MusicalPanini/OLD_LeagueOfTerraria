using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class BladeSurge : Ability
    {
        public BladeSurge(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Blade Surge";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Template";
        }

        public override string GetAbilityTooltip()
        {
            return "Dash a set distance towards a targeted enemy, striking all enemies you pass" +
                "\nResets the cooldown on kill";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 1.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 60;
                case DamageType.SUM:
                    return 50;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 8;
        }

        public override int GetBaseManaCost()
        {
            return 10;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MEL) + " + " + GetScalingTooltip(player, DamageType.SUM) + " melee damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            int npc = TerraLeague.NPCMouseIsHovering(30, true);
            if (npc != -1)
            {
                NPC NPC = Main.npc[npc];
                if (NPC.Distance(player.MountedCenter) <= 16 * 30)
                {
                    if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                    {
                        if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
                        {
                            player.mount.Dismount(player);
                            Vector2 position = player.position;
                            Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 32);
                            int projType = ProjectileType<XanCrestBlades_BladeSurge>();
                            int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL) + GetAbilityScaledDamage(player, DamageType.SUM);
                            int knockback = 0;

                            Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                            SetCooldowns(player, type);
                        }
                    }
                }
                else
                {
                    TerraLeague.DustBorderRing(16 * 30, player.MountedCenter, 6, default(Color), 1);
                }
            }
        }
    }
}
