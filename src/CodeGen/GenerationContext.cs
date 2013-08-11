using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;

namespace DUO2C.CodeGen
{
    public abstract class GenerationUnit
    {
        public abstract void Build(String prefix, StringBuilder sb);

        public override string ToString()
        {
            var sb = new StringBuilder();
            Build(String.Empty, sb);
            return sb.ToString();
        }
    }

    public class GenerationContext : GenerationUnit
    {
        private class Line : GenerationUnit
        {
            private Func<String> _func;
            private String _str;

            public bool IsEmpty { get; private set; }

            public Line()
            {
                _func = () => String.Empty;
                _str = null;

                IsEmpty = true;
            }

            public void Write(String format, params Object[] args)
            {
                var old = _func;
                if (args.Length == 0) {
                    _func = () => old() + format;
                } else {
                    _func = () => old() + String.Format(format, args);
                }

                _str = null;
                IsEmpty = false;
            }

            public void Write(Func<String> lazy)
            {
                var old = _func;
                _func = () => old() + lazy();

                _str = null;
                IsEmpty = false;
            }

            public override void Build(String prefix, StringBuilder sb)
            {
                if (_str == null) _str = _func();

                if (_str.Length > 0 && _str[0] == '\r') {
                    sb.AppendLine(_str.Substring(1));
                } else {
                    sb.AppendLine(prefix + _str);
                }
            }

            public void AlignColumns(IEnumerable<int> widths)
            {
                if (_str == null) _str = _func();

                var sb = new StringBuilder();
                for (int i = 0, c = 0, n = 0; i < _str.Length; ++i) {
                    if (_str[i] == '\t') {
                        int width;
                        do {
                            width = widths.ElementAtOrDefault(n++);
                        } while (width - c > 8 && n < widths.Count());

                        while (c++ < width) sb.Append(" ");
                        c = 0;
                    } else {
                        ++c;
                        sb.Append(_str[i]);
                    }
                }
                _str = sb.ToString();
            }

            public IEnumerable<int> GetColumns()
            {
                // Temp
                yield break;

                if (_str == null) _str = _func();

                for (int i = 0, c = 0; i < _str.Length; ++i) {
                    if (_str[i] == '\t') {
                        if (c > 8) yield break;
                        yield return c;
                        c = 0;
                    } else {
                        ++c;
                    }
                }
            }
        }

        private class LazyContext : GenerationContext
        {
            private Action<GenerationContext> _action;

            public LazyContext(Action<GenerationContext> action)
            {
                _action = action;
            }

            public override void Build(String prefix, StringBuilder sb)
            {
                _action(this);
                base.Build(prefix, sb);
            }
        }

        private GenerationContext _parent;
        private String _linePrefix;

        private List<GenerationUnit> _units;

        protected bool CanWrite
        {
            get { return _units.Count > 0 && _units.Last() is Line; }
        }

        public GenerationContext()
            : this(null, String.Empty) { }

        private GenerationContext(GenerationContext parent, String linePrefix)
        {
            _parent = parent;
            _linePrefix = linePrefix;

            _units = new List<GenerationUnit>();
        }

        private void AppendUnit(GenerationUnit unit)
        {
            _units.Add(unit);
        }

        public GenerationContext Enter(int indent = 4)
        {
            return Enter(Enumerable.Range(0, indent).Aggregate("", (s, x) => s + " "));
        }

        public GenerationContext Enter(String linePrefix)
        {
            if (CanWrite && ((Line) _units.Last()).IsEmpty) {
                _units.RemoveAt(_units.Count - 1);
            }

            return new GenerationContext(this, linePrefix);
        }

        public GenerationContext Lazy(Action<GenerationContext> action)
        {
            if (CanWrite && ((Line) _units.Last()).IsEmpty) {
                _units.RemoveAt(_units.Count - 1);
            }

            _units.Add(new LazyContext(action));
            return this;
        }

        public GenerationContext Leave()
        {
            if (CanWrite && ((Line) _units.Last()).IsEmpty) {
                _units.RemoveAt(_units.Count - 1);
            }

            _parent.AppendUnit(this);
            return _parent;
        }

        public GenerationContext Write(String format, params Object[] args)
        {
            if (!CanWrite) AppendUnit(new Line());

            var line = (Line) _units.Last();
            line.Write(format, args);

            return this;
        }

        public GenerationContext Write(Func<String> lazy)
        {
            if (!CanWrite) AppendUnit(new Line());

            var line = (Line) _units.Last();
            line.Write(lazy);

            return this;
        }

        public GenerationContext Anchor()
        {
            return Write("\t");
        }

        public GenerationContext Ln()
        {
            AppendUnit(new Line());
            return this;
        }

        public override void Build(String prefix, StringBuilder sb)
        {
            prefix += _linePrefix;
            
            var cols = new List<int>();

            foreach (var line in from x in _units where x is Line select (Line) x) {
                int c = 0;
                foreach (var col in line.GetColumns()) {
                    if (cols.Count <= c) cols.Add(col);
                    else cols[c] = Math.Max(cols[c], col);
                    ++c;
                }
            }

            bool lastEmpty = false;
            foreach (var unit in _units) {
                if (unit is Line) {
                    var line = (Line) unit;
                    if (line.IsEmpty) {
                        if (lastEmpty) {
                            continue;
                        } else {
                            lastEmpty = true;
                        }
                    } else {
                        ((Line) unit).AlignColumns(cols);
                        lastEmpty = false;
                    }
                } else {
                    lastEmpty = false;
                }
                unit.Build(prefix, sb);
            }
        }
    }
}
