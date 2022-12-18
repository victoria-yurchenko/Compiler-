using Compiler_.Models.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis.Binding
{

    public sealed class SMConvert
    {
        public static SMConvert None => new SMConvert(false, false, false);
        public static SMConvert Identity => new SMConvert(true, true, true);
        public static SMConvert Implicit => new SMConvert(true, false, true);
        public static SMConvert Explicit => new SMConvert(true, false, false);

        private SMConvert(bool exists, bool isIdentity, bool isImplicit)
        {
            Exists = exists;
            IsIdentity = isIdentity;
            IsImplicit = isImplicit;
        }

        public bool Exists { get; }
        public bool IsIdentity { get; }
        public bool IsImplicit { get; }
        public bool IsExplicit => Exists && !IsImplicit;

        public static SMConvert Classify(SMType from, SMType to)
        {
            if (from == to)
                return Identity;

            if(from == SMType.Int ||
               from == SMType.Bool)
            {
                if (to == SMType.String)
                    return Explicit;
            }

            if (from == SMType.String)
            {
                if (from == SMType.Int ||
                    from == SMType.Bool)
                    return Explicit;
            }

            return None;
        }
    }
}
