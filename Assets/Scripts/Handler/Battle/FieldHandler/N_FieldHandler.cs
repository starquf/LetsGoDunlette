public class N_FieldHandler : FieldHandler
{
    private void Awake()
    {
        fieldType = ElementalType.Nature;
    }

    public override void DisableField(bool skip = false)
    {
        base.DisableField();
    }

    public override void EnableField(bool skip = false)
    {
        base.EnableField();
    }
}
