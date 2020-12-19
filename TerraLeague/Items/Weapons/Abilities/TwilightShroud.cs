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
    public class TwilightShroud : Ability
    {
        public TwilightShroud(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Twilight Shroud";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/TwilightShroud";
        }

        public override string GetAbilityTooltip()
        {
            return "Drop a smoke bomb that causes you to become obsucured and immune to projectiles while not using items";
        }

        public override int GetBaseManaCost()
        {
            return 100;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override int GetRawCooldown()
        {
            return 30;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                DoEfx(player, type);
                int[] order = new int[] { -4, 4, -3, 3, -2, 2, -1, 1, 0 };

                for (int i = 0; i < 9; i++)
                {
                    Projectile.NewProjectile(player.Center, new Vector2(order[i], 0), ProjectileType<AssassinsKunai_ShroudSmoke>(), 0, 0, player.whoAmI);
                    Projectile.NewProjectile(player.Center, new Vector2(order[i], 0), ProjectileType<AssassinsKunai_ShroudSmoke>(), 0, 0, player.whoAmI);
                    Projectile.NewProjectile(player.Center, new Vector2(order[i], 0), ProjectileType<AssassinsKunai_ShroudSmoke>(), 0, 0, player.whoAmI);
                    Projectile.NewProjectile(player.Center, new Vector2(order[i], 0), ProjectileType<AssassinsKunai_ShroudSmoke>(), 0, 0, player.whoAmI);
                }
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            player.itemAnimation = ItemUseStyleID.SwingThrow;
            player.itemAnimationMax = 24;
            player.reuseDelay = 24;
            Main.PlaySound(new LegacySoundStyle(2, 11).WithPitchVariance(-1), player.Center);
        }
    }
}
