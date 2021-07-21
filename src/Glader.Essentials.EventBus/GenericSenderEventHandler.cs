using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public delegate void GenericSenderEventHandler<in TSenderType, in TEventArgs>(TSenderType? sender, TEventArgs e);
}
