using UnityEngine;

namespace Assets.Scripts.Collectable
{
    public class XPItem : MonoBehaviour
    {
        [HideInInspector] public XPItemSpawner Spawner;
        public FloatVariableSO xpAmountSO;
        public FloatVariableSO xpValue;
        private void OnTriggerEnter2D(Collider2D other)
        {
            // We do not need to check for Player because the xp Tag only collides with the player
            xpAmountSO.Value += xpValue.Value;
            Spawner.DespawnXPItem(gameObject);
        }
    }

}