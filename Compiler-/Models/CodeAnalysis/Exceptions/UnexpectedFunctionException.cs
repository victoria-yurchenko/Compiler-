using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Exceptions
{

	[Serializable]
	public class UnexpectedFunctionException : Exception
	{
		public UnexpectedFunctionException() { }
		public UnexpectedFunctionException(string message) : base(message) { }
		public UnexpectedFunctionException(string message, Exception inner) : base(message, inner) { }
		protected UnexpectedFunctionException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
