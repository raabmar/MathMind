using Sifteo;
using System;

namespace MathMind
{
	public class MathMind : BaseApp
	{

		override public int FrameRate {
			get { return 20; }
		}

		// called during intitialization, before the game has started to run
		override public void Setup ()
		{
			Log.Debug ("Setup()");

			// The CubeSet represents the set of all connected cubes.  Each Cube
			// object represents a physical cube. Here we iterate over all the cubes
			// and draw on the displays of each one.
			foreach (Cube cube in this.CubeSet) {

				// ### Color ###
				// A Color object represents an RGB color.
				Color color = new Color (182, 218, 85);

				// ### FillScreen ###
				// FillScreen paints the cube's entire screen the given color.
				cube.FillScreen (color);

				// ### FillRect ###
				// FillRect draws a rectangle on the cube's screen at a given location
				// in a given size and color. A cube's screen is 128x128 pixels. Here
				// we draw a big square in the center of the screen.
				int x = 24;
				int y = 24;
				int width = 80;
				int height = 80;
				color = new Color (36, 182, 255);
				cube.FillRect (color, x, y, width, height);

				// ### Paint ###
				// Paint tells the cube to copy the frame buffer to the display.
				// Nothing we've drawn will actually show up on the cube's display
				// until we call its Paint() method. Don't forget to call this!
				cube.Paint ();
			}



		}

		override public void Tick ()
		{
			Log.Debug ("Tick()");
		}

		// development mode only
		// start MathMind as an executable and run it, waiting for Siftrunner to connect
		static void Main (string[] args)
		{
			new MathMind ().Run ();
		}
	}
}

