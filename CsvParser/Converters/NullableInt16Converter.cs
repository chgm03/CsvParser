// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.CsvParser.Converters
{
    internal class NullableInt16Converter : CustomConverter<short?>
    {
        public override string ConvertToString(short? value) => value.HasValue ? value.Value.ToString() : string.Empty;

        public override bool TryConvertFromString(string s, out short? value)
        {
            value = null;

            if (s == string.Empty)
                return true;
            
            if (short.TryParse(s, out short temp))
            {
                value = temp;
                return true;
            }
            return false;
        }
    }
}
