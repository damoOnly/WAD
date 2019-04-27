using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class FieldValue
    {
        public string Name { get; set; }

        public short Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class DictionaryFieldValue
    {
        public DictionaryFieldValue()
        {
            Key = string.Empty;
            Value = 0;
        }
        public DictionaryFieldValue(string key, byte value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public byte Value { get; set; }
    }
}
