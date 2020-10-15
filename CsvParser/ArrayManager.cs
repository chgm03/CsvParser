﻿// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Diagnostics;

namespace SoftCircuits.CsvParser
{
    /// <summary>
    /// Because the number of columns in a CSV is generally the same for each line,
    /// this helper class attempts to reuse an existing array with a minimum number
    /// of resizing or new allocations. If the number of items added to the array
    /// equals the initial size of the array, no resizing or allocations occur.
    /// </summary>
    /// <typeparam name="T">Array type.</typeparam>
    internal class ArrayManager<T>
    {
        private const int GrowBy = 10;

        private T[] Items;
        private int Count;

        /// <summary>
        /// Initializes an <see cref="ArrayManager{T}"/> instance.
        /// </summary>
        /// <param name="items">Initial array.</param>
        public ArrayManager(T[] items)
        {
            Items = items ?? new T[GrowBy];
            Count = 0;
        }

        /// <summary>
        /// Adds a new item to the array.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            if (Items.Length <= Count)
                Array.Resize(ref Items, Count + GrowBy);
            Debug.Assert(Items.Length > Count);
            Items[Count++] = item;
        }

        /// <summary>
        /// Trims any unused items from the array and returns the result.
        /// </summary>
        public T[] GetResults()
        {
            Debug.Assert(Items.Length >= Count);
            if (Items.Length > Count)
                Array.Resize(ref Items, Count);
            Debug.Assert(Items.Length == Count);
            return Items;
        }
    }
}
