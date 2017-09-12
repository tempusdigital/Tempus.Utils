namespace Tempus.Utils.EntityFrameworkCore.Conventions
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using Tempus.Utils.EntityFrameworkCore.Conventions.Internal;

    [Obsolete("Código temporário para adicionar Custom Conventions até que a API do EF 7 esteja completa")]
    public static class ConventionExtensions
    {
        /// <summary>
        /// Apply conventions to model. 
        /// It will override any configuration done so far. 
        /// The best place to apply the conventions is on the top of the OnModelCreating method, after add the entities to the Context.
        /// The conventions will apply only to the entities already added to the context when the ApplyConvention is called.
        /// <code>
        /// public class AdminContext : DbContext
        /// {
        ///     public AdminContext(DbContextOptions options) : base(options)
        ///     {
        /// 
        ///     }
        /// 
        ///     // Add the entity to the Context, then the custom conventions will recognize it
        ///     public DbSet<Product> Products { get; set; }
        /// 
        ///     protected override void OnModelCreating(ModelBuilder modelBuilder)
        ///     {
        ///         // Apply custom conventions
        ///         modelBuilder.ApplyConvention(
        ///             new DateTimeTypeConvention(),
        ///             new StringRequiredConvention(),
        ///             new UniqueIndexAttributeConvention());
        /// 
        ///         modelBuilder.Entity<Product>(opt =>
        ///         {
        ///             // Override conventions
        ///         });
        ///     }
        /// }
        /// </code>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="conventions"></param>
        public static void ApplyConvention(this ModelBuilder modelBuilder, params IConvention[] conventions)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var convention in conventions)
                {
                    if (convention is IEntityConvention)
                        (convention as IEntityConvention).Apply(entity);

                    if (convention is IPropertyConvention)
                        foreach (var property in entity.GetProperties())
                        {
                            (convention as IPropertyConvention).Apply(entity, property);
                        }

                    if (convention is IForeignKeyConvention)
                        foreach (var fk in entity.GetForeignKeys())
                        {
                            (convention as IForeignKeyConvention).Apply(entity, fk);
                        }
                }
            }
        }
    }
}
