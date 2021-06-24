using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
