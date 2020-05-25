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
        int cooldown;
        int effectDuration;

        public FrostfireCovenant(int Damage, int EffectDuration, int Cooldown)
        {
            damage = Damage;
            effectDuration = EffectDuration;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return "[c/ff4d4d:Active: FROSTFIRE COVENANT -] [c/ff8080:Target an ally to channel the Harbinger of Fire through them for " + effectDuration +" seconds.]" +
                "\n[c/ff8080:All their attacks will deal 40 On Hit damage and apply 'Harbingers Inferno']" +
                "\n[c/ff8080:At the same time you channel the Harbinger of Frost for " + effectDuration + " seconds, slowing near by enemies in a Frost Storm around you.]" +
                "\n[c/ff8080:Burnt enemies caught in the Frost Storm will take " + damage + " magic damage]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
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
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoAction(int target, Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.SendBuffPacket(BuffType<HarbingerOfFire>(), effectDuration * 60, target, -1, player.whoAmI);
            player.AddBuff(BuffType<HarbingerOfFrost>(), effectDuration * 60);
            modPlayer.frostHarbinger = true;
            Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ProjectileType<Item_FrostStorm>(), damage, 0, player.whoAmI, target).timeLeft = effectDuration * 60;

            modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));
        }
    }
}

