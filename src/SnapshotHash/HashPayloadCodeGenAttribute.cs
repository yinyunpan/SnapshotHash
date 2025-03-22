namespace SnapshotHash
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HashPayloadCodeGenAttribute : Attribute
    {
        public string[] ExcludedProperties { get; }
        public HashPayloadCodeGenAttribute(params string[] excludedProperties) => ExcludedProperties = excludedProperties;
    }
}
