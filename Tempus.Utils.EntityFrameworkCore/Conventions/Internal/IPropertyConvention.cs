namespace Tempus.Utils.EntityFrameworkCore.Conventions.Internal
{
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// Conventions for Properties
    /// </summary>
    public interface IPropertyConvention: IConvention
    {
        void Apply(IMutableEntityType entityType, IMutableProperty property);
    }
}
