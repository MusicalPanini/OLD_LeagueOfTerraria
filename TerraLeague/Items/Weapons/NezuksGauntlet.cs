using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class NezuksGauntlet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ne'Zuk's Gauntlet");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 52;
            item.mana = 7;
            item.ranged = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.width = 32;
            item.height = 34;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 4f;
            item.value = 55000;
            item.rare = ItemRarityID.LightRed;
            item.shootSpeed = 10f;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 73);
            item.shoot = ProjectileType<NezuksGauntlet_MysticShot>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new EssenceFlux(this));
            abilityItem.SetAbility(AbilityType.E, new ArcaneShift(this));
            abilityItem.ChampQuote = "The gauntlet's for show... the talent's all me";
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }
    }

    public class GauntletGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.netID == NPCID.Mimic && Main.rand.Next(0, 5) == 0 && !npc.SpawnedFromStatue)
            {
                Item.NewItem(npc.getRect(), ItemType<NezuksGauntlet>(), 1, false, -2);
            }

            base.NPCLoot(npc);
        }
    }
}
