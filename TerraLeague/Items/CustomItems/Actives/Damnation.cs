using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Damnation : Active
    {
        int damage;

        public Damnation(int Damage, int Cooldown)
        {
            damage = Damage;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            return TooltipName("DAMNATION") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Deal ") + damage + TerraLeague.CreateColorString(ActiveSecondaryColor, " damage to an enemy at your cursor and steal their speed")
                + "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
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
            //AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoAction(int npc, Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            NPC NPC = Main.npc[npc];
            NPC.AddBuff(BuffType<Buffs.Slowed>(), 180);

            Efx(player);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

            player.ApplyDamageToNPC(NPC, damage, 0, 0, false);

            SetCooldown(player);
            //modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

            Projectile.NewProjectileDirect(NPC.Center, new Vector2( 2,  5), ProjectileType<Item_Damnation>(), 0, 0, player.whoAmI);
            Projectile.NewProjectileDirect(NPC.Center, new Vector2(-2,  5), ProjectileType<Item_Damnation>(), 0, 0, player.whoAmI);
            Projectile.NewProjectileDirect(NPC.Center, new Vector2( 2, -5), ProjectileType<Item_Damnation>(), 0, 0, player.whoAmI);
            Projectile.NewProjectileDirect(NPC.Center, new Vector2(-2, -5), ProjectileType<Item_Damnation>(), 0, 0, player.whoAmI);
        }

        public override void Efx(Player user)
        {
            Main.PlaySound(new LegacySoundStyle(2, 20), user.Center);

            base.Efx(user);
        }
    }
}

