namespace SnapshotHash
{
    /// <summary>
    /// Represents a snapshot object
    /// </summary>
    public interface IPayloadWritable
    {
        void Write(BinaryWriter writer);
    }
}
