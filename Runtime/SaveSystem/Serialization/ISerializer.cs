namespace Amenity.SaveSystem
{
    public interface ISerializer<T>
    {
        string SerializeToString(T data);
        T Deserialize(string input);
        string FileExtension { get; }
        TSomeType DeepCopy<TSomeType>(TSomeType data);
    }
}