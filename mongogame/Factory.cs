using System;
using System.Collections.Generic;

namespace monogamegui
{
	interface Iterator<T>
	{
		Option<T> getNext();
		void Reset();
	}

	class GuiElementFactory
	{
		ElementType type;

		public GuiElement Load()
		{
			return type.Visit((arg1, arg2, arg3) => new Button(arg1, arg2, arg3),
			                  (arg1, arg2) => new EmptyButton(arg1, arg2),
			                  (arg1, arg2, arg3) => new Label(arg1, arg2, arg3));
		}

		public GuiElementFactory(ElementType type)
		{
			this.type = type;
		}
	}

	class FormLoader
	{
		List<ElementType> elements;

		public FormLoader(List<ElementType> elements)
		{
			this.elements = elements;
		}

		public GuiElementsFactory Load()
		{
			return new GuiElementsFactory(this.elements);
		}
		
	}

	class GuiElementsFactory : Iterator<GuiElementFactory>
	{
		List<ElementType> elements;
		int index;
		int default_index = -1;

		public GuiElementsFactory(List<ElementType> elements)
		{
			this.elements = elements;
			index = default_index;
		}

		public Option<GuiElementFactory> getNext()
		{
			index = index + 1;
			if (index < elements.Count)
			{
				return new Some<GuiElementFactory>(new GuiElementFactory(elements[index]));
			}
			return new None<GuiElementFactory>();
		}

		public void Reset()
		{
			index = default_index;
		}
	}

	class GuiElementsFactoryToGuiElements : Iterator<GuiElement>
	{
		List<Option<GuiElement>> factory;
		int index;
		int default_index = -1;
		Option<GuiElementFactory> currentFactory;



		public GuiElementsFactoryToGuiElements(GuiElementsFactory factories)
		{
			currentFactory = factories.getNext();
			factory = new List<Option<GuiElement>>();


			while (currentFactory.visit(() => false, (arg => true)))
			{
				factory.Add(new Some<GuiElement>(currentFactory.visit(() => { throw new Exception("Expecting a value..."); }, (arg) => arg.Load())));
				currentFactory = factories.getNext();
			}


			//while (currentFactory.visit(() => false, (arg => true)))
			//{
			//	factory.Add(new Some<GuiElement>(currentFactory.visit(() => { throw new Exception("Expecting a value..."); }, (arg) => arg.Load())));
			//	currentFactory = factories.getNext();
			//}
		}

		public Option<GuiElement> getNext()
		{
			index = index + 1;
			if (index < factory.Count)
			{
				return factory[index];
			}
			return new None<GuiElement>();
		}

		public void Reset()
		{
			this.index = default_index;
		}
	}


}

