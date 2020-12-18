using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class GreatBeyondBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Great Beyond");
            Description.SetDefault("+20 Ability Haste");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.abilityHaste += 20;

            if (modPlayer.bottleOfStardust)
            {
                modPlayer.itemHaste += 20;
                modPlayer.summonerHaste += 20;
            }
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.bottleOfStardust)
                tip = "+20 Ability, Item, and Summoner spell Haste";
            else
                tip = "+20 Ability Haste";
            base.ModifyBuffTip(ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
