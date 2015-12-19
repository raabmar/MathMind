using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Sifteo;
using Sifteo.Util;



/***************************************************************************************
 * 
 * MathMind
 * 
 **************************************************************************************/
namespace MathMindSpace {

	public class MathMind : BaseApp {

		//Examples of calculations
		private List<List<CalcPart>> exampleCalculations = new List<List<CalcPart>>{
			new List<CalcPart> {
				// 8 + 7 * 3 = 29
				new CalcPart ("8", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("7", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("3", CalculationPartType.NUMBER),
				new CalcPart ("res92", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 6 * 7 * 2 = 84
				new CalcPart ("6", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("7", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("2", CalculationPartType.NUMBER),
				new CalcPart ("res84", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 4 * 5 - 2 = 18
				new CalcPart ("4", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("5", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("2", CalculationPartType.NUMBER),
				new CalcPart ("res18", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 6 + 4 * 8 = 38
				new CalcPart ("6", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("4", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("8", CalculationPartType.NUMBER),
				new CalcPart ("res38", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 9 * 7 / 3 = 21
				new CalcPart ("9", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("7", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("3", CalculationPartType.NUMBER),
				new CalcPart ("res21", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 5 * 7 + 8 = 43
				new CalcPart ("5", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("7", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("8", CalculationPartType.NUMBER),
				new CalcPart ("res43", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 6 / 2 * 4 = 12
				new CalcPart ("2", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("4", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("6", CalculationPartType.NUMBER),
				new CalcPart ("res21", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 5 / 1 + 9 = 14
				new CalcPart ("5", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("9", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("1", CalculationPartType.NUMBER),
				new CalcPart ("res14", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 3 * 8 * 4 = 96
				new CalcPart ("3", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("8", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("4", CalculationPartType.NUMBER),
				new CalcPart ("res96", CalculationPartType.RESULT)
			},
			new List<CalcPart> {
				// 8 * 5 - 6 = 34
				new CalcPart ("6", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("5", CalculationPartType.NUMBER),
				new CalcPart ("quest", CalculationPartType.OPERATOR),
				new CalcPart ("8", CalculationPartType.NUMBER),
				new CalcPart ("res43", CalculationPartType.RESULT)
			}
		};


		// Public String List ImageNames that contains all the filenames of images
		public List<String> ImageNames { get; private set; }
		private List<CubeWrapper> mWrappers = new List<CubeWrapper>();
		private int totalCubes = 6;
		private bool foundRow = false;
		public bool calcOk = false;
		private int calcIndex = 0;


		// ## SETUP ##
		// Here we initialize our app.
		public override void Setup() {
			
			// Load up the list of images.
			LoadImageFolder();

			// calcIndex entspricht der Spielrunde (erste Rechnung, zweite Rechnung, etc.)
			List<CalcPart> calculation = exampleCalculations.ElementAt(calcIndex);

			// Loop through all the cubes and set them up.
			for (int i = 0; i < totalCubes; i++) {

				// Create a wrapper object for each cube. The wrapper object allows us
				// to bundle a cube with extra information and behavior.
				CubeWrapper wrapper = new CubeWrapper(this, CubeSet[i]);
				mWrappers.Add(wrapper); // add wrapper including individual cube into wrapper-list

				// Each cube contains a part of the entire calculation which needs to be printed
				CalcPart part = calculation[i];
				wrapper.SetCalcPart(part);

			}
				
			// **NeighborAddEvent** and **NeighborRemoveEvent** are triggered when
			// the player puts two cubes together or separates two neighbored cubes.
			// These events are fired by CubeSet instead of Cube because they involve
			// interaction between two Cube objects. (There are Cube-level neighbor
			// events as well, which comes in handy in certain situations, but most
			// of the time you will find the CubeSet-level events to be more useful.)
			CubeSet.NeighborAddEvent += OnNeighborAdd;
			CubeSet.NeighborRemoveEvent += OnNeighborRemove;
		}



		// ## Neighbor Add ##
		// This method is a handler for the NeighborAdd event. It is triggered when
		// two cubes are placed side by side.
		//
		// Cube1 and cube2 are the two cubes that are involved in this neighboring.
		// The two cube arguments can be in any order; if your logic depends on
		// cubes being in specific positions or roles, you need to add logic to
		// this handler to sort the two cubes out.
		//
		// Side1 and side2 are the sides that the cubes neighbored on.
		private void OnNeighborAdd(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor add: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

			// Find 6 connected cubes (rotation does not matter)
			Cube[] connected = CubeHelper.FindConnected(cube1);
			if (connected.Length == totalCubes) {
				Log.Debug ("6 connected");
				foundRow = true;
			} else {
				foundRow = false;
			}
		}



		// ## Neighbor Remove ##
		// This method is a handler for the NeighborRemove event. It is triggered
		// when two cubes that were neighbored are separated.
		//
		// The side arguments for this event are the sides that the cubes
		// _were_ neighbored on before they were separated. If you check the
		// current state of their neighbors on those sides, they should of course
		// be NONE.
		private void OnNeighborRemove(Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)  {
			Log.Debug("Neighbor remove: {0}.{1} <-> {2}.{3}", cube1.UniqueId, side1, cube2.UniqueId, side2);

			if (calcOk) {
				calcIndex = (calcIndex + 1) % exampleCalculations.Count;
				// show next calculation example
				for (int i = 0; i < mWrappers.Count; i++) {
					mWrappers [i].SetCalcPart (exampleCalculations[calcIndex].ElementAt (i));		
				}
				calcOk = false; //reset to false after successful calculation and forwarding to next calc-example
			} else {
				foreach (var item in mWrappers) {
					item.RefreshSlide ();
				}
				foundRow = false;
			}
		}
				


		// ## Tick MathMind ##
		// Defer all per-frame logic to each cube's wrapper.
		public override void Tick() {
			foreach (CubeWrapper wrapper in mWrappers) {
				wrapper.Tick();
			}
			
			//TODO Achtung, mit FindRow müssen die Cubes richtig gedreht sein - ev. FindConnected(cube) verwenden, aber von wo cube hernehmen??
			Cube[] cubeRow = CubeHelper.FindRow (CubeSet);

			if (foundRow && cubeRow.Count() == totalCubes) { //TODO Wieso doppelte Abfrage nach foundRow/cubeRow - ist das nicht ident?
				int result = 0; 
				bool orderOk = false;
				CubeWrapper wCube1 = (CubeWrapper)cubeRow[0].userData;
				CubeWrapper wCube2 = (CubeWrapper)cubeRow[1].userData;
				CubeWrapper wCube3 = (CubeWrapper)cubeRow[2].userData;
				CubeWrapper wCube4 = (CubeWrapper)cubeRow[3].userData;
				CubeWrapper wCube5 = (CubeWrapper)cubeRow[4].userData;
				CubeWrapper wCube6 = (CubeWrapper)cubeRow[5].userData;
				
				//check order/sequence - cube2 and cube 4 always have to be type Operator
				orderOk = wCube2.CalcPart.Type == CalculationPartType.OPERATOR && wCube4.CalcPart.Type == CalculationPartType.OPERATOR &&
				wCube1.CalcPart.Type == CalculationPartType.NUMBER && wCube3.CalcPart.Type == CalculationPartType.NUMBER &&
				wCube5.CalcPart.Type == CalculationPartType.NUMBER && wCube6.CalcPart.Type == CalculationPartType.RESULT;

				if (orderOk) {
					result = calcResult(wCube1.CalcPart.ImageName, getOperator(wCube2.CalcPart.ImageName), 
							wCube3.CalcPart.ImageName, getOperator(wCube4.CalcPart.ImageName), wCube5.CalcPart.ImageName);
					
					if (result == Int32.Parse (wCube6.CalcPart.ImageName.Substring (3, 2))) { //HARDCODED because of filename
						foreach (var item in mWrappers) {
							item.DrawResult (true);
						}
						calcOk = true;
					} else {
						foreach (var item in mWrappers) {
							item.DrawResult (false);
						}
						calcOk = false;
					}
				} 
				else {
					foreach (var item in mWrappers) {
						item.DrawResult (false);
					}
					calcOk = false;
				}
				
			}

		}

		
		
		// ## getOperator ##
		// Converts operator-filename to arithmetic operator
		private string getOperator(String imagename)
		{
			switch (imagename) 
			{
				case "addop":
					return "+";
				case "subop":
					return "-";
				case "multop":
					return "*";
				case "divop":
					return "/";
				default:
					break;
			}
			return "Operator not found";
		}


		// ## calcResult ##
		// Calculates the actual result according to the current cube-sequence
		private int calcResult(string firstNumber, string firstOperator, string secondNumber, string secondOperator, string thirdNumber) {
			int firstIntNumber = int.Parse (firstNumber);
			int secondIntNumber = int.Parse (secondNumber);
			int thirdIntNumber = int.Parse (thirdNumber);
			int temp = 0;

			//punkt-vor-strich-regelung (erster Operator 'Strich', zweiter Operator 'Punkt'
			if ((firstOperator == "+" || firstOperator == "-") && (secondOperator == "*" || secondOperator == "/")) {
				temp = calcNeighbors (secondIntNumber, secondOperator, thirdIntNumber);
				temp = calcNeighbors (firstIntNumber, firstOperator, temp);
				return temp;
			} else {
				temp = calcNeighbors (firstIntNumber, firstOperator, secondIntNumber);
				temp = calcNeighbors (temp, secondOperator, thirdIntNumber);
				return temp;
			}
		}

		// ## calcNeighbors ##
		// Helper function for actual calculation
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

		
		// ## LoadImageFolder ##
		// ImageSet is an enumeration of your app's images. It is populated based
		// on your app's siftbundle and index. You rarely have to interact with it
		// directly, since you can refer to images by name.
		//
		// In this method, we scan the image set to build an array with the names
		// of all the images.
		private void LoadImageFolder() {
			ImageSet imageSet = this.Images;
			List<String> nameList = new List<String>();
			foreach (ImageInfo image in imageSet) {
				nameList.Add(image.name);
			}
			// Public String List ImageNames that contains all the filenames of images
			ImageNames = nameList;
		}
	}





//-----------------------------------------------------------------------------------------------------------------------





	// ## CalculationPartType ENUM ##
	public enum CalculationPartType
	{
		NUMBER, OPERATOR, RESULT
	}



//-----------------------------------------------------------------------------------------------------------------------

	




	/***************************************************************************************
	 * Class CalcPart
	 * Contains the imagename and type (number, operator, result) of the part
	 * *************************************************************************************/
	public class CalcPart
	{
		public CalcPart (string imagename, CalculationPartType type)
		{
			ImageName = imagename;
			Type = type;
		}


		public string ImageName {
			get;
			set;
		}


		public CalculationPartType Type {
			get;
			set;
		}
	}






//-----------------------------------------------------------------------------------------------------------------------







	/***************************************************************************************
	 * Class CubeWrapper
	 * "Wrapper" is not a specific API, but a pattern that is used in many Sifteo
	 * apps. A wrapper is an object that bundles a Cube object with game-specific
	 * data and behaviors.
	 * *************************************************************************************/
	public class CubeWrapper {
		private MathMind mApp;
		private Cube mCube;
		public int mIndex;
		private int opIndex;
		private List<String> opImages;
		private int mXOffset = 0;
		private int mYOffset = 0;
		public int mScale = 1;
		public int mRotation = 0;
		public CalculationPartType mType = CalculationPartType.OPERATOR;
		public int CurrentValue { get; set; }
		public CalcPart CalcPart { get; private set; }
		public bool mNeedDraw = false; // This flag tells the wrapper to redraw the current image on the cube. (See Tick, below).
		
		//Konstruktor
		public CubeWrapper(MathMind app, Cube cube) {
			mApp = app;
			mCube = cube;
			mCube.userData = this;
			mIndex = 0;
			opIndex = 0;

			// List of Strings of all the available operators (addop, subop, multop, divop, questop)
			opImages = mApp.ImageNames.Where (i => i.Contains ("op")).ToList ();

			// Here we attach more event handlers for button and accelerometer actions (i.e. event handlers for individual cubes/cubewrapper)
			mCube.ButtonEvent += OnButton;
			mCube.ShakeStartedEvent += OnShakeStarted;
			mCube.ShakeStoppedEvent += OnShakeStopped;
		}



		// ## SetCalcPart ##
		// Set up individual calc parts on a cube and prepare for print
		public void SetCalcPart(CalcPart part)
		{
			CalcPart = part;
			DrawSlide (CalcPart.ImageName);
		}



		// ## EVENT Button ##
		// This is a handler for the Button event. It is triggered when a cube's
		// face button is either pressed or released. The `pressed` argument
		// is true when you press down and false when you release.
		private void OnButton(Cube cube, bool pressed) {
			if (pressed) 
			{
				//Pressed button is just relevant for the operator-cubes
				if (CalcPart != null && CalcPart.Type == CalculationPartType.OPERATOR) {
					opIndex = (opIndex + 1) % opImages.Count ();
					CalcPart.ImageName = opImages.ElementAt (opIndex);
					DrawSlide (CalcPart.ImageName);
				}
			}
		}
			
			

		// ## EVENT Shake Started ##
		// This is a handler for the ShakeStarted event. It is triggered when the
		// player starts shaking a cube. When the player stops shaking, a
		// corresponding ShakeStopped event will be fired (see below).
		private void OnShakeStarted(Cube cube) {
			Log.Debug("Shake start");
		}



		// ## EVENT Shake Stopped ##
		// This is a handler for the ShakeStarted event. It is triggered when the
		// player stops shaking a cube. The `duration` argument tells you
		// how long (in milliseconds) the cube was shaken.
		private void OnShakeStopped(Cube cube, int duration) {
			Log.Debug("Shake stop: {0}", duration);

			//Shake is just possible at the result-cube
			if (CalcPart != null && CalcPart.Type == CalculationPartType.RESULT) {
				Log.Debug ("ImageName: " + CalcPart.ImageName);
				char[] charArray = CalcPart.ImageName.Substring(3, 2).ToCharArray(); //HARDCODED because of filename
				Array.Reverse( charArray );
				string number =  new string( charArray );
				CalcPart.ImageName = CalcPart.ImageName.Substring(0, 3) + number; //HARDCODED because of filename
				RefreshSlide ();
			}

		}


		// ## RefreshSlide ##
		// Refreshes the slide on the cube without setting back the background
		public void RefreshSlide()
		{
			if (CalcPart != null) {
				DrawSlide (CalcPart.ImageName);
			}
		}


		// ## DrawSlide ##
		// This method draws the current image to the cube's display. The
		// DrawSlide method has a lot of arguments, but many of them are optional
		// and have reasonable default values.
		public void DrawSlide(String imageName) {
			int screenX = mXOffset;
			int screenY = mYOffset;
			int imageX = 0;
			int imageY = 0;
			int width = 128;
			int height = 128;
			int scale = mScale;
			int rotation = mRotation;

			// Clear off whatever was previously on the display before drawing the new image.
			mCube.FillScreen(Color.White);
			if (CalcPart != null) {
				mCube.Image(CalcPart.ImageName, screenX, screenY, imageX, imageY, width, height, scale, rotation);
			}
			mCube.Paint();
		}
		
	

		// ## DrawResult ##
		// Paints the background either green or red, depending on the result of the user's calculation
		public void DrawResult(bool success)
		{
			if (success) {
				mCube.FillScreen(new Color (0, 255, 0)); //grün
			} else {
				mCube.FillScreen (new Color (255, 0, 0)); //rot
			}
			if (CalcPart != null) {
				mCube.Image(CalcPart.ImageName, 0, 0, 0, 0, 128, 128, 1, 0);
				//Parameters imageName, screenx, screeny, imageX, imageY, width, height, scale, rotation
			}
			mCube.Paint ();
		}


		// ## Tick in CubeWrapper ##
		// This method is called every frame by the Tick in MathMind (see above.)
		public void Tick() {
			// You can check whether a cube is being shaken at this moment by looking
			// at the IsShaking flag.
			if (mCube.IsShaking) {
				mNeedDraw = true;
			}
		}
	}
}

