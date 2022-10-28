
namespace inc.stu.SystemUI
{
    public class IntField : InputField<int>
    {
        protected override int Parse(string value) => int.TryParse(value, out var result) ? result : 0;
    }
}


