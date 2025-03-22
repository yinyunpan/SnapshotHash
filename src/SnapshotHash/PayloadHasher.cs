using System.Data.HashFunction;
using System.Data.HashFunction.MurmurHash;

namespace SnapshotHash
{
    public class PayloadHasher : IPayloadHasher
    {
        private readonly IHashFunction _hashFunction = MurmurHash3Factory.Instance.Create();
        public byte[] ComputeHash(IPayloadWritable value)
        {
            if (value == null)
            {
                return Array.Empty<byte>();
            }

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    value.Write(writer);
                }
                bytes = stream.ToArray();
            }
            return _hashFunction.ComputeHash(bytes).Hash;
        }

        public string ComputHashAsBase64String(IPayloadWritable value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    value.Write(writer);
                }
                bytes = stream.ToArray();
            }
            return _hashFunction.ComputeHash(bytes).AsBase64String();
        }

        public string ComputHashAsBase64String<TValue>(IDictionary<string, TValue> value) where TValue : IPayloadWritable
        {
            if (value?.Any() != true)
            {
                return string.Empty;
            }
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    foreach (var kv in value.OrderBy(it => it.Key))
                    {
                        writer.Write(kv.Key);
                        kv.Value.Write(writer);
                    }
                }
                bytes = stream.ToArray();
            }
            return _hashFunction.ComputeHash(bytes).AsBase64String();
        }
    }
}
