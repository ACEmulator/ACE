using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.WeenieEditor
{
    public class EnumComboBoxItem : IComparable<EnumComboBoxItem>

    {
        public EnumComboBoxItem(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public int Key { get; set; }

        public string Value { get; set; }

        public int CompareTo(EnumComboBoxItem other)
        {
            if (other == null || Value == null || other.Value == null)
                return 1;

            return Value.CompareTo(other.Value);
        }
    }
}
