using HarmonyLib;
using System;
using UnityEngine;

namespace ExampleMod {
    [System.Serializable]
    public class HarmonyMod : Mod {
        public override void FrameUpdate() { 
        }

        public override void Load() {
            MonoBehaviour.print("Loading HarmonyMod...");
            // Use name of your mod as ID to avoid mixups with other mods
            Harmony harmony = new Harmony("HarmonyMod");
            // Unpach in case we already patched it once during this run
            harmony.UnpatchAll();
            // Applies all patches using annotations like [HarmonyPatch]
            harmony.PatchAll();
            MonoBehaviour.print("HarmonyMod loaded!");
        }

        public override void SimulationUdpate() {
        }

        public override void Start() {
        }
    }

    [HarmonyPatch(typeof(Spaceship))]
    public class NamePatch {

        [HarmonyPatch("HandlePartFinished")]
        public static void Prefix(SpaceshipPart repairPart) {
            if (repairPart.name == "cryoChambers") {
                // Add a hundred workers
                for (int i = 0; i < 100; i++) {
                    Worker worker = new Worker(null);
                    WorldScripts.Inst.simulator.market.persistent.starterWorkers.Add(worker);
                    WorldScripts.Inst.simulator.market.AddWorker(worker);
                }
            }
        }
    }
}
