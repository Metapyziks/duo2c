using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DUO2C.Nodes;

namespace DUO2C.CodeGen
{
    public class GenerationContext
    {
        private class Line
        {
            private List<StringBuilder> _columns;

            public bool IsEmpty
            {
                get { return _columns.Count == 1 && _columns.First().Length == 0; }
            }

            public Line()
            {
                _columns = new List<StringBuilder>();
                _columns.Add(new StringBuilder());
            }

            public void Write(String format, params object[] args)
            {
                if (args.Length == 0) {
                    _columns.Last().Append(format);
                } else {
                    _columns.Last().AppendFormat(format, args);
                }
            }

            public void Anchor()
            {
                _columns.Add(new StringBuilder());
            }

            public int GetColumnCount()
            {
                return _columns.Count - 1;
            }

            public int GetColumnWidth(int col)
            {
                return _columns.Count > col + 1 ? _columns[col].Length : 0;
            }

            public void PadColumn(int col, int width)
            {
                if (_columns.Count > col + 1) {
                    var column = _columns[col];
                    while (column.Length < width) {
                        column.Append(" ");
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

            private void Finalize()
            {
                var columns = _lines.Max(x => x.GetColumnCount());
                for (int i = 0; i < columns; ++i) {
                    var width = _lines.Max(x => x.GetColumnWidth(i));
                    foreach (var line in _lines) line.PadColumn(i, width);
                }
            }

            public Group Leave()
            {
                Finalize();
                foreach (var line in _lines) {
                    if (line == _lines.Last() && line.IsEmpty) break;
                    for (int i = 0; i < _indent; ++i) _parent.Write(" ");
                    _parent.Write(line.ToString()).NewLine();
                }
                return _parent;
            }

            public override string ToString()
            {
                Finalize();
                return String.Join(Environment.NewLine, _lines);
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

        public GenerationContext Write(SubstituteNode node)
        {
            node.GenerateCode(this);
            return this;
        }

        public GenerationContext Write(String format, params Object[] args)
        {
            _curGroup.Write(format, args);
            return this;
        }

        public void Anchor()
        {
            _curGroup.Anchor();
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
