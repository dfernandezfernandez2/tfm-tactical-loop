namespace Game.Core.Data {
    using System;
    using UnityEngine;

    [Serializable]
    public class GameData {
        private static GameData _instance;

        [SerializeField] private int units;

        public static GameData GetInstance() {
            _instance ??= JsonUtility.FromJson<GameData>(Resources.Load<TextAsset>("game_data").text);
            return _instance;
        }

        public int GetUnits() => this.units;
    }
}
