namespace Game.Map.Run {
    using System;
    using System.Collections.Generic;
    using Visitor;

    public class RunNode {
        private readonly Guid _id;
        public readonly EncounterType EncounterType;
        public readonly HashSet<RunNode> NextNodes = new();

        public RunNode(EncounterType encounterType) {
            this.EncounterType = encounterType;
            this._id = Guid.NewGuid();
        }

        public bool Completed { get; private set; }

        public void AddNextNode(params RunNode[] nextNodes) {
            foreach (RunNode nextNode in nextNodes) {
                this.NextNodes.Add(nextNode);
            }
        }

        public void Accept<TArg>(IRunNodeVisitor<TArg> visitor, TArg arg) => visitor.Visit(this, arg);

        public void Complete() => this.Completed = true;

        public override bool Equals(object obj) => obj is RunNode other && this._id == other._id;

        public override int GetHashCode() => this._id.GetHashCode();
    }
}
