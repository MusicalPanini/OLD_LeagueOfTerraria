using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class LifeGrip : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Life Grip");
            Description.SetDefault("Your resist, life steal, melee, and ranged damage have been increased");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                for (int i = 0; i < modPlayer.Shields.Count; i++)
                {
                    if (modPlayer.Shields[i].AssociatedBuff == Type)
                    {
                        break;
                    }

                    if (i == modPlayer.Shields.Count - 1)
                    {
                        player.ClearBuff(Type);
                    }
                }

                if (modPlayer.Shields.Count == 0)
                {
                    player.ClearBuff(Type);
                }
            }
            player.meleeDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMagic += 1;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMinion += 1;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;

        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
