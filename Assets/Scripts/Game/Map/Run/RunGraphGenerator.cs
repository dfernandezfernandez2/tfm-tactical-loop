namespace Game.Map.Run {
    public static class RunGraphGenerator {
        public static RunGraph Generate() {
            RunNode startNode = new(EncounterType.Start, 0);
            RunNode firstNodeMap = new(EncounterType.Basic, 1);
            startNode.AddNextNode(firstNodeMap);
            RunNode secondLevelFirstNode = new(EncounterType.Elite, 2);
            RunNode secondLevelSecondNode = new(EncounterType.Basic, 2);
            RunNode secondLevelThirdNode = new(EncounterType.Basic, 2);
            firstNodeMap.AddNextNode(secondLevelFirstNode, secondLevelSecondNode, secondLevelThirdNode);
            RunNode thirdLevelFirstNode = new(EncounterType.Basic, 3);
            RunNode thirdLevelSecondNode = new(EncounterType.Elite, 3);
            RunNode thirdLevelThirdNode = new(EncounterType.Basic, 3);
            RunNode thirdLevelFourthNode = new(EncounterType.Elite, 3);
            secondLevelFirstNode.AddNextNode(thirdLevelFirstNode, thirdLevelSecondNode);
            secondLevelSecondNode.AddNextNode(thirdLevelSecondNode, thirdLevelThirdNode);
            secondLevelThirdNode.AddNextNode(thirdLevelThirdNode, thirdLevelFourthNode);
            RunNode bossNode = new(EncounterType.Boss, 4);
            thirdLevelFirstNode.AddNextNode(bossNode);
            thirdLevelSecondNode.AddNextNode(bossNode);
            thirdLevelThirdNode.AddNextNode(bossNode);
            thirdLevelFourthNode.AddNextNode(bossNode);
            RunNode endNode = new(EncounterType.End, 5);
            bossNode.AddNextNode(endNode);
            return new RunGraph(startNode);
        }
    }
}
