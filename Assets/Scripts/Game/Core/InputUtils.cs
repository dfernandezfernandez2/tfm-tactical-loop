namespace Game.Core {
    using UnityEngine;

    public static class InputUtils {
        public static Vector2Int GetInputDirection() =>
            IsRightSelected() ? Vector2Int.right :
            IsLeftSelected() ? Vector2Int.left :
            IsUpSelected() ? Vector2Int.up :
            IsDownSelected() ? Vector2Int.down :
            Vector2Int.zero;

        public static bool IsEnterSelected() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space);

        public static bool IsEnterClickSelected() => Input.GetMouseButtonDown(0);

        public static bool IsCancelSelected() => Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1);

        public static bool IsRightSelected() => Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);

        public static bool IsLeftSelected() => Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);

        public static bool IsUpSelected() => Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);

        public static bool IsDownSelected() => Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

        public static Vector3 GetClickPosition() => Input.mousePosition;

        public static bool IsSwapNextSelected() => Input.GetKeyDown(KeyCode.E) || Input.mouseScrollDelta.y > 0.5f;

        public static bool IsSwapPreviousSelected() => Input.GetKeyDown(KeyCode.Q) || Input.mouseScrollDelta.y < -0.5f;

        public static bool IsRestoreSelected() => Input.GetKeyDown(KeyCode.R);
    }
}
