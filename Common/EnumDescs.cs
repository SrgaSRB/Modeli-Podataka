﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Common
{
    public class EnumDescs
    {
        private Dictionary<ModelCode, Type> property2enumType = new Dictionary<ModelCode, Type>();

        public EnumDescs()
        {
            /*
			property2enumType.Add(ModelCode.CONDEQ_PHASES, typeof(PhaseCode));
			property2enumType.Add(ModelCode.POWERTR_FUNC, typeof(TransformerFunction));
			property2enumType.Add(ModelCode.POWERTRWINDING_CONNTYPE, typeof(WindingConnection));
			property2enumType.Add(ModelCode.POWERTRWINDING_WINDTYPE, typeof(WindingType));														
		    */

            property2enumType.Add(ModelCode.BSCINTSCHEDULE_V1MULTIPLIER, typeof(UnitMultiplier));
            property2enumType.Add(ModelCode.BSCINTSCHEDULE_V2MULTIPLIER, typeof(UnitMultiplier));
            property2enumType.Add(ModelCode.BSCINTSCHEDULE_V1UNIT, typeof(UnitSymbol));
            property2enumType.Add(ModelCode.BSCINTSCHEDULE_V2UNIT, typeof(UnitSymbol));

            property2enumType.Add(ModelCode.CURVE_CSTYLE, typeof(CurveStyle));
            property2enumType.Add(ModelCode.CURVE_XMP, typeof(UnitMultiplier));
            property2enumType.Add(ModelCode.CURVE_Y1MP, typeof(UnitMultiplier));
            property2enumType.Add(ModelCode.CURVE_Y2MP, typeof(UnitMultiplier));
            property2enumType.Add(ModelCode.CURVE_Y3MP, typeof(UnitMultiplier));
            property2enumType.Add(ModelCode.CURVE_XUNIT, typeof(UnitSymbol));
            property2enumType.Add(ModelCode.CURVE_Y1UNIT, typeof(UnitSymbol));
            property2enumType.Add(ModelCode.CURVE_Y2UNIT, typeof(UnitSymbol));
            property2enumType.Add(ModelCode.CURVE_Y3UNIT, typeof(UnitSymbol));

            property2enumType.Add(ModelCode.SO_NEWSTATE, typeof(SwitchState));

        }

        public List<string> GetEnumList(ModelCode propertyId)
        {
            List<string> enumList = new List<string>();

            if (property2enumType.ContainsKey(propertyId))
            {
                Type type = property2enumType[propertyId];

                for (int i = 0; i < Enum.GetValues(type).Length; i++)
                {
                    enumList.Add(Enum.GetValues(type).GetValue(i).ToString());
                }
            }
            else
            {
                throw new Exception(string.Format("Failed to get enum list. Property ({0}) is not of enum type.", propertyId));
            }

            return enumList;
        }

        public List<string> GetEnumList(Type enumType)
        {
            List<string> enumList = new List<string>();

            try
            {
                for (int i = 0; i < Enum.GetValues(enumType).Length; i++)
                {
                    enumList.Add(Enum.GetValues(enumType).GetValue(i).ToString());
                }

                return enumList;
            }
            catch
            {
                throw new Exception(string.Format("Failed to get enum list. Type ({0}) is not of enum type.", enumType));
            }
        }

        public Type GetEnumTypeForPropertyId(ModelCode propertyId)
        {
            if (property2enumType.ContainsKey(propertyId))
            {
                return property2enumType[propertyId];
            }
            else
            {
                throw new Exception(string.Format("Property ({0}) is not of enum type.", propertyId));
            }
        }

        public Type GetEnumTypeForPropertyId(ModelCode propertyId, bool throwsException)
        {
            if (property2enumType.ContainsKey(propertyId))
            {
                return property2enumType[propertyId];
            }
            else if (throwsException)
            {
                throw new Exception(string.Format("Property ({0}) is not of enum type.", propertyId));
            }
            else
            {
                return null;
            }
        }

        public short GetEnumValueFromString(ModelCode propertyId, string value)
        {
            Type type = GetEnumTypeForPropertyId(propertyId);

            if (Enum.GetUnderlyingType(type) == typeof(short))
            {
                return (short)Enum.Parse(type, value);
            }
            else if (Enum.GetUnderlyingType(type) == typeof(uint))
            {
                return (short)((uint)Enum.Parse(type, value));
            }
            else if (Enum.GetUnderlyingType(type) == typeof(byte))
            {
                return (short)((byte)Enum.Parse(type, value));
            }
            else if (Enum.GetUnderlyingType(type) == typeof(sbyte))
            {
                return (short)((sbyte)Enum.Parse(type, value));
            }
            else
            {
                throw new Exception(string.Format("Failed to get enum value from string ({0}). Invalid underlying type.", value));
            }
        }

        public string GetStringFromEnum(ModelCode propertyId, short enumValue)
        {
            if (property2enumType.ContainsKey(propertyId))
            {
                string retVal = Enum.GetName(GetEnumTypeForPropertyId(propertyId), enumValue);
                if (retVal != null)
                {
                    return retVal;
                }
                else
                {
                    return enumValue.ToString();
                }
            }
            else
            {
                return enumValue.ToString();
            }
        }
    }
}
