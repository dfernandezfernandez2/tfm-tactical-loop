namespace Game.Map.Run {
    public static class RunGraphGenerator {
        public static RunGraph Generate() {
            RunNode startNode = new(EncounterType.Start);
            RunNode firstNodeMap = new(EncounterType.Basic);
            startNode.AddNextNode(firstNodeMap);
            RunNode secondLevelFirstNode = new(EncounterType.Elite);
            RunNode secondLevelSecondNode = new(EncounterType.Basic);
            RunNode secondLevelThirdNode = new(EncounterType.Basic);
            firstNodeMap.AddNextNode(secondLevelFirstNode, secondLevelSecondNode, secondLevelThirdNode);
            RunNode thirdLevelFirstNode = new(EncounterType.Basic);
            RunNode thirdLevelSecondNode = new(EncounterType.Elite);
            RunNode thirdLevelThirdNode = new(EncounterType.Basic);
            RunNode thirdLevelFourthNode = new(EncounterType.Elite);
            secondLevelFirstNode.AddNextNode(thirdLevelFirstNode, thirdLevelSecondNode);
            secondLevelSecondNode.AddNextNode(thirdLevelSecondNode, thirdLevelThirdNode);
            secondLevelThirdNode.AddNextNode(thirdLevelThirdNode, thirdLevelFourthNode);
            RunNode bossNode = new(EncounterType.Boss);
            thirdLevelFirstNode.AddNextNode(bossNode);
            thirdLevelSecondNode.AddNextNode(bossNode);
            thirdLevelThirdNode.AddNextNode(bossNode);
            thirdLevelFourthNode.AddNextNode(bossNode);
            RunNode endNode = new(EncounterType.End);
            bossNode.AddNextNode(endNode);
            return new RunGraph(startNode);
        }
    }
}
