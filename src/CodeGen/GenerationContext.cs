using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;

namespace DUO2C.CodeGen
{
    public delegate String LazyString();

    public class GenerationContext
    {
        private abstract class Column
        {
            public abstract void Write(String format, params Object[] args);
        }

        private class BuilderColumn : Column
        {
            private StringBuilder _str;

            public BuilderColumn()
            {
                _str = new StringBuilder();
            }

            public override void Write(string format, params object[] args)
            {
                if (args.Length == 0) {
                    _str.Append(format);
                } else {
                    _str.AppendFormat(format, args);
                }
            }

            public override string ToString()
            {
                return _str.ToString();
            }
        }

        private class LazyColumn : Column
        {
            private LazyString _func;

            public LazyColumn(LazyString func)
            {
                _func = func;
            }

            public override void Write(string format, params object[] args)
            {
                var old = _func;
                _func = () => old() + String.Format(format, args);
            }

            public override string ToString()
            {
                return _func();
            }
        }

        private class Line
        {
            private List<Column> _columns;

            public bool IsEmpty
            {
                get { return _columns.Count == 1 && _columns.First().ToString().Length == 0; }
            }

            public Line()
            {
                _columns = new List<Column>();
                _columns.Add(new BuilderColumn());
            }

            public void Write(String format, params object[] args)
            {
                _columns.Last().Write(format, args);
            }

            public void Write(LazyString func)
            {
                var old = _columns.Last();
                _columns[_columns.Count - 1] = new LazyColumn(() => old + func());
            }

            public void Anchor()
            {
                _columns.Add(new BuilderColumn());
            }

            public int GetColumnCount()
            {
                return _columns.Count - 1;
            }

            public int GetColumnWidth(int col)
            {
                return _columns.Count > col + 1 ? _columns[col].ToString().Length : 0;
            }

            public void PadColumn(int col, int width)
            {
                if (_columns.Count > col + 1) {
                    var column = _columns[col] as BuilderColumn;
                    while (column != null && column.ToString().Length < width) {
                        column.Write(" ");
                    }
                }
            }

            public override string ToString()
            {
                return String.Join("", _columns);
            }
        }

        private class Group
        {
            private int _indent;
            private Group _parent;
            private List<Line> _lines;

            public Group()
            {
                _parent = null;
                _indent = 0;
                _lines = new List<Line>();
                _lines.Add(new Line());
            }

            public Group(Group parent, int indent)
            {
                _parent = parent;
                _indent = indent;
                _lines = new List<Line>();
                _lines.Add(new Line());
            }

            public Group Write(String format, params object[] args)
            {
                _lines.Last().Write(format, args);
                return this;
            }

            public Group Write(LazyString func)
            {
                _lines.Last().Write(func);
                return this;
            }

            public Group Anchor()
            {
                _lines.Last().Anchor();
                return this;
            }

            public Group NewLine()
            {
                _lines.Add(new Line());
                return this;
            }

            public Group Enter(int indent)
            {
                return new Group(this, indent);
            }

            private void AlignColumns()
            {
                var columns = _lines.Max(x => x.GetColumnCount());
                for (int i = 0; i < columns; ++i) {
                    var width = _lines.Max(x => x.GetColumnWidth(i));
                    foreach (var line in _lines) line.PadColumn(i, width);
                }
            }

            public Group Leave()
            {
                AlignColumns();
                foreach (var line in _lines) {
                    if (line == _lines.Last() && line.IsEmpty) break;
                    var str = line.ToString();
                    if (str.Length == 0) {
                        _parent.NewLine();
                    } else {
                        if (str[0] != '\r') {
                            for (int i = 0; i < _indent; ++i) _parent.Write(" ");
                        }
                        
                        _parent.Write(str).NewLine();
                    }
                }
                return _parent;
            }

            public override string ToString()
            {
                AlignColumns();
                return String.Join(Environment.NewLine, _lines).Replace("\n\r", "\n");
            }
        }

        private Group _curGroup;

        public String GeneratedCode
        {
            get { return _curGroup.ToString(); }
        }

        public GenerationContext()
        {
            _curGroup = new Group();
        }

        public GenerationContext NewLine()
        {
            _curGroup.NewLine();
            return this;
        }

        public GenerationContext Write(String format, params Object[] args)
        {
            _curGroup.Write(format, args);
            return this;
        }

        public GenerationContext Write(LazyString func)
        {
            _curGroup.Write(func);
            return this;
        }

        public GenerationContext Anchor()
        {
            _curGroup.Anchor();
            return this;
        }

        public GenerationContext Enter(int indent = 4)
        {
            _curGroup = _curGroup.Enter(indent);
            return this;
        }

        public GenerationContext Leave()
        {
            _curGroup = _curGroup.Leave();
            return this;
        }
    }
}
