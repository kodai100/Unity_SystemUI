using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace inc.stu.SystemUI
{

    public class TestStringArrayDropdownField : DropdownField<(string, int)>
    {

        private List<string> _data = new()
        {
            "Alice",
            "Bob",
            "Chris",
            "Darwin"
        };

        private List<(string, int)> _indicesPairList;

        protected override void Awake()
        {
            base.Awake();

            _indicesPairList = _data.Select((s, i) => (s, i)).ToList();
        }
        
        protected override (string, int) ParseToEnum(int value)
        {
            return (_data[value], value);
        }

        protected override int ParseToInt((string, int) value)
        {
            return _indicesPairList.First(d => d.Item1.Equals(value.Item1)).Item2;
        }

        protected override List<TMP_Dropdown.OptionData> CreateOptions()
        {

            var list = new List<TMP_Dropdown.OptionData>();
            
            foreach (var value in _data)
            {
                list.Add(new TMP_Dropdown.OptionData{text = value});
            }

            return list;
        }
    }

}
