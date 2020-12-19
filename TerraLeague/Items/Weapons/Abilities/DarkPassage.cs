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
    public class DarkPassage : Ability
    {
        public DarkPassage(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Dark Passage";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/DarkPassage";
        }

        public override string GetAbilityTooltip()
        {
            return "Summon a lantern at the target location and shield all allies near it." +
                    "An ally may also grab the lantern and be brought to your location.";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage);
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
            return 17;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return TerraLeague.CreateScalingTooltip(DamageType.NONE, GetAbilityBaseDamage(player), 100, true) + " + " + GetScalingTooltip(player, DamageType.MEL, true) + " shielding";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                Vector2 position = Main.MouseWorld;
                Vector2 velocity = Vector2.Zero;
                int projType = ProjectileType<ChainWardensScythe_Lantern>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL);
                int knockback = 3;

                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
        }
    }
}
