using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;

namespace Glader.Essentials
{
	/// <summary>
	/// Event fired by the UI system when a Text link is clicked.
	/// Text link events are modelled after the TextMeshPro link system.
	/// See: https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0/manual/RichTextLink.html
	/// Where a link has a Link ID and a Link Text.
	/// For example, think clicking on a player's chat message name with [name]. That would be link=(ChatMessageName) with text name. wrapped with [] which is plaintext.
	/// </summary>
	public sealed record TextLinkClickedEventArgs(MouseButton Button, string LinkId, string LinkText) 
		: IEventBusEventArgs
	{

	}
}
