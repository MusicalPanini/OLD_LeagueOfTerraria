using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class SteraksFury : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sterak's Fury");
            Description.SetDefault("You have a shield made of rage!" +
                "\nYour defence has been increased!" +
                "\nYou are immune to most debuffs!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 20;

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
                BuffID.Frostburn
                };

            for (int i = 0; i < buffs.Length; i++)
            {
                player.buffImmune[buffs[i]] = true;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
