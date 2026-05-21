namespace Game.Battle.Map {
    using Data;
    using UnityEngine;

    public class BattleMapCameraController : MonoBehaviour {
        [SerializeField] private WorldRender worldRender;
        [SerializeField] private Camera mainCamera;

        [Header("Viewport")] [SerializeField] [Range(0.1f, 1f)]
        private float topViewportHeightRatio = 0.75f;

        [Header("Zoom")] [SerializeField] private float minZoom = 3f;

        [SerializeField] private float maxZoom = 10f;
        [SerializeField] private float keyboardZoomStep = 0.5f;
        [SerializeField] private float scrollZoomStep = 0.5f;

        private bool isBattleOn;

        private void Awake() {
            float bottomReservedRatio = 1f - this.topViewportHeightRatio;
            this.mainCamera.rect = new Rect(0f, bottomReservedRatio, 1f, this.topViewportHeightRatio);
        }

        private void Update() {
            if (!this.isBattleOn) {
                return;
            }

            this.HandleKeyboardZoomInput();
            this.HandleMouseScrollZoomInput();
        }

        public void StartBattle() => this.isBattleOn = true;

        public void EndBattle() => this.isBattleOn = false;

        public void CenterCameraOnMap(GridPosition centerMapPosition) {
            Vector3 centerMap = this.worldRender.GridToWorld(centerMapPosition);
            this.mainCamera.transform.position =
                new Vector3(centerMap.x, centerMap.y, this.mainCamera.transform.position.z);
        }

        private void HandleKeyboardZoomInput() {
            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) {
                this.ApplyZoom(-this.keyboardZoomStep);
            }

            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) {
                this.ApplyZoom(this.keyboardZoomStep);
            }
        }

        private void HandleMouseScrollZoomInput() {
            if (!this.IsMouseInsideBattleViewport()) {
                return;
            }

            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Approximately(scroll, 0f)) {
                return;
            }

            this.ApplyZoom(-scroll * this.scrollZoomStep);
        }

        private bool IsMouseInsideBattleViewport() {
            float bottomReservedRatio = 1f - this.topViewportHeightRatio;
            float minY = Screen.height * bottomReservedRatio;
            return Input.mousePosition.y >= minY;
        }

        private void ApplyZoom(float zoomDelta) {
            float nextZoom = this.mainCamera.orthographicSize + zoomDelta;
            this.mainCamera.orthographicSize = Mathf.Clamp(nextZoom, this.minZoom, this.maxZoom);
        }
    }
}
