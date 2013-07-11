namespace DUO2C.Nodes.Oberon2
{
    public abstract class DesignatorOperation : SubstituteNode
    {
        public DesignatorOperation(ParseNode original, bool leaf = false, bool hasPayload = true)
            : base(original, leaf, hasPayload) { }
    }
}
