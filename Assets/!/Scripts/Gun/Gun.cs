using Assets.Scripts.Gun.Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Gun
{
    public class Gun : GunBase
    {
        [Tooltip("Configurable gun data.")]
        public GunData GunData;

        protected override void StartBase()
        {
            InitData(GunData);
        }
    }
}