using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glader.Essentials
{
	public static class EntityTypeBuilderExtensions
	{
		/// <summary>
		/// Seeds the provided <see cref="EntityTypeBuilder{TEntity}"/> with the enumeration data input
		/// based on the function provided.
		/// </summary>
		/// <typeparam name="TModelType">The model type.</typeparam>
		/// <typeparam name="TEnumType">The num type.</typeparam>
		/// <param name="builder">The type builder.</param>
		/// <param name="creationFunc">Function that takes in input of enum value and produces the model.</param>
		public static void SeedWithEnum<TModelType, TEnumType>(this EntityTypeBuilder<TModelType> builder, Func<TEnumType, TModelType> creationFunc) 
			where TModelType : class
			where TEnumType : Enum
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (creationFunc == null) throw new ArgumentNullException(nameof(creationFunc));
			
			builder.HasData(Enum.GetValues(typeof(TEnumType))
				.Cast<TEnumType>()
				.ToArray()
				.Select(creationFunc)
				.ToArray());
		}

		/// <summary>
		/// Registers the provided property as owned if it's required.
		/// (Ex. if it's a primitive type then it won't be registered as an owned-type).
		/// </summary>
		/// <typeparam name="TEntity">The containing entity.</typeparam>
		/// <typeparam name="TRelatedEntity">Property type.</typeparam>
		/// <param name="builder">The model builder.</param>
		/// <param name="buildAction">The build action.</param>
		public static void OwnsOneIfNeeded<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TRelatedEntity>> buildAction) 
			where TEntity : class 
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (buildAction == null) throw new ArgumentNullException(nameof(buildAction));

			if (!typeof(TRelatedEntity).IsPrimitive)
			{
				typeof(EntityTypeBuilder<TEntity>)
					.GetMethod(nameof(EntityTypeBuilder<TEntity>.OwnsOne), new Type[] {typeof(Expression<Func<TEntity, TRelatedEntity>>)})
					.Invoke(builder, new object[1] { buildAction });
			}
		}
	}
}
