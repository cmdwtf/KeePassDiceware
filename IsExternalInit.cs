#if !NET5_0_OR_GREATER

namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// A small fix for using init autoprops pre .NET 5.0.
	/// </summary>
	internal static class IsExternalInit { }
}

#endif // !NET5_0_OR_GREATER
