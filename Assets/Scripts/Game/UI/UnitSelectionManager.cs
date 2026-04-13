namespace Game.UI {
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.Data;
    using global::Unit.Data;
    using TMPro;
    using Unit;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    internal struct AvailableUnitsType {
        public UnitSelectionUI UnitSelectionUI { get; }
        public UnitSelector UnitSelector { get; }

        public AvailableUnitsType(UnitSelectionUI unitSelectionUI, UnitSelector unitSelector) {
            this.UnitSelectionUI = unitSelectionUI;
            this.UnitSelector = unitSelector;
        }
    }

    public class UnitSelectionManager : MonoBehaviour {
        [Header("Available unit types")] [SerializeField]
        private UnitSelectionUI unitSelectionPrefab;

        [SerializeField] private GameObject unitsAvailablePanel;
        [SerializeField] private List<UnitObject> availableUnitTypes = new();

        [Header("Team selected section")] [SerializeField]
        private UnitSelectionUI unitTeamPrefab;

        [SerializeField] private GameObject teamChoosePanel;

        [Header("Confirm selection")] [SerializeField]
        private Button confirmButton;

        [Header("Unit info section")] [SerializeField]
        private GameObject unitStatsPanel;

        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private Image unitTypeImage;
        [SerializeField] private TMP_Text statTextPrefab;

        private readonly List<AvailableUnitsType> _availableUnits = new();
        private readonly List<UnitSelectionUI> _units = new();
        private readonly List<TMP_Text> _unitSelectedStats = new();
        private int _currentTeamIndex;

        private int _currentUnitIndex;

        private void Awake() {
            this.BuildAvailableUnits();
            this.BuildTeamUnits();
        }

        private void Start() {
            this.OnSelect(0);
            for (int i = 1; i < this._availableUnits.Count; i++) {
                this._availableUnits[i].UnitSelector.DoUnSelect();
            }
        }

        private void Update() {
            if (InputUtils.IsCancelSelected()) {
                this.confirmButton.interactable = false;
                this.RemoveLastUnitSelected();
            }
            else if (this.IsEnabledSelection()) {
                this.confirmButton.interactable = false;
                if (InputUtils.IsRightSelected()) {
                    this.MoveSelection(1);
                }
                else if (InputUtils.IsLeftSelected()) {
                    this.MoveSelection(-1);
                }
                else if (InputUtils.IsEnterSelected()) {
                    this.OnConfirm(this._currentUnitIndex);
                }
            }
            else {
                this.confirmButton.interactable = true;
                if (InputUtils.IsEnterSelected()) {
                    this.ConfirmSelections();
                }
            }
        }

        private void BuildAvailableUnits() {
            for (int i = 0; i < this.availableUnitTypes.Count; i++) {
                UnitObject unitObject = this.availableUnitTypes[i];
                UnitSelectionUI availableTypeUnit =
                    Instantiate(this.unitSelectionPrefab, this.unitsAvailablePanel.transform);
                availableTypeUnit.SetUnit(unitObject);
                UnitSelector unitSelector = availableTypeUnit.gameObject.GetComponentInChildren<UnitSelector>();
                unitSelector.Index = i;
                unitSelector.OnConfirm += this.OnConfirm;
                unitSelector.OnSelect += this.OnSelect;
                this._availableUnits.Add(new AvailableUnitsType(availableTypeUnit, unitSelector));
            }
        }

        private void BuildTeamUnits() {
            for (int i = 0; i < GameData.GetInstance().GetUnits(); i++) {
                UnitSelectionUI chooseUnit = Instantiate(this.unitTeamPrefab, this.teamChoosePanel.transform);
                this._units.Add(chooseUnit);
            }
        }

        private void MoveSelection(int direction) {
            int count = this._availableUnits.Count;
            int next = (this._currentUnitIndex + direction + count) % count;
            this.OnSelect(next);
        }

        public void ConfirmSelections() {
            List<UnitObject> unitObjects = this._units.Select(unit => unit.GetUnit()).ToList();
            Team selectedTeam = new(unitObjects, BattleTeam.Player);
            RunData.GetInstance().Team = selectedTeam;
            SceneManager.LoadScene("GameScene");
        }

        private void OnSelect(int index) {
            if (!this.IsEnabledSelection()) {
                return;
            }

            this._availableUnits[this._currentUnitIndex].UnitSelector.DoUnSelect();
            this._availableUnits[index].UnitSelector.DoSelect();
            this._currentUnitIndex = index;
            this.BuildUnitSelectedInfo();
        }

        private void OnConfirm(int index) {
            if (!this.IsEnabledSelection()) {
                return;
            }

            this._units[this._currentTeamIndex].SetUnit(this._availableUnits[index].UnitSelectionUI.GetUnit());
            this._currentTeamIndex++;
        }

        private bool IsEnabledSelection() => this._currentTeamIndex <= this._units.Count - 1;

        private void BuildUnitSelectedInfo() {
            UnitObject unitObject = this._availableUnits[this._currentUnitIndex].UnitSelectionUI.GetUnit();
            this.unitNameText.text = unitObject.GetName();
            this.unitTypeImage.sprite = unitObject.GetSprite();
            foreach (TMP_Text text in this._unitSelectedStats) {
                Destroy(text.gameObject);
            }

            this._unitSelectedStats.Clear();
            foreach ((StatType type, float value) in unitObject.GetStats()) {
                TMP_Text text = Instantiate(this.statTextPrefab, this.unitStatsPanel.transform);
                text.text = type + ": " + value.ToString("0.##");
                this._unitSelectedStats.Add(text);
            }
        }

        private void RemoveLastUnitSelected() {
            if (this._currentTeamIndex == 0) {
                return;
            }

            this._currentTeamIndex--;
            this._units[this._currentTeamIndex].SetUnit(null);
        }
    }
}
