namespace Hahn.Domain.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }

        /// <summary>
        /// Set the properties of this entity from another.
        /// </summary>
        /// <param name="entity">The entity that will be used to get the new values.</param>
        void SetFrom(IEntity entity);
    }
}
