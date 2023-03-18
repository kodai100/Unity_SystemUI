
namespace inc.stu.SystemUI
{
    public class StringField : InputField<string>
    {
        protected override string Parse(string value) => value;
        protected override string ValueToText(string value)
        {
            return value;
        }
    }
}


