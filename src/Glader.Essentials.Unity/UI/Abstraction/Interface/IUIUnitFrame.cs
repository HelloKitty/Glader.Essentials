using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IUIUnitFrame : IUIElement
	{
		UILabeledBar HealthBar { get; }

		UILabeledBar TechniquePointsBar { get; }

		IUIText UnitName { get; }

		IUIText UnitLevel { get; }
	}
}
