using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler_.Models.CodeAnalysis.Exceptions
{

	[Serializable]
	public class UnexpectedNodeException : Exception
	{
		public UnexpectedNodeException() { }
		public UnexpectedNodeException(string message) : base(message) { }
		public UnexpectedNodeException(string message, Exception inner) : base(message, inner) { }
		protected UnexpectedNodeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
