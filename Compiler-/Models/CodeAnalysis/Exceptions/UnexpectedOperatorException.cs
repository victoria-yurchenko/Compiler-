using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Exceptions
{

	[Serializable]
	public class UnexpectedOperatorException : Exception
	{
		public UnexpectedOperatorException() { }
		public UnexpectedOperatorException(string message) : base(message) { }
		public UnexpectedOperatorException(string message, Exception inner) : base(message, inner) { }
		protected UnexpectedOperatorException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
