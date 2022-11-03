using System;
using System.Collections.Generic;
using TMPro;

namespace inc.stu.SystemUI
{
    public enum TestDropdownEnum
    {
        First, Second, Third
    }
    
    public class TestDropdownField : DropdownField<TestDropdownEnum>
    {
        protected override TestDropdownEnum ParseToEnum(int value)
        {
            return (TestDropdownEnum)Enum.ToObject(typeof(TestDropdownEnum), value);
        }

        protected override int ParseToInt(TestDropdownEnum value)
        {
            return (int)value;
        }

        protected override List<TMP_Dropdown.OptionData> CreateOptions()
        {

            var list = new List<TMP_Dropdown.OptionData>();
            
            foreach (TestDropdownEnum value in Enum.GetValues(typeof(TestDropdownEnum)))
            {
                var n = Enum.GetName(typeof(TestDropdownEnum), value);
                var data = new TMP_Dropdown.OptionData
                {
                    text = n
                };
                list.Add(data);
            }

            return list;
        }
    }

}
