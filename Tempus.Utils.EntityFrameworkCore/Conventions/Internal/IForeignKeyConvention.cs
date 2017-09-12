namespace Tempus.Utils.EntityFrameworkCore.Conventions.Internal
{
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// Conventions for Foreign Keys
    /// </summary>
    public interface IForeignKeyConvention : IConvention
    {
        void Apply(IMutableEntityType entityType, IMutableForeignKey foreignKey);
    }
}
