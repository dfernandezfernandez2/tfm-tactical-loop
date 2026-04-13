namespace Game.Battle.UI {
    using System.Collections.Generic;
    using global::Unit.Data;
    using TMPro;
    using Unit;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnitInfoPanelUI : MonoBehaviour {
        [SerializeField] private Image unitImage;
        [SerializeField] private GameObject unitStatsPanel;
        [SerializeField] private TMP_Text statTextPrefab;

        private readonly List<TMP_Text> _unitStats = new();

        private readonly StatType[] _statsFilter = {
            StatType.Hp, StatType.Atk, StatType.Def,
            StatType.Movement, StatType.Range, StatType.Speed,
            StatType.CritChance, StatType.Accuracy, StatType.Evasion,
            StatType.Mp, StatType.MpRegen
        };

        public void SetUnitInfo(UnitObject unitObject) {
            this.unitImage.sprite = unitObject.GetSprite();
            foreach (TMP_Text tmpText in this._unitStats) {
                Destroy(tmpText.gameObject);
            }
            foreach ((StatType type, float value) in unitObject.GetUnit().GetCurrentStats(this._statsFilter)) {
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
