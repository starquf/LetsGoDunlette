public enum ElementalType
{
    None = 0,     // ���Ӽ�
    Nature = 1,   // �ڿ�
    Electric = 2, // ����
    Fire = 3,    // ��
    Water = 4,    // ��
    Monster = 5   // ��Ÿ
}

public enum BezierType
{
    Quadratic = 0,
    Cubic = 1,
    Linear = 2
}

public enum PieceType
{
    SKILL = 0
}

public enum CCType
{
    None = -1,
    Stun = 0,          // ����
    Silence = 1,       // ħ��
    Exhausted = 2,     // �ǰ���
    Wound = 3,          // ��ó
    Invincibility = 4, // ����
    Fascinate, //��Ȥ (���̷� ����)
    Heating, //��ü ���� (Ÿ�ν� ����)
    SignOfGoblinGunman //ǥ�� (����ų� ����)
}

public enum BuffType
{
    None = -1,
    Shield = 0,
    Heal = 1,
    Upgrade = 2
}

public enum TextUpAnimType
{
    Fixed = 0,
    Up = 1,
    Volcano = 2
}

public enum EncounterType
{
    Battle,
    RandomEvent,
    Rest,
    BossBattle
}

public enum ScrollType
{
    Heal,
    Use,
    Shield,
    Hiding,
    //Power,
    Chaos,
    Memorie,
    //Flame,
    //Twice
}

public enum ShowInfoRange
{
    Inventory,
    Graveyard
}

public enum EnemyType
{
    NORMAL_SLIME,
    LOW_SKELETON_WARRIOR,
    MIMIC,
    FAIRY,
    QUEEN,
    DEPENDENT,
    HARPY,
    MANDRAGORA,
    GNOLL,
    KOBOLD,
    GAGOYLE,
    ALLIGATOR,
    BUGBEAR,
    REDFOX,
    GARGOYLE_ATTACKER,
    SIRENE,
    TAROS,
    SARO,
    GAR,
    DNAM,
    WOODENDOLL,
    LIZARDMAN,
    GOBLINGUNMAN,
    FISHMAN,
    HYDRA,
    SKELETON_WARRIOR,
    SKELETON_COMMANDER,
}

public enum DesIconType
{
    None = 0,
    Attack = 1,
    Stun = 2,           // ����
    Silence = 3,        // ħ��
    Exhausted = 4,      // �ǰ���
    Wound = 5,          // ��ó
    Invincibility = 6,  // ����
    Fascinate,          // ��Ȥ (���̷� ����)
    Heating,            // ��ü ���� (Ÿ�ν� ����)
    Heal,               // ��
    Upgrade,            //��ȭ
    Shield, //��ȣ��
    SignOfGoblinGunman, //ǥ�� (����ų� ����)
}

public enum AnimName
{
    C_SphereCast,
    C_ManaSphereHit,
    E_ManaSphereHit,
    E_LightningRod,
    E_Static,
    E_Static_Stun,
    N_Drain,
    N_ManaSphereHit,
    N_PoisionCloud,
    N_Wind,
    W_ManaSphereHit,
    W_Bubble,
    W_BoatFare,
    W_BoatFareBonusMoney,
    W_Splash01,
    W_Splash02,
    F_ManaSphereHit,
    F_Arson,
    F_ChainExplosion,
    F_ChainExplosionBonus,
    M_Butt,
    M_Sword,
    M_Bite,
    M_Scratch,
    M_Shield,
    M_Wisp,
    BuffEffect01,
    BuffEffect02,
    BuffEffect03,
    BuffEffect04,
    StunEffect,
    SlientEffect,
    UI_SkillDetermined,
    UI_RewardDetermined,
    W_Shield,
    M_Recover,
    F_Effect02,
    SkillEffect01,
    PlayerHeal,
    PlayerStunned,
    BlueExplosion01,
    EarthEffect02,
    EarthEffect03,
    ElecEffect05,
    T_WaterSplash06,
}

public enum PlayerSkillType
{
    Active_Cooldown,
    Active_Count,
    Passive
}

public enum SkillRange
{
    Single = 0,
    All = 1,
    Random = 2
}

public enum GradeMinsuSibalNum
{
    Normal = 1,
    Epic = 2,
    True6StarMythAwakeningLegendTranscendentReincarnation = 3
}