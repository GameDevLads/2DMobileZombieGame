using UnityEngine;

namespace Assets.Scripts.Collectable
{
    public class XPItemSpawner : MonoBehaviour
    {
        public ObjectPool XPPool;

        /// <summary>
        /// Spawns an XP item at the given position
        /// </summary>
        public void SpawnXPItem(Vector3 position)
        {
            GameObject xpItem = XPPool.GetObject();
            xpItem.transform.position = position;
            XPItem xpItemComponent = xpItem.GetComponent<XPItem>();
            xpItemComponent.Spawner = this;
        }

        /// <summary>
        /// Returns the XP item prefab to the pool
        /// </summary>
        public void DespawnXPItem(GameObject xpItem)
        {
            XPPool.ReturnObject(xpItem);
        }
    }

}