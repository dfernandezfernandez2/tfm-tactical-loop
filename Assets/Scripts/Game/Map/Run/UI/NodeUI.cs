namespace Game.Map.Run.UI {
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class NodeUI : MonoBehaviour {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text label;
        [SerializeField] private Image background;

        [Header("Colors")] [SerializeField] private Color disabledColor = new(0.35f, 0.35f, 0.35f, 1f);

        [SerializeField] private Color enabledColor = new(1f, 1f, 1f, 1f);
        [SerializeField] private Color selectedColor = new(1f, 0.85f, 0.2f, 1f);

        [Header("Scale")] [SerializeField] private float normalScale = 1f;

        [SerializeField] private float selectedScale = 1.2f;

        private RunNode _node;

        public void Init(RunNode node) {
            this._node = node;
            this.label.text = node.EncounterType.ToString();

            this.button.interactable = false;
            this.button.onClick.RemoveAllListeners();
            this.button.onClick.AddListener(() => this.OnClick?.Invoke());

            this.UnSelect();
            this.Disable();
        }

        public void Select() {
            this.background.color = this.selectedColor;
            this.transform.localScale = Vector3.one * this.selectedScale;
            this.label.fontStyle = FontStyles.Bold;
        }

        public void UnSelect() {
            this.background.color = this.enabledColor;
            this.transform.localScale = Vector3.one * this.normalScale;
            this.label.fontStyle = FontStyles.Normal;
        }

        public void Enable() {
            this.button.interactable = true;
            this.background.color = this.enabledColor;
        }

        public void Disable() {
            this.button.interactable = false;
            this.background.color = this.disabledColor;
        }

        public event Action OnClick;
    }
}
