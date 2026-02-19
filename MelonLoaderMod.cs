using MelonLoader;
using SLZ.Rig;
using UnityEngine;

namespace AlwaysSprinting
{
    public static class BuildInfo
    {
        public const string Name = "AlwaysSprinting";
        public const string Author = "Lanno";
        public const string Company = null;
        public const string Version = "0.1.0";
        public const string DownloadLink = null;
    }

    public class AlwaysSprinting : MelonMod
    {
        private ControllerRig rig;
        private PhysicsRig physicsRig;
        private PhysTorso physTorso;

        private bool setupComplete = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            rig = Object.FindObjectOfType<ControllerRig>();
            physicsRig = Object.FindObjectOfType<PhysicsRig>();
            physTorso = Object.FindObjectOfType<PhysTorso>();

            setupComplete = false;
        }

        // 🔧 Disable torso collisions but KEEP hands physical
        private void DisableBodyCollisions()
        {
            if (!physTorso) return;

            physTorso.rbChest.detectCollisions = false;
            physTorso.rbHead.detectCollisions = false;
            physTorso.rbPelvis.detectCollisions = false;
            physTorso.rbSpine.detectCollisions = false;

            // Optional stability tweaks
            physTorso.rbPelvis.drag = 0f;
            physTorso.rbPelvis.angularDrag = 0.05f;
        }

        // 🔧 Disable Bonelab stick locomotion but keep UI working
        private void DisableDefaultMovement()
        {
            if (!rig) return;

            rig.maxVelocity = 0f;
            rig._wasOverFlickThresh = false;
        }

        public override void OnUpdate()
        {
            if (!rig || !physicsRig || !physTorso)
                return;

            // Run one-time setup after rig is ready
            if (!setupComplete)
            {
                DisableBodyCollisions();
                setupComplete = true;
                MelonLogger.Msg("Gorilla base setup complete");
            }

            // Continuously suppress default locomotion
            DisableDefaultMovement();

            // 🚧 Gorilla push movement will go HERE next step
        }
    }
}
