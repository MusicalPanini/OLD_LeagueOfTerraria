using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class TestOfSpirit : Ability
    {
        public TestOfSpirit(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Test of Spirit";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/TestofSpirit";
        }

        public override string GetAbilityTooltip()
        {
            return "Reach out and pull the spirit of an enemy." +
                    "\nThis spirit can be attacked to deal 50% of the damage back to the owner of the spirit.";
        }

        public override int GetRawCooldown()
        {
            return 14;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return "";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                int projType = ProjectileType<EyeofGod_TestofSpirit>();
                int damage = 1;
                int knockback = 0;

                Projectile.NewProjectileDirect(position, velocity, projType, damage, knockback, player.whoAmI);

                SetAnimation(player, 30, 30, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 8, -0.25f);
        }
    }
}
