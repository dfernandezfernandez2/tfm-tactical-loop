namespace Unit.Data {
    using UnityEngine;

    [CreateAssetMenu(menuName = "Units/Unit Data")]
    public class UnitData : ScriptableObject {
        [Range(1, 50)] public int hp;
        [Range(0, 50)] public int mp;
        [Range(0, 5)] public int mpRegen;
        [Range(0, 10)] public int movement;
        [Range(1, 5)] public int ap;
        [Range(1, 10)] public int atk;
        [Range(1, 10)] public int defense;
        [Range(0, 5)] public int speed;
        [Range(1, 10)] public int range;
        [Range(0, 1)] public float accuracy;
        [Range(0, 1)] public float evasion;
        [Range(0, 1)] public float critChance;
    }
}
