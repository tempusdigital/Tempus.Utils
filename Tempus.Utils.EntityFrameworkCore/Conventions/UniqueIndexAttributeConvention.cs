namespace Tempus.Utils.EntityFrameworkCore.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore;
    using Tempus.Utils.EntityFrameworkCore.Conventions.Internal;

    /// <summary>
    /// Define Unique Indexed based on the UniqueAttribute
    /// </summary>
    public class UniqueIndexAttributeConvention : IEntityConvention
    {
        public void Apply(IMutableEntityType entityType)
        {
            var clrPropertiesGroupedByUniqueIndexName = entityType.ClrType
                .GetProperties()
                .Select(clrProperty => new
                {
                    clrProperty,
                    attributes = clrProperty.GetCustomAttributes(typeof(UniqueAttribute), false).Cast<UniqueAttribute>()
                })
                .SelectMany(data => data.attributes.Select(attribute => new { data.clrProperty, attribute }))
                .GroupBy(
                    data => data.attribute.IndexName,
                    (indexName, data) => new { indexName, clrProperties = data.Select(s => s.clrProperty) })
                .ToArray();

            foreach (var indexConfig in clrPropertiesGroupedByUniqueIndexName)
            {
                var properties = new List<IMutableProperty>();

                foreach (var clrProperty in indexConfig.clrProperties)
                {
                    var property = entityType.FindProperty(clrProperty);

                    if (property == null)
                        throw new Exception($"A propriedade {clrProperty.Name} em {clrProperty.DeclaringType.FullName} foi decorada com o atributo {nameof(UniqueAttribute)}, porém a propriedade não foi mapeada para o Entity Framework");

                    properties.Add(property);
                }

                var index = entityType.GetOrAddIndex(properties);
                index.IsUnique = true;
            }
        }
    }
}
