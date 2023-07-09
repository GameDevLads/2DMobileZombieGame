using Assets.__.Scripts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.__.Scripts.Collectable
{
    public class CollectablePool : MonoBehaviour, ICollectablePool
    {
        [Tooltip("The list of all possible collectables that will be dropped.")]
        public List<GameObject> Collectables;

        [Tooltip("Rule used to enable/disable dropping of collectable items.")]
        public bool EnableDrops = true;

        [Tooltip("The minimum drop percentage to use when generating random number.")]
        public float MinDropPercentage = 0f;

        [Tooltip("The maximum drop percentage to use when generating random number.")]
        public float MaxDropPercentage = 1f;

        /// <summary>
        /// Method used to drop a collectable that has been added to the collectable pool.
        /// </summary>
        public void DropCollectable()
        {
            if (Collectables == null || Collectables.Count() == 0 || !EnableDrops)
                return; // No collectables to drop.

            var collectable = GetCollectable();

            if (collectable == null)
                return;

            Instantiate(collectable, this.gameObject.transform.position, this.gameObject.transform.rotation);
        }

        /// <summary>
        /// Gets a collectable based on the configured drop chance percentage.
        /// </summary>
        /// <returns></returns>
        private GameObject GetCollectable()
        {
            var randomNumber = Random.Range(MinDropPercentage, MaxDropPercentage);

            foreach (var groupedCollectable in Collectables
                .Where(x => x.GetComponent<ICollectable>() != null)
                .OrderBy(x => x.GetComponent<ICollectable>().DropChancePercentage.Value)
                .GroupBy(x => x.GetComponent<ICollectable>().DropChancePercentage.Value))
            {
                if (randomNumber > groupedCollectable.Key)
                    continue; // The collectable drop percentage does not fall within the random num range.

                var collectableCount = groupedCollectable.Count();

                if (collectableCount > 1)
                { // As we have multiple collectables with the same percentage, drop a random one within that group.
                    var randomItem = new System.Random().Next(0, collectableCount);
                    return groupedCollectable.ElementAt(randomItem);
                }
                else
                { // Get the first ICollectable as we only have one with the same percentage
                    return groupedCollectable.FirstOrDefault();
                }
            }

            return null;
        }
    }
}