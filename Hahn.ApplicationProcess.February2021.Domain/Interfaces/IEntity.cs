namespace Hahn.Domain.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }

        void SetFrom(IEntity entity);
    }
}
