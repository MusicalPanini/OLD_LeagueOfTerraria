using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using System.Linq;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class GeneralCleanse : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cleansed");
            Description.SetDefault("You feel pure!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            int[] buffs = new int[] {
                BuffID.Bleeding,
                BuffID.Poisoned,
                BuffID.OnFire,
                BuffID.Venom,
                BuffID.Darkness,
                BuffID.Blackout,
                BuffID.Silenced,
                BuffID.Cursed,
                BuffID.Cursed,
                BuffID.Confused,
                BuffID.Slow,
                BuffID.OgreSpit,
                BuffID.Weak,
                BuffID.BrokenArmor,
                BuffID.WitheredArmor,
                BuffID.WitheredWeapon,
                BuffID.CursedInferno,
                BuffID.Ichor,
                BuffID.Chilled,
                BuffID.Frozen,
                BuffID.Webbed,
                BuffID.Stoned,
                BuffID.VortexDebuff,
                BuffID.Obstructed,
                BuffID.Electrified,
                BuffID.Rabies,
                BuffID.MoonLeech,
                BuffID.Burning,
                BuffID.Frostburn,
                BuffType<GrievousWounds>()
                };

            for (int i = 0; i < buffs.Length; i++)
            {
                player.buffImmune[buffs[i]] = true;
            }
        }
    }
}
