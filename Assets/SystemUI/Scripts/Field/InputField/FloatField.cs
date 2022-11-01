namespace inc.stu.SystemUI
{

    public class FloatField : InputField<float>
    {
        protected override float Parse(string value) => float.TryParse(value, out var result) ? result : 0f;
        
        protected override string ValueToText(float value)
        {
            return $"{value:f2}";
        }
    }

}