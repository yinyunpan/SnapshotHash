using Google.Protobuf;
using Google.Protobuf.Collections;

namespace SnapshotHash
{
    public static class Comparer
    {
        public static int Compare<T>(T x, T y) where T : IComparable<T>
        {
            return x == null
                ? y == null ? 0 : -1
                : y == null ? 1 : x.CompareTo(y);
        }

        public static int Compare<T>(IEnumerable<T> x, IEnumerable<T> y) where T : IComparable<T>
        {
            var count1 = x?.Count() ?? 0;
            var count2 = y?.Count() ?? 0;

            if (count1 != count2)
            {
                return count1.CompareTo(count2);
            }

            if (count1 == 0)
            {
                return 0;
            }

            var array1 = x.OrderBy(it => it).ToArray();
            var array2 = y.OrderBy(it => it).ToArray();
            for (int index = 0; index < count1; index++)
            {
                var item1 = array1[index];
                var item2 = array2[index];
                var result = Compare(item1, item2);
                if (result != 0)
                {
                    return result;
                }
            }
            return 0;
        }

        public static int Compare<TKey, TValue>(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
            where TKey : IComparable<TKey>
            where TValue : IComparable<TValue>
        {
            var count1 = x?.Count() ?? 0;
            var count2 = y?.Count() ?? 0;
            if (count1 != count2)
            {
                return count1.CompareTo(count2);
            }

            if (count1 == 0)
            {
                return 0;
            }

            return Compare(x?.Select(it => new KeyValuePairWrapper<TKey, TValue>(it)), y?.Select(it => new KeyValuePairWrapper<TKey, TValue>(it)));
        }

        public static int Compare<TKey, TValue>(IDictionary<TKey, IList<TValue>> x, IDictionary<TKey, IList<TValue>> y)
            where TKey : IComparable<TKey>
            where TValue : IComparable<TValue>
        {
            var count1 = x?.Count() ?? 0;
            var count2 = y?.Count() ?? 0;
            if (count1 != count2)
            {
                return count1.CompareTo(count2);
            }

            if (count1 == 0)
            {
                return 0;
            }

            return Compare(x?.Select(it => new KeyValueCollectionPairWrapper<TKey, TValue>(it)), y?.Select(it => new KeyValueCollectionPairWrapper<TKey, TValue>(it)));
        }

        public static int Compare<TKey, TValue>(Dictionary<TKey, List<TValue>> x, Dictionary<TKey, List<TValue>> y)
          where TKey : IComparable<TKey>
          where TValue : IComparable<TValue>
        {
            var count1 = x?.Count() ?? 0;
            var count2 = y?.Count() ?? 0;
            if (count1 != count2)
            {
                return count1.CompareTo(count2);
            }

            if (count1 == 0)
            {
                return 0;
            }

            return Compare(x?.Select(it => new KeyValueCollectionPairWrapper<TKey, TValue>(ConvertPair(it))), y?.Select(it => new KeyValueCollectionPairWrapper<TKey, TValue>(ConvertPair(it))));
        }

        private static KeyValuePair<TKey, IList<TValue>> ConvertPair<TKey, TValue>(KeyValuePair<TKey, List<TValue>> originalPair)
        {
            return new KeyValuePair<TKey, IList<TValue>>(originalPair.Key, originalPair.Value);
        }

        public static int Compare(RepeatedField<ByteString> x, RepeatedField<ByteString> y)
        {
            var count1 = x?.Count ?? 0;
            var count2 = y?.Count ?? 0;

            if (count1 != count2)
            {
                return count1.CompareTo(count2);
            }

            if (count1 == 0)
            {
                return 0;
            }

            for (int index = 0; index < x.Count; index++)
            {
                var result = Compare<byte>(x[index].ToByteArray(), y[index].ToByteArray());
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        private struct KeyValuePairWrapper<TKey, TValue> : IComparable<KeyValuePairWrapper<TKey, TValue>>
            where TKey : IComparable<TKey>
            where TValue : IComparable<TValue>
        {
            public KeyValuePairWrapper(KeyValuePair<TKey, TValue> kv)
            {
                KV = kv;
            }
            public KeyValuePair<TKey, TValue> KV { get; }
            public int CompareTo(KeyValuePairWrapper<TKey, TValue> other)
            {
                var result = Compare(KV.Key, other.KV.Key);
                return result != 0
                    ? result
                    : Compare(KV.Value, other.KV.Value);
            }
        }

        private struct KeyValueCollectionPairWrapper<TKey, TValue> : IComparable<KeyValueCollectionPairWrapper<TKey, TValue>>
           where TKey : IComparable<TKey>
           where TValue : IComparable<TValue>
        {
            public KeyValueCollectionPairWrapper(KeyValuePair<TKey, IList<TValue>> kv)
            {
                KV = kv;
            }
            public KeyValuePair<TKey, IList<TValue>> KV { get; }
            public int CompareTo(KeyValueCollectionPairWrapper<TKey, TValue> other)
            {
                var result = Compare(KV.Key, other.KV.Key);
                return result != 0
                    ? result
                    : Compare(KV.Value, other.KV.Value);
            }
        }
    }
}
