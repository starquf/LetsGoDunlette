public enum PatternType
{
    None = 0,
    Clover,
    Diamonds,
    Heart,
    Spade,
    Monster
}

public enum BezierType
{
    Quadratic,
    Cubic,
    Linear
}

public enum PieceType
{
    SKILL
}

[System.Flags]
public enum CCType
{
    None = 0,
    Stun = 1 << 0,          // ±âÀý
    Silence = 1 << 1,       // Ä§¹¬
    Exhausted = 1 << 2,     // ÇÇ°ïÇÔ
    Wound = 1 << 3,         // »óÃ³
    All = int.MaxValue
}