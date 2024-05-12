using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI.Group;
using Verse;

namespace PsychicRitual_DeathPall.PsychicRitual
{
    public class PsychicRitualDef_SummonDeathPall : PsychicRitualDef_InvocationCircle
    {
        private FloatRange durationHoursFromQualityRange;

        public override List<PsychicRitualToil> CreateToils(Verse.AI.Group.PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_SummonDeathPall(InvokerRole, durationHoursFromQualityRange));
            return list;
        }

        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return outcomeDescription.Formatted(Mathf.FloorToInt(durationHoursFromQualityRange.LerpThroughRange(qualityRange.min) * 2500f).ToStringTicksToPeriod());
        }
    }
}
