using Il2Cpp;
using Il2CppFluffyUnderware.Curvy.Generator;
using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

[assembly: MelonInfo(typeof(BBSItemBlinker.Core), "BBSItemBlinker", "1.0.0", "Caleb Orchard", null)]
[assembly: MelonGame("DefaultCompany", "BabySteps")]

namespace BBSItemBlinker
{
    public class Core : MelonMod
    {
        PlayerMovement playerMovement;
        PedBlinkerData pedBlinkerData;
        List<Grabable> blinkifiedGrabables = new List<Grabable>();
        public override void OnLateUpdate()
        {
            if (playerMovement == null) 
            {
                var dudest = GameObject.Find("Dudest");
                if (dudest == null) return;
                playerMovement = dudest.GetComponent<PlayerMovement>();
                return;
            }

            if (pedBlinkerData == null) { MelonCoroutines.Start(WaitForBlinkerPrefab()); return; }

            TryBlinkify(playerMovement.handItems[0]);
            TryBlinkify(playerMovement.handItems[1]);
            TryBlinkify(playerMovement.currentHat);
        }
        private void BlinkifyGrabable(Grabable grabable)
        {
            blinkifiedGrabables.Add(grabable);

            if (GrabableHasBlinker(grabable)) return;

            var blinker = grabable.gameObject.AddComponent<PedometerBlinker>();
            blinker.grabable = grabable;
            blinker.blinkerMat = pedBlinkerData.blinkerMat;
            blinker.blinkerQuad = GameObject.Instantiate(pedBlinkerData.blinkerQuad, grabable.transform);
            blinker.blinkerQuad.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            blinker.blinkerQuad.transform.localPosition = Vector3.zero;

            blinker.enabled = true;
        }
        void TryBlinkify(Grabable grab)
        {
            if (grab == null || blinkifiedGrabables.Contains(grab))
                return;
            BlinkifyGrabable(grab);
        }
        private bool GrabableHasBlinker(Grabable grabable) { return grabable.gameObject.GetComponent<PedometerBlinker>() != null; }
        private IEnumerator WaitForBlinkerPrefab()
        {
            var loader = GameObject.Find("BigManagerPrefab/GlobalObjectParent/Savables/Pedometer_Loader");
            if (loader == null) yield break;

            GlobalObjectLoader gol = loader.GetComponent<GlobalObjectLoader>();
            AssetReference assRef = gol.loadee;

            // Wait until the AssetReference gets a valid runtime key
            int safety = 0;
            while ((assRef == null || assRef.RuntimeKey?.ToString() == "Il2CppSystem.Object") && safety < 300)
            {
                yield return null;
                safety++;
            }

            if (assRef == null)
            {
                MelonLogger.Msg("Blinker loader AssetReference is still null!");
                yield break;
            }

            GameObject prefab = assRef.LoadAssetAsync<GameObject>().WaitForCompletion();
            if (prefab != null) pedBlinkerData = new PedBlinkerData(prefab);
        }
    }
}