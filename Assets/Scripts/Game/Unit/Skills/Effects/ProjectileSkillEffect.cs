namespace Game.Unit.Skills.Effects {
    using System.Collections;
    using Effect.Data;
    using Map.Battle;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Projectile")]
    public class ProjectileSkillEffect : SkillEffect {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed = 4f;
        [SerializeField] private float heightOffset = 0.45f;
        [SerializeField] private float forwardStartOffset = 0.35f;
        [SerializeField] private float distanceToStop = 0.05f;
        [SerializeField] private bool rotateTowardsTarget = true;
        [SerializeField] private float rotationOffset;
        [SerializeField] private WorldRender worldRender;
        [SerializeField] private string projectileSoundRelease = "";
        [SerializeField] private string projectileSoundHit = "";
        [SerializeField] private float destroyDelay = 0.05f;

        public override bool CanApply(UnitObject target) => true;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            Vector3 start = user.transform.position + (Vector3.up * this.heightOffset);
            Vector3 end = this.worldRender.GridToWorld(target) + (Vector3.up * this.heightOffset);

            Vector3 castDirection = end - start;

            if (castDirection.sqrMagnitude > 0.001f) {
                start += castDirection.normalized * this.forwardStartOffset;
            }

            if (!string.IsNullOrWhiteSpace(this.projectileSoundRelease)) {
                yield return user.EffectController.VisualController.PlaySound(new SoundEffectData {
                    Name = this.projectileSoundRelease,
                    Volume = 1f,
                    WaitUntilFinished = false
                });
            }

            GameObject projectile = Instantiate(
                this.projectilePrefab,
                start,
                Quaternion.identity
            );

            while (Vector3.Distance(projectile.transform.position, end) > this.distanceToStop) {
                Vector3 direction = end - projectile.transform.position;

                this.RotateProjectile(projectile, direction);

                projectile.transform.position = Vector3.MoveTowards(
                    projectile.transform.position,
                    end,
                    this.speed * Time.deltaTime
                );

                yield return null;
            }

            projectile.transform.position = end;

            if (!string.IsNullOrWhiteSpace(this.projectileSoundHit)) {
                yield return user.EffectController.VisualController.PlaySound(new SoundEffectData {
                    Name = this.projectileSoundHit,
                    Volume = 1f,
                    WaitUntilFinished = false
                });
            }

            if (this.destroyDelay > 0f) {
                yield return new WaitForSeconds(this.destroyDelay);
            }

            Destroy(projectile);
        }

        private void RotateProjectile(GameObject projectile, Vector3 direction) {
            if (!this.rotateTowardsTarget || direction.sqrMagnitude <= 0.001f) {
                return;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            projectile.transform.rotation = Quaternion.Euler(
                0f,
                0f,
                angle + this.rotationOffset
            );
        }
    }
}
