using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gun.Data
{
    [CreateAssetMenu(fileName = "RocketLauncherData", menuName = "ScriptableObjects/RocketLauncherDataScriptableObject", order = 2)]
    public class RocketLauncherData : GunData
    {
        [Tooltip("Enable area damage for the gun.")]
        public bool EnableAreaDamage = false;

        [Tooltip("The area damage radius start.")]
        public float AreaDamageRadiusStart = 0;

        [Tooltip("The area damage radius end.")]
        public float AreaDamageRadiusEnd = 0;

        [Tooltip("The area damage.")]
        public float AreaDamage = 50f;

        [Tooltip("The rocket launcher explosion on impact audio clip.")]
        public AudioClip ExplosionImpactAudioClip;
    }
}
