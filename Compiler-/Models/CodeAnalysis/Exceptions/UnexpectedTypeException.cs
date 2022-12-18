using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Exceptions
{

	[Serializable]
	public class UnexpectedTypeException : Exception
	{
		public UnexpectedTypeException() { }
		public UnexpectedTypeException(string message) : base(message) { }
		public UnexpectedTypeException(string message, Exception inner) : base(message, inner) { }
		protected UnexpectedTypeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
