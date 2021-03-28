using Hahn.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hahn.Domain.Models
{
    public abstract class Entity : IEntity
    {
        #region Properties

        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        #endregion

        #region Methods
        public virtual void SetFrom(IEntity entity)
        {
            Id = entity.Id;
        }
        #endregion
    }
}
