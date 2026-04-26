namespace Game.Map.Run {
    using System;
    using System.Collections.Generic;
    using Visitor;

    public class RunNode {

        public readonly Guid Id;
        public readonly EncounterType EncounterType;
        public readonly HashSet<RunNode> NextNodes = new();
        public readonly HashSet<RunNode> PreviousNodes = new();
        public bool Completed { get; private set; }

        public RunNode(EncounterType encounterType) {
            this.EncounterType = encounterType;
            this.Id = Guid.NewGuid();
        }

        public void AddNextNode(params RunNode[] nextNodes) {
            foreach (RunNode nextNode in nextNodes) {
                this.NextNodes.Add(nextNode);
                nextNode.PreviousNodes.Add(this);
            }
        }

        public void Accept<TArg>(IRunNodeVisitor<TArg> visitor, TArg arg) {
            visitor.Visit(this, arg);
        }

        public void Complete() => this.Completed = true;

        public override bool Equals(object obj) => obj is RunNode other && this.Id == other.Id;

        public override int GetHashCode() => this.Id.GetHashCode();
    }
}
