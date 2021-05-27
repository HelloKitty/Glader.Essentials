using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Glader.Essentials
{
	public static class GeneratorExecutionContextExtensions
	{
		/// <summary>
		/// Retrieves all <see cref="INamedTypeSymbol"/>s from the provided <see cref="GeneratorExecutionContext"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>An enumerable of all types in the global namespace of the assembly.</returns>
		public static IEnumerable<INamedTypeSymbol> GetAllTypes(this GeneratorExecutionContext context)
		{
			return context
				.Compilation
				.Assembly
				.GlobalNamespace
				.GetAllTypes()
				.Where(t => t.ContainingAssembly != null && t.ContainingAssembly.Equals(context.Compilation.Assembly, SymbolEqualityComparer.Default));
		}
	}
}
