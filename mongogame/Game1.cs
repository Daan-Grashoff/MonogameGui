using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace monogamegui
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;



		List<ElementType> ElementTypes;

		//FormLoader loader;
		//GuiElementsFactory controlFactories;
		GuiElementsFactoryToGuiElements controls;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			IsMouseVisible = true;
			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			SpriteFont font;
			font = Content.Load<SpriteFont>("Arial");
			Texture2D texture;
			texture = Content.Load<Texture2D>("button");



			ElementTypes = new List<ElementType>();

			ElementTypes.Add(new LabelType(new Vector2(0, 300), "this is a label", font));
			ElementTypes.Add(new LabelType(new Vector2(10, 10), "Hallo", font));
			ElementTypes.Add(new LabelType(new Vector2(10, 30), "Doei", font));

			ElementTypes.Add(new EmptyButtonType(new Vector2(0, 400), texture));
			ElementTypes.Add(new ButtonType(new Vector2(50, 100), font, texture, "Click here", () => Console.WriteLine("kappa1")));
			ElementTypes.Add(new ButtonType(new Vector2(50, 50), font, texture, "Click here", () => Console.WriteLine("kappa2")));
			ElementTypes.Add(new ButtonType(new Vector2(150, 150), font, texture, "Click here", () => Console.WriteLine("kappa3")));






			//loader = new FormLoader(ElementTypes);
			//controlFactories = loader.Load();
			//controls = new GuiElementsFactoryToGuiElements(controlFactories);
			controls = new GuiElementsFactoryToGuiElements(new FormLoader(ElementTypes).Load());
			




			//TODO: use this.Content to load your game content here 
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif



			controls.Reset();

			Option<GuiElement> current_element = controls.getNext();

			while (current_element.visit(() => false, (arg) => true))
			{
				current_element.visit(() => { throw new Exception("NO"); }, (arg) => arg).update(gameTime.ElapsedGameTime.Milliseconds);
				current_element = controls.getNext();
			}

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			controls.Reset();

			Option<GuiElement> current_element = controls.getNext();

			while(current_element.visit(() => false, (arg) => true)){
				current_element.visit(() => { throw new Exception("NO"); }, (arg) => arg).Draw(spriteBatch);
				current_element = controls.getNext();
			}

			spriteBatch.End();
            
			base.Draw (gameTime);
		}
	}

}

