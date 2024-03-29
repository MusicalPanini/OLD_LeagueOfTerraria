﻿using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Damnation : Active
    {
        int damage;
        int cooldown;

        public Damnation(int Damage, int Cooldown)
        {
            damage = Damage;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return "[c/ff4d4d:Active: DAMNATION -] [c/ff8080:Deal " + damage + " damage to an enemy at your cursor and steal their speed]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                int npc = TerraLeague.NPCMouseIsHovering(30, true);
                if (npc != -1)
                {
                    DoAction(npc, player, modItem);
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoAction(int npc, Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            NPC NPC = Main.npc[npc];
            NPC.AddBuff(BuffType<Buffs.Slowed>(), 180);

            Efx(player);
            if (Main.netMode == 1)
                PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

            player.ApplyDamageToNPC(NPC, damage, 0, 0, false);
            modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

            Projectile.NewProjectileDirect(NPC.Center, new Vector2( 2,  5), ProjectileType<DamnationProj>(), 0, 0, player.whoAmI);
            Projectile.NewProjectileDirect(NPC.Center, new Vector2(-2,  5), ProjectileType<DamnationProj>(), 0, 0, player.whoAmI);
            Projectile.NewProjectileDirect(NPC.Center, new Vector2( 2, -5), ProjectileType<DamnationProj>(), 0, 0, player.whoAmI);
            Projectile.NewProjectileDirect(NPC.Center, new Vector2(-2, -5), ProjectileType<DamnationProj>(), 0, 0, player.whoAmI);
        }

        public override void Efx(Player user)
        {
            Main.PlaySound(new LegacySoundStyle(2, 20), user.Center);

            base.Efx(user);
        }
    }
}

