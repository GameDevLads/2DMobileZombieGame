using Assets.__.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Collectable
{
    public class XPItem : MonoBehaviour, ICollectable
    {
        public FloatVariableSO xpAmountSO;
        public FloatVariableSO xpValue;

        [field: SerializeField]
        public FloatVariableSO DropChancePercentage { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                xpAmountSO.Value += xpValue.Value;
                Destroy(gameObject);
            }
        }
    }
}