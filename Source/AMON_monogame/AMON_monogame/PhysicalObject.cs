﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AMON
{
	//This class represents anything with a physical presence ingame
	class PhysicalObject
	{
		private Rectangle drawDest;
		protected Vector2 dimensions;
		protected Vector2 position;
		protected Vector2 velocity;
		protected Texture2D sprite;
		protected Color drawColour;

		public PhysicalObject(Texture2D objectSprite)
		{
			Instantiate(new Vector2(), objectSprite);
		}

		public PhysicalObject(Vector2 SpawnLocation, Texture2D objectSprite)
		{
			Instantiate(SpawnLocation, objectSprite);
		}

		private void Instantiate(Vector2 SpawnLocation, Texture2D objectSprite)
		{
			velocity = new Vector2();
			drawDest = new Rectangle();

			position = SpawnLocation;
			sprite = objectSprite;
			dimensions = new Vector2(sprite.Bounds.Width, sprite.Bounds.Height);
			drawColour = Color.White;
		}

		public Vector2 GetCentre()
		{
			return new Vector2(position.X + dimensions.X/2.0f, position.Y/dimensions.Y/2.0f);
		}

		public virtual void Tick(float deltaTime)
		{
			position.X += velocity.X * deltaTime;
			position.Y += velocity.Y * deltaTime;
		}

		public bool Collided(PhysicalObject other)
		{
			//Check if the two rectangles overlap horizontally
			float overallWidth = (other.position.X + other.dimensions.X) - this.position.X;
			float combinedWidth = other.dimensions.X + this.dimensions.X;

			if (overallWidth < 0) return false;
			if (overallWidth > combinedWidth) return false;

			//Check if they overlap vertically
			float overallHeight = (other.position.Y + other.dimensions.Y) - position.Y;
			float combinedHeight = other.dimensions.Y + this.dimensions.Y;

			if (overallHeight < 0) return false;
			if (overallHeight > combinedHeight) return false;

			return true;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			drawDest.X = (int)(position.X + 0.5f);
			drawDest.Y = (int)(position.Y + 0.5f);
			drawDest.Width = (int)(dimensions.X + 0.5f);
			drawDest.Height = (int)(dimensions.Y + 0.5f);
			spriteBatch.Draw(sprite, drawDest, drawColour);
		}
	}
}
