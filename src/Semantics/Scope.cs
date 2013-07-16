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
                if (module != null) {
                    return Parent != null ? Parent[identifier, module] : null;
                } else {
                    return _definitions.ContainsKey(identifier)
                        ? _definitions[identifier] : HasParent
                        ? Parent[identifier] : null;
                }
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

        public Scope CreateModuleScope(String identifier)
        {
            var scope = new Scope(this);
            _modules.Add(identifier, scope);
            return scope;
        }

        public override OberonType this[string identifier, string module = null]
        {
            get {
                return _modules.ContainsKey(module) ? _modules[module][identifier] : null;
            }
        }
    }
}
