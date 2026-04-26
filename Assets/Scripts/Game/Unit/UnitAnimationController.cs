namespace Game.Unit {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Data;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class UnitAnimationController : MonoBehaviour {
        private static readonly int _isMoving = Animator.StringToHash("isMoving");

        private readonly Dictionary<string, int> _signalCounters = new();

        private Animator _animator;
        private UnitLayer _unitLayer;

        public void Awake() {
            this._animator = this.GetComponent<Animator>();
            this._unitLayer = new UnitLayer(
                new KeyValuePair<int, Vector2Int>(this._animator.GetLayerIndex("Down"), Vector2Int.down),
                new KeyValuePair<int, Vector2Int>(this._animator.GetLayerIndex("Up"), Vector2Int.up),
                new KeyValuePair<int, Vector2Int>(this._animator.GetLayerIndex("Right"), Vector2Int.right),
                new KeyValuePair<int, Vector2Int>(this._animator.GetLayerIndex("Left"), Vector2Int.left)
            );
        }

        public IEnumerator PlayAnimation(AnimationType animationType, Func<IEnumerator> onText = null) {
            string triggerName = animationType.ToString();
            yield return this.PlayAnimation(triggerName, onText);
        }

        public IEnumerator PlayAnimation(string triggerName, Func<IEnumerator> onText = null) {
            string endSignalName = "signal.end." + triggerName.ToLower();
            int endSignalVersion = this.GetSignalVersion(endSignalName);
            string textSignalName = "signal.text." + triggerName.ToLower();
            int textSignalVersion = this.GetSignalVersion(textSignalName);

            this.ResetTrigger(triggerName);
            this.SetTrigger(triggerName);
            if (onText != null) {
                yield return this.WaitForSignal(textSignalName, textSignalVersion);
                yield return onText();
            }

            yield return this.WaitForSignal(endSignalName, endSignalVersion);
        }

        public void SetMoving(bool value) => this._animator.SetBool(_isMoving, value);

        public void UpdateDirection(Vector2Int direction) {
            foreach (KeyValuePair<int, float> keyValuePair in this._unitLayer.GetChangeLayer(direction)) {
                this._animator.SetLayerWeight(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void AddSignal(string signalId) =>
            this._signalCounters[signalId] = this._signalCounters.GetValueOrDefault(signalId, 0) + 1;


        private void SetTrigger(string triggerName) => this._animator.SetTrigger(triggerName);
        private void ResetTrigger(string triggerName) => this._animator.ResetTrigger(triggerName);

        private IEnumerator WaitForSignal(string signalId, int version) {
            yield return new WaitUntil(() => this._signalCounters.GetValueOrDefault(signalId, 0) > version);
        }

        private int GetSignalVersion(string signalId) => this._signalCounters.GetValueOrDefault(signalId, 0);
    }
}
