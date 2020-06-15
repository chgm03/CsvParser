// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Linq;

namespace SoftCircuits.CsvParser
{
    internal class NullableByteArrayConverter : DataConverter<byte?[]>
    {
        public override string ConvertToString(byte?[] array)
        {
            if (array == null || array.Length == 0)
                return string.Empty;

            return string.Join(";", array.Select(v => v.HasValue ? v.Value.ToString() : string.Empty));
        }

        public override bool TryConvertFromString(string s, out byte?[] array)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    array = new byte?[0];
                    return true;
                }

                string[] tokens = s.Split(';');
                array = new byte?[tokens.Length];
                for (int i = 0; i < tokens.Length; i++)
                    array[i] = (tokens[i].Length > 0) ? (byte?)byte.Parse(tokens[i]) : null;
                return true;
            }
            catch (Exception)
            {
                array = null;
                return false;
            }
        }
    }
}
