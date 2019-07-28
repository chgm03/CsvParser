// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.CsvParser.Converters
{
    internal class CharArrayConverter : CustomConverter<char[]>
    {
        public override string ConvertToString(char[] array) => (array != null) ? new string(array) : string.Empty;

        public override bool TryConvertFromString(string s, out char[] array)
        {
            if (s == null)
            {
                array = null;
                return false;
            }
            array = s.ToCharArray();
            return true;
        }
    }
}
