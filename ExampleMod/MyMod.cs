using System;
using System.Linq;
using UnityEngine;

namespace ExampleMod {
    /// <summary>
    /// This is the Upkeep Mod.
    /// It adds some difficulty to the game by periodically taking a certain amount of concrete as an upkeep cost per building.
    /// </summary>
    [System.Serializable]
    public class MyMod : Mod {
        // How much concrete does upkeep cost per building (multiplied by 100 due to how resources work)
        private int upkeepCostPerBuilding = 100;
        // How frequently we want the upkeep to be taken (default 10 sec)
        private float upkeepFrequency = 10f;
        // When is the next upkeep "payment"
        private float nextUpkeepTime = 0f;

        public override void Load() {
            MonoBehaviour.print("Loading the Upkeep Mod.");
        }

        public override void Start() {
            // Set the first time the upkeep has to be paid
            nextUpkeepTime = Time.time + upkeepFrequency;
        }

        public override void FrameUpdate() {
            if (nextUpkeepTime <= Time.time) {
                int numberOfBuildings = GetNumberOfBuildings();
                int upkeepCost = numberOfBuildings * upkeepCostPerBuilding;
                Resource concreteResource = Resource.Get("concrete");

                // Create a new resource cost list for the upkeep (needs to be a list or array because it could cost multiple resources at once)
                ResourceCost[] cost = new[] { new ResourceCost(concreteResource, upkeepCost) };
                WorldScripts.Inst.resourceManager.Pay(cost);
                MonoBehaviour.print($"Upkeep of {upkeepCost} concrete has been paid for {numberOfBuildings} buildings!");

                // Set when the upkeep is to be paid next
                nextUpkeepTime += upkeepFrequency;
            }
        }

        public override void SimulationUdpate() {
        }

        private int GetNumberOfBuildings() {
            // Count all of the nodes in the city graph that are buildings (nodes can be other things too, like intersections)
            return WorldScripts.Inst.simulator.nodes.Count(x => x is Building);
        }
    }
}
