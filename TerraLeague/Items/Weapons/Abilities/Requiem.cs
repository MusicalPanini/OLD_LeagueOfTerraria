using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class Requiem : Ability
    {
        public Requiem(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Requiem";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Requiem";
        }

        public override string GetAbilityTooltip()
        {
            return "Channel for 4 seconds then deal damage to all enemy npc's in the world";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 125;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 90;
        }

        public override int GetBaseManaCost()
        {
            return 200;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                modPlayer.requiemDamage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                List<NPC> npcs = Main.npc.OfType<NPC>().ToList();
                if (npcs.Count != 0)
                {
                    modPlayer.TaggedNPC = npcs;
                    for (int i = 0; i < npcs.Count - 1; i++)
                    {
                        if (!npcs[i].townNPC && !npcs[i].immortal && npcs[i].active)
                        {
                            npcs[i].AddBuff(BuffType<Buffs.Requiem>(), 240);
                        }
                    }
                }

                player.AddBuff(BuffType<RequiemChannel>(), 240);
                SetCooldowns(player, type);
            }
        }
    }
}
