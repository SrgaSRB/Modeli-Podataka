﻿using System;

namespace FTN.Common
{
    /// <summary>Način definisanja krive</summary>
    public enum CurveStyle : short
    {
        Unknown = 0,
        ConstantYValue = 1,  // constantYValue
        Formula = 2,  // formula
        RampYValue = 3,  // rampYValue
        StraightLineYValues = 4   // straightLineYValues
    }

    /// <summary>Status prekidača</summary>
    public enum SwitchState : short
    {
        Unknown = 0,
        Close = 1,
        Open = 2
    }

    /// <summary>Multiplikator jedinice</summary>
    public enum UnitMultiplier : short
    {
        Unknown = 0,
        G = 1,
        M = 2,
        c = 3,
        d = 4,
        k = 5,
        m = 6,
        micro = 7,
        n = 8,
        none = 9,
        p = 10
    }

    /// <summary>Simbol jedinice</summary>
    public enum UnitSymbol : short
    {
        Unknown = 0,
        A = 1,
        F = 2,
        H = 3,
        Hz = 4,
        J = 5,
        N = 6,
        a = 7,
        S = 8,
        V = 9,
        VA = 10,
        VAh = 11,
        VAr = 12,
        VArh = 13,
        W = 14,
        Wh = 15,
        deg = 16,
        degC = 17,
        g = 18,
        h = 19,
        m = 20,
        m2 = 21,
        m3 = 22,
        min = 23,
        none = 24,
        ohm = 25,
        rad = 26,
        s = 27
    }

    /*
	public enum PhaseCode : short
	{
		Unknown = 0x0,
		N = 0x1,
		C = 0x2,
		CN = 0x3,
		B = 0x4,
		BN = 0x5,
		BC = 0x6,
		BCN = 0x7,
		A = 0x8,
		AN = 0x9,
		AC = 0xA,
		ACN = 0xB,
		AB = 0xC,
		ABN = 0xD,
		ABC = 0xE,
		ABCN = 0xF
	}
	
	public enum TransformerFunction : short
	{
		Supply = 1,				// Supply transformer
		Consumer = 2,			// Transformer supplying a consumer
		Grounding = 3,			// Transformer used only for grounding of network neutral
		Voltreg = 4,			// Feeder voltage regulator
		Step = 5,				// Step
		Generator = 6,			// Step-up transformer next to a generator.
		Transmission = 7,		// HV/HV transformer within transmission network.
		Interconnection = 8		// HV/HV transformer linking transmission network with other transmission networks.
	}
	
	public enum WindingConnection : short
	{
		Y = 1,		// Wye
		D = 2,		// Delta
		Z = 3,		// ZigZag
		I = 4,		// Single-phase connection. Phase-to-phase or phase-to-ground is determined by elements' phase attribute.
		Scott = 5,   // Scott T-connection. The primary winding is 2-phase, split in 8.66:1 ratio
		OY = 6,		// 2-phase open wye. Not used in Network Model, only as result of Topology Analysis.
		OD = 7		// 2-phase open delta. Not used in Network Model, only as result of Topology Analysis.
	}

	public enum WindingType : short
	{
		None = 0,
		Primary = 1,
		Secondary = 2,
		Tertiary = 3
	}
	*/
}
