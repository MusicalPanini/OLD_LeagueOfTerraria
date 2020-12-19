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
    public class SoulShackles : Ability
    {
        public static int range = 550;

        public SoulShackles(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Soul Shackles";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/SoulShackles";
        }

        public override string GetAbilityTooltip()
        {
            return "Unleash celestial chains upon near by enemies." +
                    "\nAfter 3 seconds, if the chain is not broken, the enemy will be restuck and stunned for 4 seconds." +
                    "\nThe chain will break if the target moves too far away";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 4);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 70;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 70;
        }

        public override int GetBaseManaCost()
        {
            return 100;
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
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = Vector2.Zero;
                int projType = ProjectileType<TheFallenCelestialsDarkMagic_SoulShackles>();
                int knockback = 0;
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);

                DoEfx(player, type);
                player.CheckMana(GetBaseManaCost(), true);

                var npcs = TerraLeague.GetAllNPCsInRange(player.MountedCenter, range, true, true);

                for (int i = 0; i < npcs.Count; i++)
                {
                    SetCooldowns(player, type);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, npcs[i]);
                }
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 3, 54, -0.2f);

            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 248, 0, 0, 0, new Color(159, 0, 255), 1.5f);
                dust.velocity *= 2f;
                dust.noGravity = true;
            }
        }
    }
}
