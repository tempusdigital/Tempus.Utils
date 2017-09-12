namespace Tempus.Utils.EntityFrameworkCore.Conventions.Internal
{
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// Conventions for entities
    /// </summary>
    public interface IEntityConvention : IConvention
    {
        void Apply(IMutableEntityType entityType);
    }
}
