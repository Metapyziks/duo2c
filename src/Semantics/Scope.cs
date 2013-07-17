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

        private Dictionary<String, OberonType> _definitions;

        public Scope(Scope parent = null)
        {
            Parent = parent;

            _definitions = new Dictionary<string,OberonType>();
        }

        public void Declare(String identifier, OberonType type)
        {
            _definitions.Add(identifier, type);
        }

        public bool IsDeclared(String identifier, String module = null)
        {
            return this[identifier, module] != null;
        }

        public virtual OberonType this[String identifier, String module = null]
        {
            get {
                OberonType type;
                if (module != null) {
                    type = Parent != null ? Parent[identifier, module] : null;
                } else {
                    type = _definitions.ContainsKey(identifier)
                        ? _definitions[identifier] : HasParent
                        ? Parent[identifier] : null;
                }

                if (type != null && !type.IsResolved) {
                    type.Resolve(this);
                }

                return type;
            }
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
            Declare(type.Identifier, type);
            return scope;
        }

        public override OberonType this[string identifier, string module = null]
        {
            get {
                return module != null && _modules.ContainsKey(module)
                    ? _modules[module][identifier]
                    : base[identifier];
            }
        }
    }
}
