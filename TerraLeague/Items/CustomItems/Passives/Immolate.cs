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
            return TooltipName("IMMOLATE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Set near by enemies on fire and deal " + (weaker ? 10 : 50) + " damage per second");
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
                TerraLeague.GiveNPCsInRangeABuff(player.MountedCenter, effectRadius, (weaker ? BuffType<Buffs.WeakSunfire>() : BuffType<Buffs.Sunfire>()), 240, true, true);

                Efx(player);
                SendEfx(player, modItem);
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
    }
}
