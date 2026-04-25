namespace Game.Effect {
    using System.ComponentModel;
    using UnityEngine;

    public enum EffectTextureType {
        CircleTexture,
        SoftCircleTexture
    }

    public static class EffectTextureFactory {
        public static Texture2D GetTexture(EffectTextureType textureType, int size) =>
            textureType switch {
                EffectTextureType.CircleTexture => CreateCircleTexture(size),
                EffectTextureType.SoftCircleTexture => CreateSoftCircleTexture(size),
                _ => throw new InvalidEnumArgumentException(nameof(textureType))
            };

        private static Texture2D CreateCircleTexture(int size = 32) {
            Texture2D texture = new(size, size, TextureFormat.RGBA32, false);
            Color clear = new(1f, 1f, 1f, 0f);
            Vector2 center = new(size / 2f, size / 2f);
            float radius = (size / 2f) - 1f;
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    float alpha = Mathf.Clamp01(1f - ((distance - (radius * 0.75f)) / (radius * 0.25f)));
                    texture.SetPixel(x, y, distance <= radius ? new Color(1f, 1f, 1f, alpha) : clear);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateSoftCircleTexture(int size = 64) {
            Texture2D texture = new(size, size, TextureFormat.RGBA32, false);

            Color clear = new(1f, 1f, 1f, 0f);
            Vector2 center = new(size / 2f, size / 2f);
            float radius = (size / 2f) - 1f;
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    Vector2 pixel = new(x, y);
                    float distance = Vector2.Distance(pixel, center);
                    float normalizedDistance = distance / radius;

                    float alpha = Mathf.Clamp01(1f - normalizedDistance);
                    alpha *= alpha;

                    Color color = distance <= radius
                        ? new Color(1f, 1f, 1f, alpha)
                        : clear;

                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }
    }
}
