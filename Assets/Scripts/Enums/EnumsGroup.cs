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
    Stun = 1 << 0,          // ����
    Silence = 1 << 1,       // ħ��
    Exhausted = 1 << 2,     // �ǰ���
    Wound = 1 << 3,         // ��ó
    All = int.MaxValue
}