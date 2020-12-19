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
    public class DivineJudgement : Ability
    {
        public DivineJudgement(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Divine Judgement";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/DivineJudgement";
        }

        public override string GetAbilityTooltip()
        {
            return "Grant an ally or yourself invulnerability for 2 seconds." +
                    "\nWhen the shield wears off, 7 celestial swords will fall from the sky and explode on the ground";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 100;
                case DamageType.SUM:
                    return 80;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 120;
        }

        public override int GetBaseManaCost()
        {
            return 150;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MEL) + " + " + GetScalingTooltip(player, DamageType.SUM) + " summon damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            int target = TerraLeague.PlayerMouseIsHovering(30, -1, player.team);
            if (target != -1)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
                {
                    Vector2 position = Main.player[target].position;
                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<StarfireSpellblades_DivineJudgement>();
                    int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL) + GetAbilityScaledDamage(player, DamageType.SUM);
                    int knockback = 10;

                    DoEfx(Main.player[target], type);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, target);
                    SetCooldowns(player, type);
                }
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), player.Center);
        }
    }
}
