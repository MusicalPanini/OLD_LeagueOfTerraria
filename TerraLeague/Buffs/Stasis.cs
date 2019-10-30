using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class Stasis : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stasis");
            Description.SetDefault("Frozen in time");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.frozen = true;
            player.silence = true;
            player.noItems = true;
        }
    }

    public class PlayerStasis : ModPlayer
    {
        public override void PreUpdate()
        {
            if (player.HasBuff(BuffType<Stasis>()))
            {
                player.position = player.oldPosition;
                player.velocity = Vector2.Zero;
                Lighting.AddLight(player.Center, 1, 1, 0);
            }


            base.PreUpdate();
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (player.HasBuff(BuffType<Stasis>()))
                return false;
            else
                return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (player.HasBuff(BuffType<Stasis>()))
                return false;
            else
                return base.CanBeHitByProjectile(proj);
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (player.HasBuff(BuffType<Stasis>()))
            {
                r = 1;
                g = 1;
                b = 0;
            }

        }
    }
}
