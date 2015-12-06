using Sifteo;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MathMind
{
	public class MathMind : BaseApp
	{

		//override public int FrameRate {
		//	get { return 1; }
		//}

		public String[] mImageNames;
		//public List<CubeWrapper> mWrappers = new List<CubeWrapper>();
		public Random mRandom = new Random();




		// called during intitialization, before the game has started to run
		override public void Setup ()
		{
			Log.Debug ("Setup()");


			// Load up the list of images.
			mImageNames = LoadImageIndex ();

			String imageName = this.mImageNames [1];

			// The CubeSet represents the set of all connected cubes.  Each Cube
			// object represents a physical cube. Here we iterate over all the cubes
			// and draw on the displays of each one.
			foreach (Cube cube in this.CubeSet) {
				cube.Image (imageName, 0, 0, 0, 0, 128, 128, 1, 0);

				// Create a wrapper object for each cube. The wrapper object allows us
				// to bundle a cube with extra information and behavior.
				//CubeWrapper wrapper = new CubeWrapper(this, cube);
				//mWrappers.Add(wrapper);
				//wrapper.DrawSlide();
			}

			// ## Event Handlers ##
			// Objects in the Sifteo API (particularly BaseApp, CubeSet, and Cube)
			// fire events to notify an app of various happenings, including actions
			// that the player performs on the cubes.
			//
			// To listen for an event, just add the handler method to the event. The
			// handler method must have the correct signature to be added. Refer to
			// the API documentation or look at the examples below to get a sense of
			// the correct signatures for various events.
			//
			// **NeighborAddEvent** and **NeighborRemoveEvent** are triggered when
			// the player puts two cubes together or separates two neighbored cubes.
			// These events are fired by CubeSet instead of Cube because they involve
			// interaction between two Cube objects. (There are Cube-level neighbor
			// events as well, which comes in handy in certain situations, but most
			// of the time you will find the CubeSet-level events to be more useful.)

			//CubeSet.NeighborAddEvent += OnNeighborAdd;
			//CubeSet.NeighborRemoveEvent += OnNeighborRemove;
		


			//*******************************************************************
			//TEMP FIRST TEST - FillCube with Color and Rect
			//*******************************************************************

			// ### Color ###
			// A Color object represents an RGB color.
			//Color color = new Color (170, 218, 85);

			// ### FillScreen ###
			// FillScreen paints the cube's entire screen the given color.
			//cube.FillScreen (color);

			// ### FillRect ###
			// FillRect draws a rectangle on the cube's screen at a given location
			// in a given size and color. A cube's screen is 128x128 pixels. Here
			// we draw a big square in the center of the screen.
			//int x = 24;
			//int y = 24;
			//int width = 80;
			//int height = 80;
			//color = new Color (100, 182, 255);
			//cube.FillRect (color, x, y, width, height);

			//*******************************************************************



			// ### Paint ###
			// Paint tells the cube to copy the frame buffer to the display.
			// Nothing we've drawn will actually show up on the cube's display
			// until we call its Paint() method. Don't forget to call this!
			cube.Paint ();
		}


		override public void Tick()
		{
			Log.Debug ("Tick()");

			//foreach (CubeWrapper wrapper in mWrappers) {
			//	wrapper.Tick();
			//}
			String tFirstOperand = "1";
			String tSecondOperand = "2";
			String tThirdOperand = "3";
			String tFirstOperator = "+";
			String tSecondOperator = "*";
			Log.Debug (calcResult(tFirstOperand,tFirstOperator,tSecondOperand,tSecondOperator,tThirdOperand).ToString());
		}


		// ImageSet is an enumeration of your app's images. It is populated based
		// on your app's siftbundle and index. You rarely have to interact with it
		// directly, since you can refer to images by name.
		//
		// In this method, we scan the image set to build an array with the names
		// of all the images.
		private String[] LoadImageIndex() {
			ImageSet imageSet = this.Images;
			ArrayList nameList = new ArrayList();
			foreach (ImageInfo image in imageSet) {
				nameList.Add(image.name);
			}
			String[] rv = new String[nameList.Count];
			for (int i=0; i<nameList.Count; i++) {
				rv[i] = (string)nameList[i];
			}
			return rv;
		}


		// calcResult 
		private int calcResult(string firstOperand, string firstOperator, string secondOperand, string secondOperator, string thirdOperand) {
			int firstIntOperand = int.Parse (firstOperand);
			int secondIntOperand = int.Parse (secondOperand);
			int thirdIntOperand = int.Parse (thirdOperand);
			int temp = 0;

			//punkt-vor-strich-check beim secondOperator. sonst nach der reihe auswerten
			if (secondOperator == "*" || secondOperator == "/") {
				temp = calcNeighbors (secondIntOperand, secondOperator, thirdIntOperand);
				temp = calcNeighbors (firstIntOperand, firstOperator, temp);
				return temp;
			} else {
				temp = calcNeighbors (firstIntOperand, firstOperator, secondIntOperand);
				temp = calcNeighbors (temp, secondOperator, thirdIntOperand);
				return temp;
			}
		}


		// calcNeighbors
		private int calcNeighbors(int first, string action, int second) {
			if (action == "+")
				return first + second;
			else if (action == "-")
				return first - second;
			else if (action == "*")
				return first * second;
			else if (action == "/")
				return first / second;
			else
				return int.MinValue;
		}





		// development mode only
		// start MathMind as an executable and run it, waiting for Siftrunner to connect
		static void Main (string[] args)
		{
			new MathMind ().Run ();
		}
	}
}

