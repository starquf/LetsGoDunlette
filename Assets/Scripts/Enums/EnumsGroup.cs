public enum ElementalType
{
    None = 0,     // 무속성
    Nature = 1,   // 자연
    Electric = 2, // 번개
    Fire = 3,    // 불
    Water = 4,    // 물
    Monster = 5   // 몬스타
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
    Stun = 0,          // 기절
    Silence = 1,       // 침묵
    Exhausted = 2,     // 피곤함
    Wound = 3,          // 상처
    Invincibility = 4, // 무적
    Fascinate, //매혹 (세이렌 전용)
    Heating, //신체 가열 (타로스 전용)
    SignOfGoblinGunman //표식 (고블린거너 전용)
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
    Stun = 2,           // 기절
    Silence = 3,        // 침묵
    Exhausted = 4,      // 피곤함
    Wound = 5,          // 상처
    Invincibility = 6,  // 무적
    Fascinate,          // 매혹 (세이렌 전용)
    Heating,            // 신체 가열 (타로스 전용)
    Heal,               // 힐
    Upgrade,            //강화
    Shield, //보호막
    SignOfGoblinGunman, //표식 (고블린거너 전용)
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