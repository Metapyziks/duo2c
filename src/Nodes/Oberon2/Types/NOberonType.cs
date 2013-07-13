using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUO2C.Semantics;

namespace DUO2C.Nodes.Oberon2
{
    [SubstituteToken("OberonType")]
    public class NOberonType : TypeDefinition
    {
        private OberonType _type;
        public override OberonType Type
        {
            get { return _type; }
        }

        public NOberonType(ParseNode original)
            : base(original, true)
        {
            switch (String.ToUpper()) {
                case "LONGINT":
                    _type = IntegerType.LongInt; break;
                case "INTEGER":
                    _type = IntegerType.Integer; break;
                case "SHORTINT":
                    _type = IntegerType.ShortInt; break;
                case "BYTE":
                    _type = IntegerType.Byte; break;
                case "LONGREAL":
                    _type = RealType.LongReal; break;
                case "REAL":
                    _type = RealType.Real; break;
                case "BOOLEAN":
                    _type = BooleanType.Default; break;
                case "SET":
                    _type = SetType.Default; break;
                case "CHAR":
                    _type = CharType.Default; break;
            }
        }
    }
}
