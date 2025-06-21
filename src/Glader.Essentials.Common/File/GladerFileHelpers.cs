using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Glader.Essentials
{
	public static class GladerFileHelpers
	{
		/// <summary>
		/// Indicates if the file is locked.
		/// Returns false if the file doesn't even exist.
		/// </summary>
		/// <param name="path">Path to the file.</param>
		/// <returns>True if the file appears to be locked.</returns>
		public static bool IsFileLocked(string path)
		{
			if(!File.Exists(path))
				return false;

			try
			{
				using var stream = new FileStream(
					path,
					FileMode.Open,
					FileAccess.ReadWrite,     // catch shares opened for write
					FileShare.None);          // require exclusive access
			}
			catch(IOException ex) when(IsLockViolation(ex))
			{
				return true;
			}

			return false;
		}

		private static bool IsLockViolation(IOException ex)
		{
			// LOWORD of HResult is the Win32 error code:
			var code = ex.HResult & 0xFFFF;
			const int ERROR_SHARING_VIOLATION = 32;
			const int ERROR_LOCK_VIOLATION = 33;
			return code == ERROR_SHARING_VIOLATION || code == ERROR_LOCK_VIOLATION;
		}
	}
}
