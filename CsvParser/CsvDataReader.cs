﻿// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SoftCircuits.CsvParser
{
    public class CsvDataReader<T> : CsvReader where T : class, new()
    {
        private ColumnInfoCollection ColumnsInfo;
        private ColumnInfo[] MappedColumnsInfo = null;
        private string[] Columns;

        /// <summary>
        /// Initializes a new instance of the CsvDataReader class for the specified file name.
        /// </summary>
        /// <param name="path">The name of the CSV file to read.</param>
        /// <param name="settings">Optional custom settings.</param>
        public CsvDataReader(string path, CsvSettings settings = null)
            : base(path, settings)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the CsvDataReader class for the specified file name,
        /// with the specified character encoding.
        /// </summary>
        /// <param name="path">The name of the CSV file to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="settings">Optional custom settings.</param>
        public CsvDataReader(string path, Encoding encoding, CsvSettings settings = null)
            : base(path, encoding, settings)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the CsvDataReader class for the specified file name,
        /// with the specified byte order mark detection option.
        /// </summary>
        /// <param name="path">The name of the CSV file to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at
        /// <param name="settings">Optional custom settings.</param>
        public CsvDataReader(string path, bool detectEncodingFromByteOrderMarks, CsvSettings settings = null)
            : base(path, detectEncodingFromByteOrderMarks, settings)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the CsvDataReader class for the specified file name,
        /// with the specified character encoding and byte order mark detection option.
        /// </summary>
        /// <param name="path">The name of the CSV file to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at
        /// <param name="settings">Optional custom settings.</param>
        public CsvDataReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, CsvSettings settings = null)
            : base(path, encoding, detectEncodingFromByteOrderMarks, settings)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the CsvDataReader class.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="settings">Optional custom settings.</param>
        public CsvDataReader(Stream stream, CsvSettings settings = null)
            : base(stream, settings)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the CsvDataReader class for the specified stream,
        /// with the specified character encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="settings">Optional custom settings.</param>
        public CsvDataReader(Stream stream, Encoding encoding, CsvSettings settings = null)
            : base(stream, encoding, settings)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the CsvDataReader class for the specified stream,
        /// with the specified byte order mark detection option.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at
        /// <param name="settings">Optional custom settings.</param>
        public CsvDataReader(Stream stream, bool detectEncodingFromByteOrderMarks, CsvSettings settings = null)
            : base(stream, detectEncodingFromByteOrderMarks, settings)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the CsvDataReader class for the specified stream,
        /// with the specified character encoding and byte order mark detection option.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at
        /// <param name="settings">Optional custom settings.</param>
        public CsvDataReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, CsvSettings settings = null)
            : base(stream, encoding, detectEncodingFromByteOrderMarks, settings)
        {
            Initialize();
        }

        /// <summary>
        /// Common initialization.
        /// </summary>
        private void Initialize()
        {
            ColumnsInfo = new ColumnInfoCollection();
            ColumnsInfo.BuildColumnInfoCollection<T>();
            MappedColumnsInfo = ColumnsInfo.SortAndFilter();
            Columns = null;
        }

        /// <summary>
        /// Applies mapping
        /// </summary>
        /// <param name="columnMaps"></param>
        public void MapColumns<TMaps>() where TMaps : ColumnMaps<T>, new()
        {
            TMaps columnMaps = Activator.CreateInstance<TMaps>();
            List<ColumnMap> mapping = columnMaps.GetCustomMaps();
            MappedColumnsInfo = ColumnsInfo.ApplyMapping(columnMaps.GetCustomMaps());
        }

        /// <summary>
        /// Reads a row from the input stream. If <paramref name="useHeadersForColumnOrder"/>
        /// is true, the column headers are used to map columns to class members.
        /// </summary>
        /// <param name="useHeadersForColumnOrder">Specifies whether the column headers
        /// should be used to map columns to class members.</param>
        public bool ReadHeaders(bool useHeadersForColumnOrder)
        {
            if (ReadRow(ref Columns))
            {
                if (useHeadersForColumnOrder)
                {
                    if (Columns.Length == 0)
                        throw new Exception("Cannot read column headers from empty row.");
                    MappedColumnsInfo = ColumnsInfo.ApplyHeaders(Columns, Settings.ColumnHeaderStringComparison);
                }
                return true;
            }
            return false;
        }

        public bool Read(out T item)
        {
            Debug.Assert(MappedColumnsInfo != null);
            if (MappedColumnsInfo.Length == 0)
                throw new Exception("No column mapping found. Mapping data must come from member attributes, column headers or custom column mapping.");

            if (ReadRow(ref Columns))
            {
                item = Activator.CreateInstance<T>();
                for (int i = 0; i < Columns.Length; i++)
                    MappedColumnsInfo[i].SetValue(item, Columns[i], Settings.InvalidDataRaisesException);
                return true;
            }
            item = default;
            return false;
        }
    }
}
