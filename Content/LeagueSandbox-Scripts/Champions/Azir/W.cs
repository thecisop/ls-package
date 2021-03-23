using GameServerCore;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class AzirW : IGameScript
    {
        public float petTimeAlive = 0.00f;

        public void OnActivate(IObjAiBase owner)
        {
        }

        private void OnUnitHit(IAttackableUnit target, bool isCrit)
        {
        }

        public void OnDeactivate(IObjAiBase owner)
        {
        }

        public void OnStartCasting(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
        }

        public void OnFinishCasting(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var castrange = spell.SpellData.CastRange[0];
            var apbonus = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 35 + ((15 * (spell.Level - 1)) + apbonus); //TODO: Should replace minion AA damage
            var jackduration = 5.0f; //TODO: Split into Active duration and Hidden duration when Invisibility is implemented
            var attspeed = 1 / (owner.Stats.AttackSpeedFlat * .55f);
            var ownerPos = owner.Position;
            var spellPos = new Vector2(spell.X, spell.Y);

            if (!Extensions.IsVectorWithinRange(ownerPos, spellPos, castrange))
            {
                spellPos = Extensions.GetClosestCircleEdgePoint(spellPos, ownerPos, castrange);
            }

            IMinion m = AddMinion((IChampion)owner, "AzirSoldier", "AzirSoldier", spellPos);
            // AddParticle(owner, "JackintheboxPoof.troy", spellPos);

            var attackrange = m.Stats.Range.Total;

            if (!m.IsDead)
            {
                var units = GetUnitsInRange(m.Position, attackrange, true);
                foreach (var value in units)
                {
                    if (owner.Team != value.Team && value is IAttackableUnit && !(value is IBaseTurret) && !(value is IObjAnimatedBuilding))
                    {
                        m.SetTargetUnit(value);
                        m.AutoAttackProjectileSpeed = 1450;
                        m.AutoAttackHit(value);
                        for (petTimeAlive = 0.0f; petTimeAlive < jackduration; petTimeAlive += attspeed)
                        {
                            CreateTimer(petTimeAlive, () =>
                            {
                                if (!value.IsDead && !m.IsDead)
                                {
                                    value.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                                }
                            });
                        }
                    }

                }
            }



        }

        public void ApplyEffects(IObjAiBase owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
