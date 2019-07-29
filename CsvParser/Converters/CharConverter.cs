// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.CsvParser.Converters
{
    internal class CharConverter : DataConverter<char>
    {
        public override string ConvertToString(char value) => value.ToString();

        public override bool TryConvertFromString(string s, out char value) => char.TryParse(s, out value);
    }
}
