using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSZZGame.Character
{
    public abstract class FSMState_Character_StealthBase : FSMState_Character_Base
    {
        protected struct CharacterController_Settings
        {
            public Vector3 center;
            public float height;
            public float radius;
        }

        private CharacterController_Settings originalControllerSettings;

        // Sampling of paint
        private Texture2D pixelSampler;
        private Sprite blankSprite;

        public FSMState_Character_StealthBase()
        {
            pixelSampler = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            blankSprite = Sprite.Create(pixelSampler, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }

        public override void OnEnter()
        {
            originalControllerSettings = new CharacterController_Settings
            {
                center = playerController.center,
                height = playerController.height,
                radius = playerController.radius
            };

            playerController.height = 0.1f;
            playerController.radius = 0.1f;
            playerController.center = new Vector3(playerController.center.x, 0.05f, playerController.center.z);
        }

        public override void OnExit()
        {
            playerController.height = originalControllerSettings.height;
            playerController.radius = originalControllerSettings.radius;
            playerController.center = originalControllerSettings.center;
        }

        /*
        protected bool CheckValidStealthPosition(Vector3 up)
        {
            // Get the edge of the collider closest to the floor
            Vector3 position = playerController.transform.position - Vector3.Project(playerController.center, up);
            float rayDist = 2.0f;

            RaycastHit raycastHit;

            // Get the surface underneath us
            if (Physics.Raycast(position, -up, out raycastHit, rayDist))
            {
                Paintable paintable = raycastHit.transform.GetComponent<Paintable>();
                if (paintable)
                {
                    int x = (int)((float)paintable.textureSize * raycastHit.textureCoord.x);
                    int y = (int)((float)paintable.textureSize * raycastHit.textureCoord.y);

                    RenderTexture.active = paintable.rawmaskcolorTexture;
                    pixelSampler.ReadPixels(new Rect(x, (int)paintable.textureSize - y, 1, 1), 0, 0, true);
                    pixelSampler.Apply();
                }
                else // This surface is not paintable - no way we can stealth through it
                    return false;
            }
            else // No hit, we are in the air
                return true;
        }
        */
    }
}
