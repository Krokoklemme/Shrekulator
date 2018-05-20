// Shrekulator - Tool to convert several units of measurements to Shrek's
// Copyright(C) 2018 Henning Hoppe
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<https://www.gnu.org/licenses/>.

namespace Shrekulator.Container
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class TrackableMap<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public TrackableMap(IDictionary<TKey, TValue> map = null) => _dict = map ?? new Dictionary<TKey, TValue>();

        private IDictionary<TKey, TValue> _dict;

        public class EntryAddedEventArgs : EventArgs
        {
            public TKey Key { get; }
            public TValue Value { get; }

            public EntryAddedEventArgs(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        public delegate void EntryAddedEventHandler(object sender, EntryAddedEventArgs arg);

        public event EntryAddedEventHandler EntryAdded;

        private void OnEntryAdded(object sender, EntryAddedEventArgs arg) => EntryAdded?.Invoke(sender, arg);

        public class EntryEditedEventArgs : EventArgs
        {
            public TKey Key { get; }
            public TValue OldValue { get; }
            public TValue NewValue { get; }

            public EntryEditedEventArgs(TKey key, TValue oldValue, TValue newValue)
            {
                Key = key;
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        public delegate void EntryEditedEventHandler(object sender, EntryEditedEventArgs arg);

        public event EntryEditedEventHandler EntryEdited;

        private void OnEntryEdited(object sender, EntryEditedEventArgs arg) => EntryEdited?.Invoke(sender, arg);

        public class EntryRemovedEventArgs : EventArgs
        {
            public TKey Key { get; }
            public TValue Value { get; }

            public EntryRemovedEventArgs(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        public delegate void EntryRemovedEventHandler(object sender, EntryRemovedEventArgs arg);

        public event EntryRemovedEventHandler EntryRemoved;

        private void OnEntryRemoved(object sender, EntryRemovedEventArgs arg) => EntryRemoved?.Invoke(sender, arg);

        public int Count => _dict.Count;
        public ICollection<TKey> Keys => _dict.Keys;
        public ICollection<TValue> Values => _dict.Values;
        public bool IsReadOnly => _dict.IsReadOnly;

        public void Clear() => _dict.Clear();

        public void Add(TKey key, TValue value) => this[key] = value;

        public bool ContainsKey(TKey key) => _dict.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => _dict.TryGetValue(key, out value);

        public TValue this[TKey key]
        {
            get => _dict.TryGetValue(key, out var value) ? value : throw new KeyNotFoundException("key not found");
            set
            {
                if (_dict.ContainsKey(key))
                {
                    var old = _dict[key];
                    _dict[key] = value;
                    OnEntryEdited(this, new EntryEditedEventArgs(key, old, value));
                }
                else
                {
                    _dict.Add(key, value);
                    OnEntryAdded(this, new EntryAddedEventArgs(key, value));
                }
            }
        }

        public bool Remove(TKey key)
        {
            var res = false;

            if (_dict.TryGetValue(key, out var val))
            {
                _dict.Remove(key);
                OnEntryRemoved(this, new EntryRemovedEventArgs(key, val));
            }

            return res;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dict.GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item) => _dict.Add(item.Key, item.Value);

        public bool Contains(KeyValuePair<TKey, TValue> item) => _dict.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dict.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => _dict.Remove(item.Key);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dict).GetEnumerator();
    }
}