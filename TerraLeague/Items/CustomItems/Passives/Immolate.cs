using Microsoft.Xna.Framework;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Immolate : Passive
    {
        int effectRadius;
        bool weaker;

        public Immolate(int EffectRadius, bool Weaker)
        {
            effectRadius = EffectRadius;
            weaker = Weaker;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: IMMOLATE -] [c/99e6ff:Set near by enemies on fire and deal " + (weaker ? 10 : 50) + " damage per second]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (Main.time % 240 == 0)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC DamTarget = Main.npc[i];

                    float damtoX = DamTarget.position.X + (float)DamTarget.width * 0.5f - player.Center.X;
                    float damtoY = DamTarget.position.Y + (float)DamTarget.height * 0.5f - player.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(damtoX * damtoX + damtoY * damtoY));

                    if (distance < effectRadius && !DamTarget.townNPC && DamTarget.lifeMax > 5 && !DamTarget.immortal)
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            DamTarget.GetGlobalNPC<NPCsGLOBAL>().PacketHandler.SendAddBuff(-1, player.whoAmI, (weaker ? BuffType<Buffs.WeakSunfire>() : BuffType<Buffs.Sunfire>()), 240, i);

                        DamTarget.AddBuff((weaker ? BuffType<Buffs.WeakSunfire>() : BuffType<Buffs.Sunfire>()), 240);
                    }
                }

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem));
            }
        }

        public override void Efx(Player user)
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 vel = new Vector2(13, 0).RotatedBy(MathHelper.ToRadians(20 * i));

                Dust dust = Dust.NewDustPerfect(user.Center, 6, vel, 0, default(Color), 3);
                dust.noGravity = true;
                dust.noLight = true;
            }

            TerraLeague.DustBorderRing(effectRadius, user.MountedCenter, 6, Color.White, 3);
            base.Efx(user);
        }

        public override void Efx(Player user, NPC effectedNPC)
        {
            base.Efx(user, effectedNPC);
        }
    }
}
