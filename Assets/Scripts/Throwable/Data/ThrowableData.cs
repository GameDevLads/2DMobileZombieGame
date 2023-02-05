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
    }
}
