using System;

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

}
