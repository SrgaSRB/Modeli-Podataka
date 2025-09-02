using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    public static class FTN11_ProfileConverter
    {
        #region IdentifiedObject
        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cim, ResourceDescription rd)
        {
            if (cim == null || rd == null) return;

            if (cim.MRIDHasValue)
                rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cim.MRID));

            if (cim.NameHasValue)
                rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cim.Name));

            if (cim.AliasNameHasValue)
                rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cim.AliasName));
        }
        #endregion

        #region Curve / CurveData
        public static void PopulateCurveProperties(FTN.Curve cim, ResourceDescription rd)
        {
            if (cim == null || rd == null) return;

            PopulateIdentifiedObjectProperties(cim, rd);

            if (cim.CurveStyleHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_CSTYLE, (short)GetDMSCurveStyle(cim.CurveStyle)));

            if (cim.XMultiplierHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_XMP, (short)GetDMSUnitMultiplier(cim.XMultiplier)));
            if (cim.XUnitHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_XUNIT, (short)GetDMSUnitSymbol(cim.XUnit)));

            if (cim.Y1MultiplierHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_Y1MP, (short)GetDMSUnitMultiplier(cim.Y1Multiplier)));
            if (cim.Y1UnitHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_Y1UNIT, (short)GetDMSUnitSymbol(cim.Y1Unit)));

            if (cim.Y2MultiplierHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_Y2MP, (short)GetDMSUnitMultiplier(cim.Y2Multiplier)));
            if (cim.Y2UnitHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_Y2UNIT, (short)GetDMSUnitSymbol(cim.Y2Unit)));

            if (cim.Y3MultiplierHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_Y3MP, (short)GetDMSUnitMultiplier(cim.Y3Multiplier)));
            if (cim.Y3UnitHasValue)
                rd.AddProperty(new Property(ModelCode.CURVE_Y3UNIT, (short)GetDMSUnitSymbol(cim.Y3Unit)));
        }

        public static void PopulateCurveDataProperties(FTN.CurveData cim, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (cim == null || rd == null) return;

            PopulateIdentifiedObjectProperties(cim, rd);

            if (cim.XvalueHasValue)
                rd.AddProperty(new Property(ModelCode.CURVEDATA_XVALUE, cim.Xvalue));
            if (cim.Y1valueHasValue)
                rd.AddProperty(new Property(ModelCode.CURVEDATA_Y1VALUE, cim.Y1value));
            if (cim.Y2valueHasValue)
                rd.AddProperty(new Property(ModelCode.CURVEDATA_Y2VALUE, cim.Y2value));
            if (cim.Y3valueHasValue)
                rd.AddProperty(new Property(ModelCode.CURVEDATA_Y3VALUE, cim.Y3value));

            // single-reference strana: CurveData -> Curve
            if (cim.CurveHasValue)
            {
                long gid = importHelper.GetMappedGID(cim.Curve.ID);
                if (gid < 0)
                {
                    report.Report.Append("WARNING: Convert ").Append(cim.GetType()).Append(" rdfID=\"").Append(cim.ID)
                         .Append("\" - Curve rdfID=\"").Append(cim.Curve.ID).AppendLine("\" is not mapped to GID!");
                }
                rd.AddProperty(new Property(ModelCode.CURVEDATA_CURVE, gid));
            }
        }
        #endregion

        #region Schedules: OutageSchedule / IrregularTimePoint
        public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cim, ResourceDescription rd)
        {
            if (cim == null || rd == null) return;

            if (cim.StartTimeHasValue)
                rd.AddProperty(new Property(ModelCode.BSCINTSCHEDULE_STARTTIME, cim.StartTime));

            if (cim.Value1MultiplierHasValue)
                rd.AddProperty(new Property(ModelCode.BSCINTSCHEDULE_V1MULTIPLIER, (short)GetDMSUnitMultiplier(cim.Value1Multiplier)));
            if (cim.Value1UnitHasValue)
                rd.AddProperty(new Property(ModelCode.BSCINTSCHEDULE_V1UNIT, (short)GetDMSUnitSymbol(cim.Value1Unit)));

            if (cim.Value2MultiplierHasValue)
                rd.AddProperty(new Property(ModelCode.BSCINTSCHEDULE_V2MULTIPLIER, (short)GetDMSUnitMultiplier(cim.Value2Multiplier)));
            if (cim.Value2UnitHasValue)
                rd.AddProperty(new Property(ModelCode.BSCINTSCHEDULE_V2UNIT, (short)GetDMSUnitSymbol(cim.Value2Unit)));
        }

        public static void PopulateOutageScheduleProperties(FTN.OutageSchedule cim, ResourceDescription rd)
        {
            if (cim == null || rd == null) return;

            PopulateIdentifiedObjectProperties(cim, rd);
            PopulateBasicIntervalScheduleProperties(cim, rd);  // nasleđeno
            // IIS (lista TimePoints) je vektor → ne upisuje se u Deltu
        }

        public static void PopulateIrregularTimePointProperties(FTN.IrregularTimePoint cim, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (cim == null || rd == null) return;

            PopulateIdentifiedObjectProperties(cim, rd);

            if (cim.TimeHasValue)
                rd.AddProperty(new Property(ModelCode.ITP_TIME, cim.Time));
            if (cim.Value1HasValue)
                rd.AddProperty(new Property(ModelCode.ITP_VALUE1, cim.Value1));
            if (cim.Value2HasValue)
                rd.AddProperty(new Property(ModelCode.ITP_VALUE2, cim.Value2));

            // single-reference: ITP -> IntervalSchedule (OutageSchedule)
            if (cim.IntervalScheduleHasValue)
            {
                long gid = importHelper.GetMappedGID(cim.IntervalSchedule.ID);
                if (gid < 0)
                {
                    report.Report.Append("WARNING: Convert ").Append(cim.GetType()).Append(" rdfID=\"").Append(cim.ID)
                         .Append("\" - IntervalSchedule rdfID=\"").Append(cim.IntervalSchedule.ID).AppendLine("\" is not mapped to GID!");
                }
                rd.AddProperty(new Property(ModelCode.ITP_INTERVALSCHEDULE, gid));
            }
        }
        #endregion

        #region SwitchingOperation / (Ground)Disconnector
        public static void PopulateSwitchingOperationProperties(FTN.SwitchingOperation cim, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (cim == null || rd == null) return;

            PopulateIdentifiedObjectProperties(cim, rd);

            if (cim.NewStateHasValue)
                rd.AddProperty(new Property(ModelCode.SO_NEWSTATE, (short)GetDMSSwitchState(cim.NewState)));

            if (cim.OperationTimeHasValue)
                rd.AddProperty(new Property(ModelCode.SO_OPERATIONTIME, cim.OperationTime));

            // single-reference: SwitchingOperation -> OutageSchedule
            if (cim.OutageScheduleHasValue)
            {
                long gid = importHelper.GetMappedGID(cim.OutageSchedule.ID);
                if (gid < 0)
                {
                    report.Report.Append("WARNING: Convert ").Append(cim.GetType()).Append(" rdfID=\"").Append(cim.ID)
                         .Append("\" - OutageSchedule rdfID=\"").Append(cim.OutageSchedule.ID).AppendLine("\" is not mapped to GID!");
                }
                rd.AddProperty(new Property(ModelCode.SO_OUTAGESCHEDULE, gid));
            }
            // Napomena: liste (SO_SWITCHES) se NE upisuju u Deltu.
        }

        public static void PopulateGroundDisconnectorProperties(FTN.GroundDisconnector cim, ResourceDescription rd)
        {
            if (cim == null || rd == null) return;

            // Tvoj ModelCode trenutno nema dodatnih CONDEQ/EQUIPMENT atributa → samo IO
            PopulateIdentifiedObjectProperties(cim, rd);
        }
        #endregion

        #region Enum konverzije (FTN.* -> FTN.Common.*)
        public static CurveStyle GetDMSCurveStyle(FTN.CurveStyle v)
        {
            switch (v)
            {
                case FTN.CurveStyle.constantYValue: return CurveStyle.constantYValue;
                case FTN.CurveStyle.formula: return CurveStyle.formula;
                case FTN.CurveStyle.rampYValue: return CurveStyle.rampYValue;
                case FTN.CurveStyle.straightLineYValues: return CurveStyle.straightLineYValues;
                default: return CurveStyle.rampYValue;
            }
        }

        public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier v)
        {
            switch (v)
            {
                case FTN.UnitMultiplier.G: return UnitMultiplier.G;
                case FTN.UnitMultiplier.M: return UnitMultiplier.M;
                case FTN.UnitMultiplier.c: return UnitMultiplier.c;
                case FTN.UnitMultiplier.d: return UnitMultiplier.d;
                case FTN.UnitMultiplier.k: return UnitMultiplier.k;
                case FTN.UnitMultiplier.m: return UnitMultiplier.m;
                case FTN.UnitMultiplier.micro: return UnitMultiplier.micro;
                case FTN.UnitMultiplier.n: return UnitMultiplier.n;
                case FTN.UnitMultiplier.none: return UnitMultiplier.none;
                case FTN.UnitMultiplier.p: return UnitMultiplier.p;
                default: return UnitMultiplier.none;
            }
        }

        public static UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol v)
        {
            switch (v)
            {
                case FTN.UnitSymbol.A: return UnitSymbol.A;
                case FTN.UnitSymbol.V: return UnitSymbol.V;
                case FTN.UnitSymbol.W: return UnitSymbol.W;
                case FTN.UnitSymbol.Wh: return UnitSymbol.Wh;
                case FTN.UnitSymbol.VA: return UnitSymbol.VA;
                case FTN.UnitSymbol.VAr: return UnitSymbol.VAr;
                case FTN.UnitSymbol.Hz: return UnitSymbol.Hz;
                case FTN.UnitSymbol.s: return UnitSymbol.s;
                case FTN.UnitSymbol.none: return UnitSymbol.none;
                default: return UnitSymbol.none;
            }
        }

        public static SwitchState GetDMSSwitchState(FTN.SwitchState v)
        {
            switch (v)
            {
                case FTN.SwitchState.open: return SwitchState.open;
                case FTN.SwitchState.close: return SwitchState.close;
                default: return SwitchState.close;
            }
        }
        #endregion
    }
}

