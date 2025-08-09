using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class GroundDisconnector : Switch
    {

        public GroundDisconnector(long globalId) : base(globalId)
        {
        }
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                return true; // GroundDisconnector has no additional properties to compare
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
            return base.HasProperty(property);
        }

        public override void GetProperty(Property property)
        {
            base.GetProperty(property);
        }

        public override void SetProperty(Property property)
        {
            base.SetProperty(property);
        }

        #endregion IAccess implementation

    }
}
