using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class CurveData : IdentifiedObject
    {
        private float xvalue;
        private float y1value;
        private float y2value;
        private float y3value;
        private long curve; // Reference to Curve

        public CurveData(long globalId) : base(globalId)
        {
        }

        public float Xvalue
        {
            get { return xvalue; }
            set { xvalue = value; }
        }

        public float Y1value
        {
            get { return y1value; }
            set { y1value = value; }
        }

        public float Y2value
        {
            get { return y2value; }
            set { y2value = value; }
        }

        public float Y3value
        {
            get { return y3value; }
            set { y3value = value; }
        }

        public long Curve
        {
            get { return curve; }
            set { curve = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                CurveData x = (CurveData)obj;
                return (x.xvalue == this.xvalue &&
                        x.y1value == this.y1value &&
                        x.y2value == this.y2value &&
                        x.y3value == this.y3value &&
                        x.curve == this.curve);
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
                case ModelCode.CURVEDATA_XVALUE:
                case ModelCode.CURVEDATA_Y1VALUE:
                case ModelCode.CURVEDATA_Y2VALUE:
                case ModelCode.CURVEDATA_Y3VALUE:
                case ModelCode.CURVEDATA_CURVE:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.CURVEDATA_XVALUE:
                    prop.SetValue(xvalue);
                    break;

                case ModelCode.CURVEDATA_Y1VALUE:
                    prop.SetValue(y1value);
                    break;

                case ModelCode.CURVEDATA_Y2VALUE:
                    prop.SetValue(y2value);
                    break;

                case ModelCode.CURVEDATA_Y3VALUE:
                    prop.SetValue(y3value);
                    break;

                case ModelCode.CURVEDATA_CURVE:
                    prop.SetValue(curve);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.CURVEDATA_XVALUE:
                    xvalue = prop.AsFloat();
                    break;

                case ModelCode.CURVEDATA_Y1VALUE:
                    y1value = prop.AsFloat();
                    break;

                case ModelCode.CURVEDATA_Y2VALUE:
                    y2value = prop.AsFloat();
                    break;

                case ModelCode.CURVEDATA_Y3VALUE:
                    y3value = prop.AsFloat();
                    break;

                case ModelCode.CURVEDATA_CURVE:
                    curve = prop.AsReference();
                    break;

                default:
                    base.SetProperty(prop);
                    break;
            }
        }

        public override bool IsReferenced => false;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (curve != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.CURVE_CURVEDATAS] = new List<long> { curve };
            }

            base.GetReferences(references, refType);
        }
    }
}
