namespace Unni.Infrastructure.Database.Models.Interfaces
{
    public interface IModelId<TKey> where TKey : struct, IComparable, IComparable<TKey>
    {
        public TKey Id { get; set; }
    }
}
