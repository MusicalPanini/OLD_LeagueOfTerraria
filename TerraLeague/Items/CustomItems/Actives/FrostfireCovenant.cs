using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class FrostfireCovenant : Active
    {
        int damage;
        int effectDuration;

        public FrostfireCovenant(int Damage, int EffectDuration, int Cooldown)
        {
            damage = Damage;
            effectDuration = EffectDuration;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("FROSTFIRE COVENANT") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Target an ally to channel the Harbinger of Fire through them for " + effectDuration + " seconds." +
                "\nAll their attacks will deal 40 On Hit damage and apply 'Harbingers Inferno'" +
                "\nAt the same time you channel the Harbinger of Frost for " + effectDuration + " seconds, slowing near by enemies in a Frost Storm around you." +
                "\nBurnt enemies caught in the Frost Storm will take " + damage + " magic damage") +
                "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                int target = TerraLeague.PlayerMouseIsHovering(30);
                if (target != -1 && target != player.whoAmI)
                {
                    DoAction(target, player, modItem);
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoAction(int target, Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.SendBuffPacket(BuffType<HarbingerOfFire>(), effectDuration * 60, target, -1, player.whoAmI);
            player.AddBuff(BuffType<HarbingerOfFrost>(), effectDuration * 60);
            modPlayer.frostHarbinger = true;
            Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ProjectileType<Item_FrostStorm>(), damage, 0, player.whoAmI, target).timeLeft = effectDuration * 60;
            SetCooldown(player);
        }
    }
}

