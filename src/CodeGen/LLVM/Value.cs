﻿using System;
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
        public interface IComplexWrite
        {
            GenerationContext Write(GenerationContext ctx);
        }

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

        public class TypeIdent : Value
        {
            private String _ident;

            public TypeIdent(String ident)
            {
                _ident = ident;
            }

            public TypeIdent(String ident, String module)
            {
                _ident = String.Format("{0}.{1}", module, ident);
            }

            public override string ToString()
            {
                return String.Format("%{0}", _ident);
            }
        }

        public class GlobalIdent : Value
        {
            private String _ident;

            public bool IsPublic { get; private set; }

            public GlobalIdent(String ident, bool isPublic)
            {
                _ident = ident;
                IsPublic = isPublic;
            }

            public override string ToString()
            {
                return String.Format("@{0}", _ident);
            }
        }

        public class Literal : Value
        {
            public static Literal GetDefault(OberonType type)
            {
                if (type.IsBool) return new Literal("false");
                if (type.IsInteger) return new Literal(0.ToString());
                if (type.IsReal) return new Literal(0.ToString("e"));
                throw new NotImplementedException(String.Format("No default literal for type '{0}' found", type));
            }

            String _str;

            public Literal(byte[] bytes)
            {
                _str = String.Format("c\"{0}\"", bytes.Aggregate(String.Empty,
                    (s, x) => String.Format("{0}\\{1}", s, x.ToString("X2"))));
            }

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

            public String Identifier
            {
                get { return _ident.Identifier; }
            }

            public String Module
            {
                get { return _ident.Module ?? _module.Identifier; }
            }

            public QualIdent(String ident, String module = null)
            {
                _ident = new NQualIdent(ident, module);
            }

            public QualIdent(NQualIdent ident)
            {
                _ident = ident;
            }

            public override string ToString()
            {
                return String.Format("@{0}.{1}", Module, Identifier);
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
                    ResolveID();
                    return _id;
                }
            }

            public TempIdent()
            {
                _id = 0;
            }

            public void ResolveID()
            {
                if (_id == 0) {
                    _id = ++_sLast;
                }
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
                return String.Format("@const.string.{0}", ID);
            }
        }

        public class ElementPointer : Value, IComplexWrite
        {
            public bool InBounds { get; private set; }
            public OberonType StructureType { get; private set; }
            public Value Structure { get; private set; }
            public int[] Indices { get; private set; }

            public ElementPointer(bool inBounds, OberonType structType, Value structure, params int[] indices)
            {
                InBounds = inBounds;
                StructureType = structType;
                Structure = structure;
                Indices = indices;
            }

            public GenerationContext Write(GenerationContext ctx)
            {
                ctx.Keyword("getelementptr");
                if (InBounds) ctx.Keyword("inbounds");
                ctx.Write("(").Argument(StructureType, Structure);
                foreach (var ind in Indices) {
                    ctx.Argument(IntegerType.Integer, new Literal(ind.ToString()));
                }
                return ctx.Write("\t)");
            }
        }

        static GenerationContext Write(this GenerationContext ctx, Value val)
        {
            if (val is IComplexWrite) {
                return ((IComplexWrite) val).Write(ctx);
            } else {
                return ctx.Write(() => val.ToString());
            }
        }
    }
}
