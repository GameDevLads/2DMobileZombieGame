using UnityEngine;

namespace Assets.Scripts.Throwable.Data
{
    [CreateAssetMenu(fileName = "ThrowableData", menuName = "ScriptableObjects/ThrowableDataScriptableObject", order = 1)]
    public class ThrowableData : ScriptableObject
    {
        [Tooltip("How far the throwable will travel when you throw it.")]
        public float ThrowDistance = 15f;

        [Tooltip("How fast the throwable rotates when you throw it.")]
        public float RotateSpeed = 500f;

        [Tooltip("How fast the throwable travels when you throw it.")]
        public float DistanceTravelSpeed = 0.5f;

        [Tooltip("How fast the throwable transitions between coming back to the player and being put into its local position.")]
        public float ThrowablePlayerTransitionSpeed = 0.2f;

        [Tooltip("Should the throwable weapon return to the player to be used again.")]
        public bool ShouldReturn = false;

        [Tooltip("Should the throwable weapon explode on impact or where it is thrown.")]
        public bool Explode = false;

        [Tooltip("This is the damage radius of the throwable object.")]
        public float DamageRadius = 1f;

        [Tooltip("This is the amount of damage that is dealt to enemies.")]
        public float Damage = 10f;

        [Tooltip("How much much damage do we apply in a certain amount of time.")]
        public float DamageTime = 1.5f;

        [Tooltip("The explosion damage.")]
        public float ExplosionDamage = 50f;

        [Tooltip("The explosion radius start.")]
        public float ExplosionRadiusStart = 1f;

        [Tooltip("The explosion radius end.")]
        public float ExplosionRadiusEnd = 3f;

        [Tooltip("The amount of time that we wait before the explosion occurs after the object stops moving.")]
        public float ExplosionCountDown = 2f;
    }
}