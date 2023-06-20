using UnityEngine;

namespace Assets.Scripts.Collectable
{
    public class XPItemSpawner : MonoBehaviour
    {
        public XPPool XPPool;
        public void SpawnXPItem(Vector3 position)
        {
            GameObject xpItem = XPPool.GetObject();
            // randomize position slightly
            position.x += Random.Range(-2f, 2f);
            position.y += Random.Range(-2f, 2f);
            xpItem.transform.position = position;
            XPItem xpItemComponent = xpItem.GetComponent<XPItem>();
            //xpItemComponent.Spawner = this;
        }

        public void DespawnXPItem(GameObject xpItem)
        {
            XPPool.ReturnObject(xpItem);
        }
    }

}