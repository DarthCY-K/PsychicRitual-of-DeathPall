using RimWorld;
using UnityEngine;
using Verse.AI.Group;
using Verse;

namespace PsychicRitual_DeathPall.PsychicRitual
{
    public class PsychicRitualToil_SummonDeathPall : PsychicRitualToil
    {
        private PsychicRitualRoleDef invokerRole;

        private FloatRange durationHoursFromQualityRange;

        protected PsychicRitualToil_SummonDeathPall()
        {
        }

        public PsychicRitualToil_SummonDeathPall(PsychicRitualRoleDef invokerRole, FloatRange durationHoursFromQualityRange)
        {
            this.invokerRole = invokerRole;
            this.durationHoursFromQualityRange = durationHoursFromQualityRange;
        }

        public override void Start(Verse.AI.Group.PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            base.Start(psychicRitual, parent);
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            float duration = durationHoursFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
            psychicRitual.ReleaseAllPawnsAndBuildings();
            if (pawn != null)
            {
                ApplyOutcome(psychicRitual, pawn, duration);
            }
        }

        private void ApplyOutcome(Verse.AI.Group.PsychicRitual psychicRitual, Pawn invoker, float duration)
        {
            GameCondition_DeathPall gameCondition_DeathPall = invoker.Map.gameConditionManager.GetActiveCondition(GameConditionDefOf.DeathPall) as GameCondition_DeathPall;
            if (gameCondition_DeathPall == null)
            {
                gameCondition_DeathPall = (GameCondition_DeathPall)GameConditionMaker.MakeCondition(GameConditionDefOf.DeathPall, Mathf.FloorToInt(duration * 2500f));
                gameCondition_DeathPall.psychicRitualDef = psychicRitual.def;
                invoker.Map.GameConditionManager.RegisterCondition(gameCondition_DeathPall);
            }

            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "SummonDeathPallCompleteText".Translate(invoker, Mathf.FloorToInt(duration * 2500f).ToStringTicksToPeriod(), psychicRitual.def.Named("RITUAL")), LetterDefOf.ThreatBig);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Values.Look(ref durationHoursFromQualityRange, "durationHoursFromQualityRange");
        }
    }
}
