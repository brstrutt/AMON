﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AMON
{
	enum PlayerCharacterState
	{
		PCS_NORMAL,
		PCS_WET,
		PCS_POWEREDUP
	}

	class PlayerCharacter : PhysicalObject
	{
		PlayerCharacterState playerState;
		Vector2 movementSpeed, wetMovementSpeed;
		Vector2 normalSize, wetSize;
		public float grenadeTimer;

		public PlayerCharacter(Vector2 spawnLocation) : base(spawnLocation, GraphicsManager.Instance.charFine)
		{
			playerState = PlayerCharacterState.PCS_NORMAL;
			movementSpeed = new Vector2(700, 300);
			normalSize = new Vector2(38, 46);
			wetMovementSpeed = new Vector2(400, 100);
			wetSize = new Vector2(26, 50);
			grenadeTimer = 0;
			UpdateCollider();
		}

		protected override void SpecifyCollidableTypes()
		{
			collidableTypes.Add(typeof(Missile));
			collidableTypes.Add(typeof(Plane));
			collidableTypes.Add(typeof(Cloud));
			//collidableTypes.Add(typeof(Powerup));
		}

		public override void ReactToCollision(PhysicalObject other)
		{			
			if(other is Cloud)
			{
				//try to switch to damp
			}
		}

		public override void ReactToCollisionEntry(PhysicalObject other)
		{
			//Look into IDictionary and the Visitor pattern as possible alternatives to these if/else statements
			if (other is Projectile)
			{
				AudioManager.Instance.PlayAudioClip(AudioManager.AUDIOCLIPS.PATHETIC);
				Destroy();
			}
			else if (other is Cloud)
			{
				AudioManager.Instance.PlayRandomPain();
			}
			/*else if(other is Powerup)
			{
				//switch to powered up state
			}*/
		}

		//WARNING: This method is never called right now. Its parent Tick is called instead because it's in a list of <parent object>
		public override void Tick(float deltaTime)
		{
			if(grenadeTimer > 0) grenadeTimer -= deltaTime;			

			base.Tick(deltaTime);
		}

		/// <summary>
		/// Update the object's velocity to control the player's movement.
		/// </summary>
		/// <param name="horizontalMovement"> Negative = left, Positive = right, Zero = no motion</param>
		/// <param name="verticalMovement"> Negative = up, Positive = down, Zero = no motion</param>
		public void MovePlayer(int horizontalMovement, int verticalMovement)
		{
			//convert the inputs to -1, 0, or +1 depending on their sign
			if (horizontalMovement != 0) horizontalMovement = horizontalMovement / Math.Abs(horizontalMovement);
			if (verticalMovement != 0) verticalMovement = verticalMovement / Math.Abs(verticalMovement);

			//update velocity
			switch(playerState)
			{
				case PlayerCharacterState.PCS_NORMAL:
				case PlayerCharacterState.PCS_POWEREDUP:
					velocity.X = movementSpeed.X * horizontalMovement;
					velocity.Y = movementSpeed.Y * verticalMovement;
					break;
				case PlayerCharacterState.PCS_WET:
					velocity.X = wetMovementSpeed.X * horizontalMovement;
					velocity.Y = wetMovementSpeed.Y * verticalMovement;
					break;
			}
		}

		public void DropGrenade()
		{
			if(grenadeTimer <= 0)
			{
				grenadeTimer = 2;
				GameWorld.Instance.AddObject(new Grenade(this.GetCentre()));
			}
		}

		private void UpdateCollider()
		{
			if(playerState == PlayerCharacterState.PCS_WET)
			{
				dimensions = wetSize;
			}
			else
			{
				dimensions = normalSize;
			}
		}

		public override void Destroy()
		{
			GameWorld.Instance.EndGame(false);
			base.Destroy();
		}
	}
}
