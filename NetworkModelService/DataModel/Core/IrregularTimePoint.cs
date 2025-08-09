using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class IrregularTimePoint : IdentifiedObject
    {

        private long intervalSchedule = 0;
        private float time;
        private float value1;
        private float value2;

        public IrregularTimePoint(long globalId) : base(globalId)
        {
        }

        public float Time { get => time; set => time = value; }
        public float Value1 { get => value1; set => value1 = value; }
        public float Value2 { get => value2; set => value2 = value; }
        public long IntervalSchedule { get => intervalSchedule; set => intervalSchedule = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                IrregularTimePoint x = (IrregularTimePoint)obj;

                return (x.time == this.time && x.intervalSchedule == this.intervalSchedule && x.value1 == this.value1 && x.value2 == this.value2);
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

        #region IAccess implementation

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.ITP_INTERVALSCHEDULE:
                case ModelCode.ITP_TIME:
                case ModelCode.ITP_VALUE1:
                case ModelCode.ITP_VALUE2:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ITP_INTERVALSCHEDULE:
                    property.SetValue(IntervalSchedule);
                    break;

                case ModelCode.ITP_TIME:
                    property.SetValue(Time);
                    break;

                case ModelCode.ITP_VALUE1:
                    property.SetValue(Value1);
                    break;

                case ModelCode.ITP_VALUE2:
                    property.SetValue(Value2);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ITP_INTERVALSCHEDULE:
                    IntervalSchedule = property.AsReference();
                    break;

                case ModelCode.ITP_TIME:
                    time = property.AsFloat();
                    break;

                case ModelCode.ITP_VALUE1:
                    value1 = property.AsFloat();
                    break;

                case ModelCode.ITP_VALUE2:
                    value2 = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (IntervalSchedule != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.ITP_INTERVALSCHEDULE] = new List<long> { IntervalSchedule };
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation
    }
}
