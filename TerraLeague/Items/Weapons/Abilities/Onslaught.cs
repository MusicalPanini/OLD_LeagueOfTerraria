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
    public class Onslaught : Ability
    {
        public Onslaught(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Onslaught";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Onslaught";
        }

        public override string GetAbilityTooltip()
        {
            return "Rapidly attack all nearby enemies, gaining 2 life per hit";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 0.2f);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 20;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 20;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MEL) + " melee damage"
                 +"\nUses 10% Severum Ammo";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override bool CanCurrentlyBeCast(Player player)
        {
            return player.GetModPlayer<PLAYERGLOBAL>().severumAmmo >= 10;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                player.GetModPlayer<PLAYERGLOBAL>().severumAmmo -= 10;
                int damage = GetAbilityBaseDamage(player) + (GetAbilityScaledDamage(player, DamageType.MEL));
                player.AddBuff(BuffType<Buffs.Onslaught>(), damage);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 45, -0.5f);
        }
    }
}
