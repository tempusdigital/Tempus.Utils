namespace Tempus.Utils.EntityFrameworkCore
{
    using System;

    /// <summary>
    /// Defined the property as parte of a Unique Index.
    /// If the sabe IndexName is used for multiples properties, all of them will be part of the Unique Index
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class UniqueAttribute : Attribute
    {
        public UniqueAttribute()
        {

        }

        public UniqueAttribute(string indexName)
        {
            IndexName = indexName;
        }

        public string IndexName { get; }
    }
}
