namespace Unit {
    using System.Collections;
    using Data;
    using Map;
    using UnityEngine;

    public class UnitObject : MonoBehaviour {
        [SerializeField] private UnitData data;

        private Unit _unit;
        private WorldRender _worldRender;

        public void Awake() {
            Stats stats = new Stats.Builder()
                .With(StatType.Hp, this.data.hp)
                .With(StatType.Mp, this.data.mp)
                .With(StatType.MpRegen, this.data.mpRegen)
                .With(StatType.Movement, this.data.movement)
                .With(StatType.AP, this.data.ap)
                .With(StatType.Atk, this.data.atk)
                .With(StatType.Def, this.data.defense)
                .With(StatType.Accuracy, this.data.accuracy)
                .With(StatType.Evasion, this.data.evasion)
                .With(StatType.CritChance, this.data.critChance)
                .With(StatType.Range, this.data.range)
                .With(StatType.Speed, this.data.speed)
                .Build();
            this._unit = new Unit(stats);
            this._unit.OnMove += this.OnMove;
            this._unit.OnDeath += this.OnDeath;
            this._unit.OnDamage += this.OnDamage;
            this._unit.OnHeal += this.OnHeal;
            this._unit.OnMiss += this.OnMiss;
            this._unit.OnHit += this.OnHit;
        }

        public void Init(GridPosition gridPosition, WorldRender worldRender) {
            this._worldRender = worldRender;
            this._unit.Move(gridPosition);
        }

        private void OnMove(GridPosition gridPosition) {
            Vector3 position = this._worldRender.GridToWorld(gridPosition);
            this.StartCoroutine(this.MoveRoutine(position));
        }

        private IEnumerator MoveRoutine(Vector3 target) {
            float
                duration = 0.2f; // todo: la duracion deberia ser en funcion de las posiciones o distancia, esto lo validamos en la tarea de movimiento
            float time = 0f;
            Vector3 start = this.transform.position;
            while (time < duration) {
                time += Time.deltaTime;
                this.transform.position = Vector3.Lerp(start, target, time / duration);
                yield return null;
            }

            this.transform.position = target;
        }

        private void OnDeath() {
        }

        private void OnDamage(int damage) {
        }

        private void OnHeal(int heal) {
        }

        private void OnMiss() {
        }

        private void OnHit(int i) {
        }
    }
}
