using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace monogamegui
{
	public interface Component
	{
		void update(float dt);
		void Draw(SpriteBatch spriteBatch);
	}

	public abstract class GuiElement : Component
	{
		public Vector2 position;
		
		public GuiElement (Vector2 position)
		{
			this.position = position;
		}

		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void update(float dt);
	}
	
	public class Label : GuiElement
	{
		public string text;
		SpriteFont font;
		Color textColor;

		public Label(Vector2 position, string text, SpriteFont font) : base(position)
		{
			this.text = text;
			this.font = font;
			this.position = position;
			this.textColor = Color.Black;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(font, text, position, textColor);
		}

		public override void update(float dt)
		{
			
		}
	}

	class Button : GuiElement
	{
		EmptyButton button;
		Label label;
		Action action;

		public Button(EmptyButton Button, Label label, Action action) : base(Button.position)
		{
			this.button = Button;
			this.label = label;
			this.action = action;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			this.button.Draw(spriteBatch);
			this.label.Draw(spriteBatch);
		}

		public override void update(float dt)
		{
			MouseState mouse = Mouse.GetState();
			if (mouse.LeftButton == ButtonState.Pressed && !this.button.is_clicked)
			{
				
				/// <summary>
				/// Button retina screen * 2
				/// </summary>
				int rs = 2;
				if (mouse.X * rs > position.X &&
					mouse.X * rs < position.X + this.button.texture.Bounds.Width * this.button.scale.X &&
				   mouse.Y * rs > position.Y &&
				    mouse.Y * rs < position.Y + this.button.texture.Bounds.Height * this.button.scale.Y)
				{
					this.button.is_clicked = true;

					action();
				}
			}

			if (this.button.is_clicked)
			{
				this.button.waiting_for_up = true;
				this.button.count_down = this.button.count_down + dt * .1f;
				if (this.button.count_down >= this.button.max_count_down)
				{
					if (this.button.waiting_for_up && mouse.LeftButton == ButtonState.Released)
					{
						this.button.waiting_for_up = false;	
						this.button.is_clicked = false;
						this.button.count_down = 0;
					}
				}

			}

		}
	}

	public class EmptyButton : GuiElement
	{
		public Texture2D texture;
		public Vector2 scale;
		public Boolean is_clicked { get; set; }
		public Boolean waiting_for_up;
		public float count_down;
		public float max_count_down;


		public EmptyButton(Vector2 position, Texture2D texture) : base(position)
		{
			this.texture = texture;
			this.scale = new Vector2(0.1f, 0.1f);
			max_count_down = 10f;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, position, scale:scale);
		}

		public override void update(float dt)
		{
			
		}
	}


}
