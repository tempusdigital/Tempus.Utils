namespace Tempus.Utils.EntityFrameworkCore.Conventions
{
    using Microsoft.EntityFrameworkCore.Metadata;
    using Tempus.Utils.EntityFrameworkCore.Conventions.Internal;

    /// <summary>
    /// Defined all string properties as required
    /// </summary>
    public class StringRequiredConvention : IPropertyConvention
    {
        public void Apply(IMutableEntityType entityType, IMutableProperty property)
        {
            // Todos os campos string por padrão são obrigatórios
            if (property.ClrType == typeof(string))
            {
                property.IsNullable = false;
            }
        }
    }
}
