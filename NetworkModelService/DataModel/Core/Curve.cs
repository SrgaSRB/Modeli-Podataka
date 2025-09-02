using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Curve : IdentifiedObject
    {
        private CurveStyle curveStyle;
        private UnitMultiplier xMultiplier;
        private UnitSymbol xUnit;
        private UnitMultiplier y1Multiplier;
        private UnitSymbol y1Unit;
        private UnitMultiplier y2Multiplier;
        private UnitSymbol y2Unit;
        private UnitMultiplier y3Multiplier;
        private UnitSymbol y3Unit;

        private List<long> curveDatas = new List<long>();

        public Curve(long globalId) : base(globalId)
        {
        }

        public CurveStyle CurveStyle
        {
            get { return curveStyle; }
            set { curveStyle = value; }
        }

        public UnitMultiplier XMultiplier
        {
            get { return xMultiplier; }
            set { xMultiplier = value; }
        }

        public UnitSymbol XUnit
        {
            get { return xUnit; }
            set { xUnit = value; }
        }

        public UnitMultiplier Y1Multiplier
        {
            get { return y1Multiplier; }
            set { y1Multiplier = value; }
        }

        public UnitSymbol Y1Unit
        {
            get { return y1Unit; }
            set { y1Unit = value; }
        }

        public UnitMultiplier Y2Multiplier
        {
            get { return y2Multiplier; }
            set { y2Multiplier = value; }
        }

        public UnitSymbol Y2Unit
        {
            get { return y2Unit; }
            set { y2Unit = value; }
        }

        public UnitMultiplier Y3Multiplier
        {
            get { return y3Multiplier; }
            set { y3Multiplier = value; }
        }

        public UnitSymbol Y3Unit
        {
            get { return y3Unit; }
            set { y3Unit = value; }
        }

        public List<long> CurveDatas
        {
            get { return curveDatas; }
            set { curveDatas = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Curve c = (Curve)obj;
                return c.curveStyle == this.curveStyle &&
                       c.xMultiplier == this.xMultiplier &&
                       c.xUnit == this.xUnit &&
                       c.y1Multiplier == this.y1Multiplier &&
                       c.y1Unit == this.y1Unit &&
                       c.y2Multiplier == this.y2Multiplier &&
                       c.y2Unit == this.y2Unit &&
                       c.y3Multiplier == this.y3Multiplier &&
                       c.y3Unit == this.y3Unit;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.CURVE_CSTYLE:
                case ModelCode.CURVE_XMP:
                case ModelCode.CURVE_Y1MP:
                case ModelCode.CURVE_Y2MP:
                case ModelCode.CURVE_Y3MP:
                case ModelCode.CURVE_XUNIT:
                case ModelCode.CURVE_Y1UNIT:
                case ModelCode.CURVE_Y2UNIT:
                case ModelCode.CURVE_Y3UNIT:
                case ModelCode.CURVE_CURVEDATAS:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.CURVE_CSTYLE:
                    prop.SetValue((short)curveStyle);
                    break;

                case ModelCode.CURVE_XMP:
                    prop.SetValue((short)xMultiplier);
                    break;

                case ModelCode.CURVE_XUNIT:
                    prop.SetValue((short)xUnit);
                    break;

                case ModelCode.CURVE_Y1MP:
                    prop.SetValue((short)y1Multiplier);
                    break;

                case ModelCode.CURVE_Y1UNIT:
                    prop.SetValue((short)y1Unit);
                    break;

                case ModelCode.CURVE_Y2MP:
                    prop.SetValue((short)y2Multiplier);
                    break;

                case ModelCode.CURVE_Y2UNIT:
                    prop.SetValue((short)y2Unit);
                    break;

                case ModelCode.CURVE_Y3MP:
                    prop.SetValue((short)y3Multiplier);
                    break;

                case ModelCode.CURVE_Y3UNIT:
                    prop.SetValue((short)y3Unit);
                    break;
                case ModelCode.CURVE_CURVEDATAS:
                    prop.SetValue(curveDatas);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CURVE_CSTYLE:
                    curveStyle = (CurveStyle)property.AsEnum();
                    break;

                case ModelCode.CURVE_XMP:
                    xMultiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.CURVE_XUNIT:
                    xUnit = (UnitSymbol)property.AsEnum();
                    break;

                case ModelCode.CURVE_Y1MP:
                    y1Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.CURVE_Y1UNIT:
                    y1Unit = (UnitSymbol)property.AsEnum();
                    break;

                case ModelCode.CURVE_Y2MP:
                    y2Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.CURVE_Y2UNIT:
                    y2Unit = (UnitSymbol)property.AsEnum();
                    break;

                case ModelCode.CURVE_Y3MP:
                    y3Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.CURVE_Y3UNIT:
                    y3Unit = (UnitSymbol)property.AsEnum();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (curveDatas != null && curveDatas.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.CURVE_CURVEDATAS] = curveDatas.GetRange(0, curveDatas.Count);
            }
            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.CURVEDATA_CURVE:
                    curveDatas.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.CURVEDATA_CURVE:
                    if (curveDatas.Contains(globalId))
                    {
                        curveDatas.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

    }
}
