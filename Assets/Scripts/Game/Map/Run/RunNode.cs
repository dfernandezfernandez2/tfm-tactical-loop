namespace Game.Map.Run {
    using System;
    using System.Collections.Generic;
    using Visitor;

    public class RunNode {
        public readonly Guid Id;
        public readonly EncounterType EncounterType;
        public readonly int Level;
        public readonly HashSet<RunNode> NextNodes = new();

        public RunNode(EncounterType encounterType, int level) {
            this.EncounterType = encounterType;
            this.Id = Guid.NewGuid();
            this.Level = level;
        }

        public bool Completed { get; private set; }

        public void AddNextNode(params RunNode[] nextNodes) {
            foreach (RunNode nextNode in nextNodes) {
                this.NextNodes.Add(nextNode);
            }
        }

        public void Accept<TArg>(IRunNodeVisitor<TArg> visitor, TArg arg) => visitor.Visit(this, arg);

        public void Complete() => this.Completed = true;

        public override bool Equals(object obj) => obj is RunNode other && this.Id == other.Id;

        public override int GetHashCode() => this.Id.GetHashCode();
    }
}
