namespace Map.Battle.Renderer;

using Data;
using UnityEngine;

[System.Serializable]
public class TileRenderElement {

    [SerializeField] private TileType type;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;

    public TileType Type => this.type;
    public GameObject Prefab => this.prefab;
    public Transform Parent => this.parent;

}
