using System.Collections.Generic;
using GameServerCore.Enums;

namespace GameServerCore.Domain.GameObjects
{
    public interface IChampion : IObjAiBase
    {
        IShop Shop { get; }
        float RespawnTimer { get; }
        float ChampionGoldFromMinions { get; set; }
        IRuneCollection RuneList { get; }
        Dictionary<short, ISpell> Spells { get; }
        int Skin { get; }
        IChampionStats ChampStats { get; }
        byte SkillPoints { get; set; }

        // basic
        void UpdateSkin(int skinNo);
        bool CanMove();
        bool CanCast();
        void Recall();
        void Respawn();
        bool OnDisconnect();

        // spells
        void SwapSpells(byte slot1, byte slot2);
        void RemoveSpell(byte slot);
        ISpell SetSpell(string name, byte slot, bool enabled = false);
        ISpell GetSpell(byte slot);
        ISpell LevelUpSpell(byte slot);

        void OnKill(IAttackableUnit killed);
    }
}
