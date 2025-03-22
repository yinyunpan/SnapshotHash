namespace SnapshotHash
{
    public interface IPayloadHasher
    {
        byte[] ComputeHash(IPayloadWritable value);

        string ComputHashAsBase64String(IPayloadWritable value);

        string ComputHashAsBase64String<TValue>(IDictionary<string, TValue> value) where TValue : IPayloadWritable;
    }
}
