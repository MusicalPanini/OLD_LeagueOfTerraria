using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class FrozenCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Frozen Cooldown");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.buffImmune[BuffType<Frozen>()] = true;
            if (npc.buffTime[buffIndex] == 1)
            {
                npc.buffImmune[BuffType<Frozen>()] = false;
            }
        }
    }
}
