namespace Game.Core {
    using Data;
    using Map.Run;
    using Map.Run.UI;
    using Reward;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [RequireComponent(typeof(GameManager))]
    [RequireComponent(typeof(RewardSelectionUI))]
    [RequireComponent(typeof(EncounterRewardGenerator))]
    public class RunManager : MonoBehaviour {
        [SerializeField] private MapRunRender mapRunRender;
        private GameManager _gameManager;
        private EncounterRewardGenerator _rewardGenerator;
        private RewardSelectionUI _rewardSelectionUI;
        private RunState _runState;

        private void Awake() {
            this._gameManager = this.GetComponent<GameManager>();
            this._rewardSelectionUI = this.GetComponent<RewardSelectionUI>();
            this._rewardGenerator = this.GetComponent<EncounterRewardGenerator>();
        }

        private void Start() {
            this._runState = new RunState();
            this.mapRunRender.InitMap(this._runState.RunGraph);
            this.mapRunRender.OnSelect += this.OnSelectUINode;
            this._gameManager.OnBattleEnd += this.OnBattleEnd;
            this._rewardSelectionUI.OnRewardSelected += this.OnEndRewardSelect;
            this.mapRunRender.ShowMap();
        }

        private void OnSelectUINode(RunNode node) => this._gameManager.StartMap(node);

        private void OnBattleEnd(BattleResult result) {
            if (result.Winner == BattleTeam.Player) {
                bool hasNextMap = this.mapRunRender.HasNext();
                if (!hasNextMap) {
                    SceneManager.LoadScene("WinScene");
                }
                else {
                    this._rewardSelectionUI.Show(
                        this._rewardGenerator.GenerateRewards(this._runState.RunGraph.CurrentNode));
                }
            }
            else {
                SceneManager.LoadScene("LooseScene");
            }
        }

        private void OnEndRewardSelect(IReward reward) {
            this._rewardSelectionUI.Hide();
            reward.ApplyReward(RunData.GetInstance());
            this.mapRunRender.ShowMap();
        }
    }
}
