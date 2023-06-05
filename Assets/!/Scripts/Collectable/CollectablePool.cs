using Assets.__.Scripts.Interfaces;
using System.Linq;
using UnityEngine;

namespace Assets.__.Scripts.Collectable
{
    public class CollectablePool : MonoBehaviour, ICollectablePool
    {
        [Tooltip("The list of all possible collectables that will be dropped.")]
        public GameObject[] Collectables;

        /// <summary>
        /// Method used to drop a collectable that has been added to the collectable pool.
        /// </summary>
        public void DropCollectable()
        {
            if (Collectables == null || Collectables.Count() == 0)
                return; // No collectables to drop.

            foreach (var collectable in Collectables)
            {
                var collectableComponent = collectable.GetComponent<ICollectable>();
                if (collectableComponent != null)
                {
                    Instantiate(collectable, this.gameObject.transform.position, this.gameObject.transform.rotation);                    
                }
            }
        }
    }
}