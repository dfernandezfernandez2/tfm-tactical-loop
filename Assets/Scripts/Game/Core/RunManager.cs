namespace Game.Core {
    using Data;
    using Map.Run;
    using Map.Run.UI;
    using UnityEngine;

    [RequireComponent(typeof(GameManager))]
    public class RunManager : MonoBehaviour {
        [SerializeField] private MapRunRender mapRunRender;
        private GameManager _gameManager;
        private RunState _runState;

        private void Awake() => this._gameManager = this.GetComponent<GameManager>();

        private void Start() {
            this._runState = new RunState();
            this.mapRunRender.InitMap(this._runState.RunGraph);
            this.mapRunRender.OnSelect += this.OnSelectUINode;
            this._gameManager.OnBattleEnd += this.OnBattleEnd;
            this.mapRunRender.ShowMap();
        }

        private void OnSelectUINode(RunNode node) => this._gameManager.StartMap(node);

        private void OnBattleEnd(BattleResult result) {
            if (result.Winner == BattleTeam.Player) {
                bool hasNextMap = this.mapRunRender.HasNext();
                if (!hasNextMap) {
                    // todo: go to win end game scene
                }
                else {
                    // show rewards at the end call
                    this.OnEndRewardSelect();
                }
            }
            // todo: go to game over
        }

        private void OnEndRewardSelect() => this.mapRunRender.ShowMap();
    }
}
