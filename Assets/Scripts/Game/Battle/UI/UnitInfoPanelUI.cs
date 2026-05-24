namespace Game.Battle.UI {
    using System.Collections.Generic;
    using TMPro;
    using Unit;
    using Unit.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnitInfoPanelUI : MonoBehaviour {
        [SerializeField] private Image unitImage;
        [SerializeField] private GameObject unitStatsPanel;
        [SerializeField] private TMP_Text statTextPrefab;

        private readonly StatType[] _statsFilter = {
            StatType.Hp, StatType.Atk, StatType.Def,
            StatType.Movement, StatType.Range, StatType.Speed,
            StatType.CritChance, StatType.Accuracy, StatType.Evasion,
            StatType.Mp, StatType.MpRegen, StatType.AP
        };

        private readonly List<TMP_Text> _unitStats = new();

        private UnitObject _unit;

        public void SetUnitInfo(UnitObject unitObject) {
            this.unitImage.sprite = unitObject.GetSprite();
            this._unit = unitObject;
            this.BuildStats();
        }

        public void UpdateStats() => this.BuildStats();

        private void BuildStats() {
            foreach (TMP_Text tmpText in this._unitStats) {
                Destroy(tmpText.gameObject);
            }

            this._unitStats.Clear();
            foreach ((StatType type, float value) in this._unit.Unit.GetCurrentStats(this._statsFilter)) {
                TMP_Text text = Instantiate(this.statTextPrefab, this.unitStatsPanel.transform);
                text.text = type.GetName() + ": " + FormatValue(value);
                this._unitStats.Add(text);
            }
        }

        private static string FormatValue(float value) =>
            value % 1 != 0
                ? (value * 100).ToString("0.##") + "%"
                : value.ToString("0");
    }
}
