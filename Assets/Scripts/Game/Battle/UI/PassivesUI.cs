namespace Game.Battle.UI {
    using Passive;
    using Run.Data;
    using UnityEngine;

    public class PassivesUI : MonoBehaviour {
        [SerializeField] private GameObject passivesContainer;

        private void Awake() => RunData.GetInstance().OnPassiveAdded += this.OnPassiveAdd;

        private void OnPassiveAdd(Passive passive) =>
            Instantiate(passive.GetUIObject(), this.passivesContainer.transform);
    }
}
