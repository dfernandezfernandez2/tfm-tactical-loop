namespace Game.Battle.UI {
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public enum CombatTextType {
        Crit, Hit, Miss, Heal
    }

    public static class CombatTextTypeExtensions {
        public static Color GetColor(this CombatTextType type) =>
            type switch {
                CombatTextType.Crit => Color.orange,
                CombatTextType.Hit => Color.azure,
                CombatTextType.Miss => Color.whiteSmoke,
                CombatTextType.Heal => Color.limeGreen,
                _ => Color.floralWhite
            };
    }

    public class CombatTextUI : MonoBehaviour {
        [SerializeField] private TMP_Text text;
        [SerializeField] private float duration = 1f;

        private void Awake() {
            Color color = this.text.color;
            color.a = 0f;
            this.text.color = color;
        }

        public void Init(string message, CombatTextType type) {
            this.text.text = message;
            this.text.color = type.GetColor();
            this.StartCoroutine(this.DoAnimation());
        }

        public void Init(CombatTextType type) => this.Init(type.ToString(), type);

        private IEnumerator DoAnimation() {
            float time = 0f;
            Color color = this.text.color;
            while (time < this.duration) {
                time += Time.deltaTime;
                color.a = 1f - (time / this.duration); // fade out
                this.text.color = color;
                yield return null;
            }
            color.a = 0f;
            this.text.color = color;
        }
    }
}
