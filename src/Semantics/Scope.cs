using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C.Semantics
{
    public class Scope
    {
        public Scope Parent { get; private set; }

        public bool HasParent { get { return Parent != null; } }

        private Dictionary<String, OberonType> _types;
        private Dictionary<String, OberonType> _symbols;
        private HashSet<String> _constants;

        public Scope(Scope parent = null)
        {
            Parent = parent;

            _types = new Dictionary<string, OberonType>();
            _symbols = new Dictionary<string, OberonType>();
            _constants = new HashSet<string>();
        }

        public void DeclareType(String identifier, OberonType type)
        {
            _types.Add(identifier, type);
        }

        public void DeclareSymbol(String identifier, OberonType type, bool constant = false)
        {
            _symbols.Add(identifier, type);
            if (constant) {
                _constants.Add(identifier);
            }
        }

        public bool IsTypeDeclared(String identifier, String module = null)
        {
            return GetType(identifier, module) != null;
        }

        public bool IsSymbolDeclared(String identifier, String module = null)
        {
            return GetSymbol(identifier, module) != null;
        }

        public virtual bool IsSymbolConstant(String identifier, String module = null)
        {
            if (module != null) {
                return Parent != null ? Parent.IsSymbolConstant(identifier, module) : false;
            } else {
                return _symbols.ContainsKey(identifier)
                    ? _constants.Contains(identifier) : HasParent
                    ? Parent.IsSymbolConstant(identifier) : false;
            }
        }

        public virtual OberonType GetType(String identifier, String module = null)
        {
            OberonType type;
            if (module != null) {
                type = Parent != null ? Parent.GetType(identifier, module) : null;
            } else {
                type = _types.ContainsKey(identifier)
                    ? _types[identifier] : HasParent
                    ? Parent.GetType(identifier) : null;
            }

            if (type != null && !type.IsResolved) {
                type.Resolve(this);
            }

            return type;
        }

        public virtual OberonType GetSymbol(String identifier, String module = null)
        {
            OberonType type;
            if (module != null) {
                type = Parent != null ? Parent.GetSymbol(identifier, module) : null;
            } else {
                type = _symbols.ContainsKey(identifier)
                    ? _symbols[identifier] : HasParent
                    ? Parent.GetSymbol(identifier) : null;
            }

            if (type != null && !type.IsResolved) {
                type.Resolve(this);
            }

            return type;
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
            DeclareSymbol(type.Identifier, type);
            return scope;
        }

        public override bool IsSymbolConstant(string identifier, string module = null)
        {
            return module != null && _modules.ContainsKey(module)
                ? _modules[module].IsSymbolConstant(identifier)
                : base.IsSymbolConstant(identifier);
        }

        public override OberonType GetType(string identifier, string module = null)
        {
            return module != null && _modules.ContainsKey(module)
                ? _modules[module].GetType(identifier)
                : base.GetType(identifier);
        }

        public override OberonType GetSymbol(string identifier, string module = null)
        {
            return module != null && _modules.ContainsKey(module)
                ? _modules[module].GetSymbol(identifier)
                : base.GetSymbol(identifier);
        }
    }
}
