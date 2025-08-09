using System;
using System.Collections.Generic;
using System.Text;

namespace FTN.Common
{
	
	public enum DMSType : short
	{		
		MASK_TYPE							= unchecked((short)0xFFFF),

		ITP									= 0x0001,
		SO									= 0x0002,
		CURVEDATA							= 0x0003,
		CURVE								= 0x0004,
		OUTAGESCH							= 0x0005,
		GNDDISC								= 0x0006,		
    }

    [Flags]
	public enum ModelCode : long
	{
		IDOBJ								= 0x1000000000000000,
		IDOBJ_GID							= 0x1000000000000104,
        IDOBJ_ALIASNAME						= 0x1000000000000207,
		IDOBJ_MRID							= 0x1000000000000307,
		IDOBJ_NAME							= 0x1000000000000407,

        // ---- IrregularTimePoint (ITP) ----
        ITP                                 = 0x1100000000010000,
        ITP_TIME                            = 0x1100000000010108,
        ITP_VALUE1                          = 0x1100000000010205,
        ITP_VALUE2                          = 0x1100000000010305,
        ITP_INTERVALSCHEDULE                = 0x1100000000010409,

        // ---- BasicIntervalSchedule (BSCINTSCHEDULE) ----
        BSCINTSCHEDULE					    = 0x1200000000000000,
        BSCINTSCHEDULE_STARTTIME			= 0x1200000000000108,
        BSCINTSCHEDULE_V1MULTIPLIER			= 0x120000000000020a,
        BSCINTSCHEDULE_V1UNIT				= 0x120000000000030a,
        BSCINTSCHEDULE_V2MULTIPLIER			= 0x120000000000040a,
        BSCINTSCHEDULE_V2UNIT				= 0x120000000000050a,

        // ---- IrregularIntervalSchedule (IIS) ----
        IISCHEDULE                          = 0x1210000000000000,
        IISCHEDULE_TIMEPOINTS               = 0x1210000000000109,

        // ---- PowerSystemResource (PSR) ----
        PSR                                 = 0x1300000000000000,
        // (PSR nema dodatne property‑je u tabeli)

        // ---- CurveData (CURVEDATA) ----
        CURVEDATA                           = 0x1500000000030000,
        CURVEDATA_XVALUE                    = 0x1500000000030105,
        CURVEDATA_Y1VALUE                   = 0x1500000000030205,
        CURVEDATA_Y2VALUE                   = 0x1500000000030305,
        CURVEDATA_Y3VALUE                   = 0x1500000000030405,
        CURVEDATA_CURVE                     = 0x1500000000030509,  // veza ka Curve

        // ---- Curve (CURVE) ----
        CURVE                               = 0x1600000000040000,
        CURVE_CSTYLE                        = 0x160000000004010a,
        CURVE_XMP                           = 0x160000000004020a,
        CURVE_XUNIT                         = 0x160000000004030a,
        CURVE_Y1MP                          = 0x160000000004040a,
        CURVE_Y1UNIT                        = 0x160000000004050a,
        CURVE_Y2MP                          = 0x160000000004060a,
        CURVE_Y2UNIT                        = 0x160000000004070a,
        CURVE_Y3MP                          = 0x160000000004080a,
        CURVE_Y3UNIT                        = 0x160000000004090a,
        CURVE_CURVEDATAS                    = 0x1600000000040a19,  // lista CurveData

        // ---- Equipment (EQUIPMENT) ----
        EQUIPMENT                           = 0x1310000000000000,

        // ---- ConductingEquipment (CONDEQIP) ----
        CONDEQIP                            = 0x1311000000000000,

        // ---- Switch (SWITCH) ----
        SWITCH                              = 0x1311100000000000,
        SWITCH_SWIOPR                       = 0x1311100000000109,  // lista SwitchingOperation

        // ---- GroundDisconnector (GNDDISC) ----
        GNDDISC                             = 0x1311110000060000,  // isti DMSType kao SWITCH, ali druga vrednost?

        // ---- SwitchingOperation (SO) ----
        SO                                  = 0x1400000000020000,
        SO_NEWSTATE                         = 0x140000000002010a,
        SO_OPERATIONTIME                    = 0x1400000000020208,
        SO_OUTAGESCHEDULE                   = 0x1400000000020309,  // veza ka OutageSchedule
        SO_SWITCHES                         = 0x1400000000020419,  // lista Switch

        // ---- OutageSchedule (OUTAGESCH) ----
        OUTAGESCH                           = 0x1211000000050000,
        OUTAGESCH_SWIOPR                    = 0x1211000000050119, // lista SwitchingOperation


        /*
        PSR									= 0x1100000000000000,
		PSR_CUSTOMTYPE						= 0x1100000000000107,
		PSR_LOCATION						= 0x1100000000000209,

		BASEVOLTAGE							= 0x1200000000010000,
		BASEVOLTAGE_NOMINALVOLTAGE			= 0x1200000000010105,
		BASEVOLTAGE_CONDEQS					= 0x1200000000010219,

		LOCATION							= 0x1300000000020000,
		LOCATION_CORPORATECODE				= 0x1300000000020107,
		LOCATION_CATEGORY					= 0x1300000000020207,
		LOCATION_PSRS						= 0x1300000000020319,

		WINDINGTEST							= 0x1400000000050000,
		WINDINGTEST_LEAKIMPDN				= 0x1400000000050105,
		WINDINGTEST_LOADLOSS				= 0x1400000000050205,
		WINDINGTEST_NOLOADLOSS				= 0x1400000000050305,
		WINDINGTEST_PHASESHIFT				= 0x1400000000050405,
		WINDINGTEST_LEAKIMPDN0PERCENT		= 0x1400000000050505,
		WINDINGTEST_LEAKIMPDNMINPERCENT		= 0x1400000000050605,
		WINDINGTEST_LEAKIMPDNMAXPERCENT		= 0x1400000000050705,
		WINDINGTEST_POWERTRWINDING			= 0x1400000000050809,

		EQUIPMENT							= 0x1110000000000000,
		EQUIPMENT_ISUNDERGROUND				= 0x1110000000000101,
		EQUIPMENT_ISPRIVATE					= 0x1110000000000201,		

		CONDEQ								= 0x1111000000000000,
		CONDEQ_PHASES						= 0x111100000000010a,
		CONDEQ_RATEDVOLTAGE					= 0x1111000000000205,
		CONDEQ_BASVOLTAGE					= 0x1111000000000309,

		POWERTR								= 0x1112000000030000,
		POWERTR_FUNC						= 0x111200000003010a,
		POWERTR_AUTO						= 0x1112000000030201,
		POWERTR_WINDINGS					= 0x1112000000030319,

		POWERTRWINDING						= 0x1111100000040000,
		POWERTRWINDING_CONNTYPE				= 0x111110000004010a,
		POWERTRWINDING_GROUNDED				= 0x1111100000040201,
		POWERTRWINDING_RATEDS				= 0x1111100000040305,
		POWERTRWINDING_RATEDU				= 0x1111100000040405,
		POWERTRWINDING_WINDTYPE				= 0x111110000004050a,
		POWERTRWINDING_PHASETOGRNDVOLTAGE	= 0x1111100000040605,
		POWERTRWINDING_PHASETOPHASEVOLTAGE	= 0x1111100000040705,
		POWERTRWINDING_POWERTRW				= 0x1111100000040809,
		POWERTRWINDING_TESTS				= 0x1111100000040919,

        POWERTR                             = 0x111200000003000,
        POWERTR_FUNC                        = 0x111200000003010a,
        POWERTR_AUTO                        = 0x1112000000030101,
        POWERTR_WINDINGS                    = 0x1112000000030219, 
	    */

    }

    [Flags]
	public enum ModelCodeMask : long
	{
		MASK_TYPE			 = 0x00000000ffff0000,
		MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
		MASK_ATTRIBUTE_TYPE	 = 0x00000000000000ff,

		MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
		MASK_FIRSTNBL		  = unchecked((long)0xf000000000000000),
		MASK_DELFROMNBL8	  = unchecked((long)0xfffffff000000000),		
	}																		
}


