using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BBSItemBlinker
{
    public class PedBlinkerData
    {
        PedometerBlinker pedometerBlinker;
        public Material blinkerMat;
        public GameObject blinkerQuad;
        public PedBlinkerData(GameObject prefab)
        {
            pedometerBlinker = prefab.GetComponent<PedometerBlinker>();
            blinkerMat = pedometerBlinker.blinkerMat;
            blinkerQuad = pedometerBlinker.blinkerQuad;
        }
    }
}
