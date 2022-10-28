namespace inc.stu.SystemUI
{

    public class FloatField : InputField<float>
    {
        protected override float Parse(string value) => float.TryParse(value, out var result) ? result : 0f;
    }

}