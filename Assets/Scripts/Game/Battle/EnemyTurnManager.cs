namespace Game.Battle {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Actions;
    using IA;
    using Map;
    using TMPro;
    using Unit;
    using UnityEngine;

    public class EnemyTurnManager : MonoBehaviour {
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private TMP_Text actionText;

        private EnemyTurnController _enemyTurnController;

        private void Awake() => this._enemyTurnController = new EnemyTurnController(this.battleMapManager);

        public IEnumerator DoTurn(UnitObject currentTurnUnit, List<UnitObject> unitsTurnOrder,
            IBattleContext battleContext, Action updateStats) {
            this.ShowActionText();
            IReadOnlyList<DecisionResult> decisionResults = this._enemyTurnController.CalculateTurn(currentTurnUnit,
                unitsTurnOrder, currentTurnUnit.Actions.GetAllAvailableActions());
            foreach (DecisionResult decisionResult in decisionResults) {
                yield return new WaitForSeconds(0.5f);
                this.AppendActionText(currentTurnUnit, decisionResult);
                if (decisionResult.Action is WaitAction) {
                    yield return new WaitForSeconds(0.75f);
                    this.HideActionText();
                }

                yield return decisionResult.Action.DoEnemyAction(battleContext, currentTurnUnit, decisionResult,
                    this.battleMapManager);
                updateStats.Invoke();
            }

            this.HideActionText();
        }

        private void ShowActionText() {
            this.actionText.text = string.Empty;
            this.actionText.gameObject.SetActive(true);
        }

        private void HideActionText() {
            this.actionText.text = string.Empty;
            this.actionText.gameObject.SetActive(false);
        }

        private void AppendActionText(UnitObject unit, DecisionResult decisionResult) {
            if (this.actionText == null) {
                return;
            }

            if (!string.IsNullOrEmpty(this.actionText.text)) {
                this.actionText.text += "\n";
            }

            this.actionText.text += $"{unit.GetName()} does {decisionResult.Action.GetName()} action";
            if (decisionResult.TargetPosition == null) {
                return;
            }

            UnitObject target = this.battleMapManager.GetUnit(decisionResult.TargetPosition);
            if (target != null) {
                this.actionText.text += $" with target {target.GetName()}";
            }
        }
    }
}
