namespace Game.Controls {
    using UnityEngine;

    public static class GameInputLock {
        private static int _lockCount;

        public static bool IsLocked => _lockCount > 0;

        public static void Lock() => _lockCount++;

        public static void Unlock() => _lockCount = Mathf.Max(0, _lockCount - 1);

        public static void Clear() => _lockCount = 0;
    }
}
