using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes.Oberon2;
using DUO2C.Semantics;

namespace DUO2C.CodeGen.LLVM
{
    public static partial class IntermediaryCodeGenerator
    {
        public abstract class Value
        {
            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (this is TempIdent || obj is TempIdent) {
                    return this == obj;
                } else {
                    return GetType() == obj.GetType() && ToString().Equals(obj.ToString());
                }
            }

            public override int GetHashCode()
            {
                return GetType().GetHashCode() ^ ToString().GetHashCode();
            }
        }

        public class Literal : Value
        {
            String _str;

            public Literal(String str)
            {
                _str = str;
            }

            public Literal(NNumber num)
            {
                _str = num.Inner.String;
            }

            public override string ToString()
            {
                return _str;
            }
        }

        public class QualIdent : Value
        {
            NQualIdent _ident;

            public QualIdent(String ident)
            {
                _ident = new NQualIdent(ident, null);
            }

            public QualIdent(NQualIdent ident)
            {
                _ident = ident;
            }

            public override string ToString()
            {
                if (_ident.Module != null || _scope.GetSymbolDecl(_ident.Identifier, null).Visibility != AccessModifier.Private) {
                    return String.Format("@{0}.{1}", _ident.Module ?? _module.Identifier, _ident.Identifier);
                } else {
                    return String.Format("@{0}", _ident.Identifier);
                }
            }
        }

        public class TempIdent : Value
        {
            static int _sLast = 0;

            public static void Reset()
            {
                _sLast = 0;
            }

            int _id;

            public int ID
            {
                get
                {
                    if (_id == 0) {
                        _id = ++_sLast;
                    }
                    return _id;
                }
            }

            public TempIdent()
            {
                _id = 0;
            }

            public override string ToString()
            {
                return String.Format("%{0}", ID);
            }
        }

        public class GlobalStringIdent : Value
        {
            static int _sNext = 0;

            public static void Reset()
            {
                _sNext = 0;
            }

            int _id;

            public int ID
            {
                get
                {
                    if (_id == -1) {
                        _id = _sNext ++;
                    }
                    return _id;
                }
            }

            public GlobalStringIdent()
            {
                _id = -1;
            }

            public override string ToString()
            {
                return String.Format("@.str{0}", ID);
            }
        }

        public class TempLabel : Value
        {
            static int _sLast = 0;

            public static void Reset()
            {
                _sLast = 0;
            }

            int _id;

            public string ID
            {
                get
                {
                    if (_id == 0) {
                        _id = ++_sLast;
                    }
                    return "." + _id;
                }
            }

            public TempLabel()
            {
                _id = 0;
            }

            public override string ToString()
            {
                return String.Format("%{0}", ID);
            }
        }
    }
}
