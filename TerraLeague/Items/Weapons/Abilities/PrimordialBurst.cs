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
    public class PrimordialBurst : Ability
    {
        public PrimordialBurst(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Primordial Burst";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/PrimordialBurst";
        }

        public override string GetAbilityTooltip()
        {
            return "Blast the targeted enemy with primordial energy." +
                "\nDeal up to 100% increased damage based on targets missing life";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 75;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 40;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            int npc = TerraLeague.NPCMouseIsHovering(30, true);
            if (npc != -1)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
                {

                    Vector2 position = player.position;
                    Vector2 velocity = new Vector2(0, 0);
                    int projType = ProjectileType<CrystalStaff_PrimordialBurst>();
                    int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                    int knockback = 3;

                    SetAnimation(player, 10, 10, Main.npc[npc].Center);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, npc, -1);
                    SetCooldowns(player, type);
                }
            }
        }
    }
}
