﻿using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets._2D
{

    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jump;
		private bool action;
		public bool HasKey;
		public bool shoot;
		public Interactive InteractionObj;
		public float lastInteract;
		private const float InteractDelay = 0.75f;


        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if(!jump)
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
			if (!action)
				action = CrossPlatformInputManager.GetAxis("Vertical") >= 0.5f;
			shoot = CrossPlatformInputManager.GetButtonDown("Fire1");
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
			character.Move(h, jump, CrossPlatformInputManager.GetButton("Jump"));
            jump = false;
			character.Shoot(shoot);
			shoot = false;
			if (action) {
				if (InteractionObj != null) {
					if (Time.time > lastInteract + InteractDelay) {
						lastInteract = Time.time;
						InteractionObj.TryInteract(this);
					}
				}
				action = false;
			}
        }
    }
}