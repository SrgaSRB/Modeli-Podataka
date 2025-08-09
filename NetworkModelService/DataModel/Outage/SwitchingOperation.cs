using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Outage
{
    public class SwitchingOperation : IdentifiedObject
    {

        private SwitchState newstate;
        private DateTime operationTime;
        private long outageSchedule = 0;
        private List<long> switches = new List<long>();

        public SwitchingOperation(long globalId) : base(globalId)
        {
        }

        public SwitchState NewState { get => newstate; set => newstate = value; }
        public DateTime OperationTime { get => operationTime; set => operationTime = value; }
        public long OutageSchedule { get => outageSchedule; set => outageSchedule = value; }
        public List<long> Switches { get => switches; set => switches = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SwitchingOperation x = (SwitchingOperation)obj;

                return (x.outageSchedule == this.outageSchedule && x.newstate == this.newstate && x.operationTime == this.operationTime
                        && CompareHelper.CompareLists(x.Switches, this.Switches, true));
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
                case ModelCode.SO_NEWSTATE:
                case ModelCode.SO_OPERATIONTIME:
                case ModelCode.SO_OUTAGESCHEDULE:
                case ModelCode.SO_SWITCHES:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SO_NEWSTATE:
                    property.SetValue((short)newstate);
                    break;

                case ModelCode.SO_OPERATIONTIME:
                    property.SetValue(operationTime);
                    break;

                case ModelCode.SO_OUTAGESCHEDULE:
                    property.SetValue(outageSchedule);
                    break;

                case ModelCode.SO_SWITCHES:
                    property.SetValue(switches);
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
                case ModelCode.SO_NEWSTATE:
                    newstate = (SwitchState)property.AsEnum();
                    break;

                case ModelCode.SO_OPERATIONTIME:
                    operationTime = property.AsDateTime();
                    break;

                case ModelCode.SO_OUTAGESCHEDULE:
                    outageSchedule = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return (Switches.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Switches != null && Switches.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.SO_SWITCHES] = switches.GetRange(0, switches.Count);
            }

            if (outageSchedule != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.SO_OUTAGESCHEDULE] = new List<long> { outageSchedule };
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SWITCH_SWIOPR:
                    switches.Add(globalId);
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
                case ModelCode.SWITCH_SWIOPR:

                    if (switches.Contains(globalId))
                    {
                        switches.Remove(globalId);
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

        #endregion IReference implementation
    }
}
