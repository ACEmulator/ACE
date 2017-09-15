using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACE.WeenieEditor
{
    public static class ComboBoxHelpers
    {
        public static void SetEnumDataSource<T>(this ComboBox box, bool allowEmptyEntry = false)
        {
            var values = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(p => new EnumComboBoxItem(Convert.ToInt32(p as Enum), GetEnumDescription(p as Enum)))
                .ToList();

            values.Sort();

            if (allowEmptyEntry)
            {
                values.Insert(0, new EnumComboBoxItem(-1, null));
            }

            box.DataSource = values;
            box.ValueMember = "Key";
            box.DisplayMember = "Value";
            box.SelectedIndex = 0;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes.Length > 0)
                return attributes[0].Description;

            return value.ToString();
        }
    }
}
