using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class ColdSteel : Passive
    {
        int SlowDuration;

        public ColdSteel(int slowDurationSeconds)
        {
            SlowDuration = slowDurationSeconds;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: COLD STEEL -] [c/99e6ff:Chance to slow near by enemies upon taking damage]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            

            base.UpdateAccessory(player, modItem);
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.OnHitByNPC(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.OnHitByProjectile(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.OnHitByProjectile(proj, ref damage, ref crit, player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {
            if (Main.rand.Next(0, 5) == 0)
            {
                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem));

                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    float distance = 700;
                    if (!Main.npc[i].townNPC && Main.npc[i].active && !Main.npc[i].immortal)
                    {
                        Vector2 newMove = npc.Center - player.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (player.Distance(npc.Center) < distance)
                        {
                            npc.AddBuff(BuffType<Buffs.Slowed>(), SlowDuration * 60);
                        }
                    }
                }
            }
        }

        override public void Efx(Player user)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 30), user.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(user.position, user.width, user.height, 67, 0, 0, 0, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity *= 5f;

                dust = Dust.NewDustDirect(new Vector2(user.position.X, user.position.Y), user.width, user.height, 67, 0f, 0f, 100, default(Color), 1f);
                dust.velocity *= 3f;
            }
        }
    }
}
