using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Compiler_.Models.CodeAnalysis.Symbols
{
    public static class BasicFunctions
    {
        public static readonly Function Print = new Function("print",new List<Parameter>() { new Parameter("text", SMType.String) }, SMType.Void);
        public static readonly Function Input = new Function("input",new List<Parameter>(), SMType.String);
        public static readonly Function Random = new Function("random", new List<Parameter>() { new Parameter("max", SMType.Int) }, SMType.Int);

        internal static IEnumerable<Function> GetAll() => typeof(BasicFunctions).GetFields()
                                                                                       .Where(f => f.FieldType == typeof(Function))
                                                                                       .Select(f => f.GetValue(null) as Function);
    }
}
