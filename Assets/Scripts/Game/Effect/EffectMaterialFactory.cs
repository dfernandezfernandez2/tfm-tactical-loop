namespace Game.Effect {
    using System.ComponentModel;
    using UnityEngine;

    public enum EffectTextureType {
        CircleTexture,
        BufferTexture,
        DebufferTexture
    }

    public class EffectMaterialFactory : MonoBehaviour {

        [SerializeField] private Material circleMaterial;
        [SerializeField] private Material buffMaterial;
        [SerializeField] private  Material debuffMaterial;

        public Material GetMaterial(EffectTextureType textureType) =>
            textureType switch {
                EffectTextureType.CircleTexture => this.circleMaterial,
                EffectTextureType.BufferTexture => this.buffMaterial,
                EffectTextureType.DebufferTexture => this.debuffMaterial,
                _ => throw new InvalidEnumArgumentException(nameof(textureType))
            };
    }
}
