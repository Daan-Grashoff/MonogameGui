using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monogamegui
{
	interface ElementType
	{
		GuiElement Visit(Func<EmptyButton, Label, Action, GuiElement> onButton, Func<Vector2, Texture2D, GuiElement> onEmptyButton, Func<Vector2, string, SpriteFont, GuiElement> onLabel);
	}

	class ButtonType : ElementType
	{
		Vector2 position;
		SpriteFont font;
		Texture2D texture;
		string text;
		Action action;

		public ButtonType(Vector2 position, SpriteFont font, Texture2D texture, string text, Action action)
		{
			this.position = position;
			this.font = font;
			this.texture = texture;
			this.text = text;
			this.action = action;
		}

		public GuiElement Visit(Func<EmptyButton, Label, Action, GuiElement> onButton, Func<Vector2, Texture2D, GuiElement> onEmptyButton, Func<Vector2, string, SpriteFont, GuiElement> onLabel)
		{
			return onButton(new EmptyButton(this.position, this.texture), new Label(this.position, this.text, this.font), this.action);
		}
	}

	class LabelType : ElementType
	{
		Vector2 position;
		string text;
		SpriteFont font;

		public LabelType(Vector2 position, string text, SpriteFont font)
		{
			this.position = position;
			this.text = text;
			this.font = font;
		}

		public GuiElement Visit(Func<EmptyButton, Label, Action, GuiElement> onButton, Func<Vector2, Texture2D, GuiElement> onEmptyButton, Func<Vector2, string, SpriteFont, GuiElement> onLabel)
		{
			return onLabel(this.position, this.text, this.font);
		}
	}

	class EmptyButtonType : ElementType
	{
		Vector2 position;
		Texture2D texture;

		public EmptyButtonType(Vector2 position, Texture2D texture)
		{
			this.position = position;
			this.texture = texture;
		}

		public GuiElement Visit(Func<EmptyButton, Label, Action, GuiElement> onButton, Func<Vector2, Texture2D, GuiElement> onEmptyButton, Func<Vector2, string, SpriteFont, GuiElement> onLabel)
		{
			return onEmptyButton(this.position, this.texture);
		}
	}


	interface Option<T>
	{
		U visit<U>(Func<U> onNone, Func<T, U> onSome);
		bool isSome();
		bool isNone();
	}

	class Some<T> : Option<T>
	{
		T value;

		public Some(T value)
		{
			this.value = value;
		}

		public bool isNone()
		{
			return false;
		}

		public bool isSome()
		{
			return true;
		}

		public U visit<U>(Func<U> onNone, Func<T, U> onSome)
		{
			return onSome(value);
		}
	}

	class None<T> : Option<T>
	{
		public bool isNone()
		{
			return true;
		}

		public bool isSome()
		{
			return false;
		}

		public U visit<U>(Func<U> onNone, Func<T, U> onSome)
		{
			return onNone();
		}
	}

}

