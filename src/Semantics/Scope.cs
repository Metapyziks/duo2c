﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Semantics
{
    public enum AccessModifier : byte
    {
        Private = 0,
        ReadOnly = 1,
        Public = 2
    }

    public enum DeclarationType : byte
    {
        Constant = 0,
        Global = 1,
        Local = 2,
        Parameter = 3,
        Field = 4,
        BoundProcedure = 5
    }

    public class Declaration
    {
        public OberonType Type { get; private set; }
        public AccessModifier Visibility { get; private set; }
        public DeclarationType DeclarationType { get; private set; }
        
        public bool IsConstant { get { return DeclarationType == DeclarationType.Constant; } }
        public bool IsGlobal { get { return DeclarationType == DeclarationType.Global; } }
        public bool IsLocal { get { return DeclarationType == DeclarationType.Local; } }
        public bool IsVariable { get { return IsLocal || IsGlobal || IsParameter; } }
        public bool IsParameter { get { return DeclarationType == DeclarationType.Parameter; } }
        public bool IsField { get { return DeclarationType == DeclarationType.Field; } }

        public Declaration(OberonType type, AccessModifier visibility, DeclarationType declType)
        {
            Type = type;
            Visibility = visibility;
            DeclarationType = declType;
        }
    }

    public class Scope
    {
        public Scope Parent { get; private set; }

        public bool HasParent { get { return Parent != null; } }

        private Dictionary<String, Declaration> _types;
        private Dictionary<String, Declaration> _symbols;

        public Scope(Scope parent = null)
        {
            Parent = parent;

            _types = new Dictionary<string, Declaration>();
            _symbols = new Dictionary<string, Declaration>();
        }

        public void DeclareType(String identifier, OberonType type, AccessModifier visibility)
        {
            _types.Add(identifier, new Declaration(type, visibility, DeclarationType.Global));
        }

        public void DeclareSymbol(String identifier, OberonType type, AccessModifier visibility, DeclarationType declType)
        {
            _symbols.Add(identifier, new Declaration(type, visibility, declType));
        }

        public virtual Declaration GetTypeDecl(String identifier, String module = null)
        {
            if (module != null) {
                return Parent != null ? Parent.GetTypeDecl(identifier, module) : null;
            } else {
                return _types.ContainsKey(identifier)
                    ? _types[identifier] : HasParent
                    ? Parent.GetTypeDecl(identifier, null) : null;
            }
        }

        public virtual Declaration GetSymbolDecl(String identifier, String module = null)
        {
            if (module != null) {
                return Parent != null ? Parent.GetSymbolDecl(identifier, module) : null;
            } else {
                return _symbols.ContainsKey(identifier)
                    ? _symbols[identifier] : HasParent
                    ? Parent.GetSymbolDecl(identifier, null) : null;
            }
        }

        public bool IsTypeDeclared(String identifier, String module = null)
        {
            return GetTypeDecl(identifier, module) != null;
        }

        public bool IsSymbolDeclared(String identifier, String module = null)
        {
            return GetSymbolDecl(identifier, module) != null;
        }

        public bool IsSymbolConstant(String identifier, String module = null)
        {
            var decl = GetSymbolDecl(identifier, module);
            if (decl != null) {
                return decl.IsConstant;
            } else {
                return false;
            }
        }

        public AccessModifier GetTypeVisibility(String identifier, String module = null)
        {
            var decl = GetTypeDecl(identifier, module);
            if (decl != null) {
                return decl.Visibility;
            } else {
                return AccessModifier.Public;
            }
        }

        public AccessModifier GetSymbolVisibility(String identifier, String module = null)
        {
            var decl = GetSymbolDecl(identifier, module);
            if (decl != null) {
                return decl.Visibility;
            } else {
                return AccessModifier.Public;
            }
        }

        public OberonType GetType(String identifier, String module = null)
        {
            var decl = GetTypeDecl(identifier, module);
            if (decl != null) {
                return decl.Type.Resolve(this);
            } else {
                return null;
            }
        }

        public OberonType GetSymbol(String identifier, String module = null)
        {
            var decl = GetSymbolDecl(identifier, module);
            if (decl != null) {
                return decl.Type.Resolve(this);
            } else {
                return null;
            }
        }

        public IEnumerable<KeyValuePair<String, Declaration>> GetTypes()
        {
            return _types;
        }

        public IEnumerable<KeyValuePair<String, Declaration>> GetSymbols()
        {
            return _symbols;
        }
    }

    public class RootScope : Scope
    {
        private Dictionary<String, Scope> _modules;

        public RootScope()
        {
            _modules = new Dictionary<string,Scope>();
        }

        public Scope CreateModuleScope(ModuleType type)
        {
            var scope = new Scope(this);
            _modules.Add(type.Identifier, scope);
            DeclareSymbol(type.Identifier, type, AccessModifier.Public, DeclarationType.Global);
            return scope;
        }

        public override Declaration GetTypeDecl(string identifier, string module = null)
        {
            return module != null && _modules.ContainsKey(module)
                ? _modules[module].GetTypeDecl(identifier, null)
                : base.GetTypeDecl(identifier, null);
        }

        public override Declaration GetSymbolDecl(string identifier, string module = null)
        {
            return module != null && _modules.ContainsKey(module)
                ? _modules[module].GetSymbolDecl(identifier, null)
                : base.GetSymbolDecl(identifier, null);
        }
    }
}
