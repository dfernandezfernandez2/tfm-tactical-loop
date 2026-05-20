namespace Game.Run.Map.UI {
    using System;
    using Data;
    using UnityEngine;
    using UnityEngine.UI;

    public class NodeUI : MonoBehaviour {
        [SerializeField] private Button button;
        [SerializeField] private Image background;
        [SerializeField] private Image nodeImage;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite selectedSprite;

        [SerializeField] private EncounterTypeNodeSet encounterTypeNodeSet;

        [SerializeField] private Color disabledColor = new(0.35f, 0.35f, 0.35f, 1f);
        [SerializeField] private Color enabledColor = new(0f, 1f, 0f, 1f);
        [SerializeField] private Color selectedColor = new(1f, 0.5f, 0f, 1f);

        [SerializeField] private float normalScale = 1f;
        [SerializeField] private float selectedScale = 1.2f;

        private MapNode _node;

        public void Init(MapNode node) {
            this._node = node;
            this.nodeImage.sprite = this.encounterTypeNodeSet.ToDict()[node.RunNode.EncounterType];
            this.button.interactable = false;
            this.button.onClick.RemoveAllListeners();
            this.button.onClick.AddListener(() => this.OnClick?.Invoke(this._node));

            this.UnSelect();
            this.Disable();
        }

        public void Select() {
            this.background.color = this.selectedColor;
            this.background.sprite = this.selectedSprite;
            this.nodeImage.color = this.selectedColor;
            this.transform.localScale = Vector3.one * this.selectedScale;
        }

        public void UnSelect() {
            this.background.sprite = this.defaultSprite;
            this.background.color = this.enabledColor;
            this.nodeImage.color = this.enabledColor;
            this.transform.localScale = Vector3.one * this.normalScale;
        }

        public void Enable() {
            this.button.interactable = true;
            this.background.color = this.enabledColor;
            this.nodeImage.color = this.enabledColor;
            this.background.sprite = this.defaultSprite;
        }

        public void Disable() {
            this.button.interactable = false;
            this.background.color = this.disabledColor;
            this.nodeImage.color = this.disabledColor;
            this.background.sprite = this.defaultSprite;
        }

        public event Action<MapNode> OnClick;
    }
}
