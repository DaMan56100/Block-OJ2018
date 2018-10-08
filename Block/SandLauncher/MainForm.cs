/*
 * Created by SharpDevelop.
 * User: Sammy
 * Date: 05/10/2018
 * Time: 20:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SandLauncher;

namespace SandLauncher
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public static SolidBrush brushBlack = new SolidBrush(Color.Black);
		public static SolidBrush brushWhite = new SolidBrush(Color.White);
		Graphics gfx;
		
		gameWindow mainWindow;
		gameWindow leftWindow;
		gameWindow rightWindow;
		TileBoard gameBoard;
		
		Enemy[] enemies;
		
		bool mouseDown; // keeps track of whether the mouse was depressed
		
		const int borderSize = 15;
		const int TileSize=4;
		const int BoardSize=25;
		const int EnemyCount=25;
		const double EnemySpeed=1;
		
		public static int lostWidth;
		public static int lostHeight;
		
		public static int clicks;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			gameBoard = new TileBoard(BoardSize,BoardSize);
			gameBoard.FloodBoard(new Tile(0,"empty"));
			mainWindow = new gameWindow(newRectPoint2Point(allignPoint(this.ClientRectangle,borderSize,0),allignPoint(this.ClientRectangle,100-borderSize,100)),picMain,100,100);
			leftWindow = new gameWindow(newRectPoint2Point(allignPoint(this.ClientRectangle,0,0),allignPoint(this.ClientRectangle,borderSize,100)),picMain,borderSize,borderSize);
			rightWindow = new gameWindow(newRectPoint2Point(allignPoint(this.ClientRectangle,100-borderSize,0),allignPoint(this.ClientRectangle,100,100)),picMain,borderSize,borderSize);
			mainWindow.setState("Title");
			leftWindow.setState("OJ");
			rightWindow.setState("Scores");
			lostWidth=this.Width-this.ClientRectangle.Width;
			lostHeight=this.Height-this.ClientRectangle.Height;
			mouseDown=false;
		}
		
		void TckResizeTick(object sender, EventArgs e)
		{
			gfx = picMain.CreateGraphics();
			resizeTick();
		}
		
		void TckGameTick(object sender, EventArgs e)
		{
			gfx = picMain.CreateGraphics();
			if (mainWindow.gameState.Equals("Title")) {
				menuTick();
			} else {
				gameTick();
			}
		}
		
		void PicMainClick(object sender, EventArgs e)
		{
			switch (mainWindow.gameState) {
				case "Title" :
					if (isPointInRectangle(getMousePoint(this,this.ClientRectangle),
					                       newRectPoint2Point(allignPoint(mainWindow.area,31,60),allignPoint(mainWindow.area,70,71)))) {
						mainWindow.setState("Main");
						DisplayBoard(picMain,gameBoard,mainWindow);
						spawnEnemies(EnemyCount,EnemySpeed);
					}
					break;
				case "Main":
					mouseDown=true;
					break;
				default:
					
					break;
			}
		}
		
		void resizeTick() {
			picMain.Refresh();
			picMain.Size=this.Size;
			fillAllignedRectPoint2Point(gfx,brushBlack,this.ClientRectangle,0,0,borderSize,100);
			fillAllignedRectPoint2Point(gfx,brushBlack,this.ClientRectangle,100-borderSize,0,100,100);
			fillOJ();
			mainWindow.ResizeMe(newRectPoint2Point(allignPoint(this.ClientRectangle,borderSize,0),allignPoint(this.ClientRectangle,100-borderSize,100)));
			leftWindow.ResizeMe(newRectPoint2Point(allignPoint(this.ClientRectangle,0,0),allignPoint(this.ClientRectangle,borderSize,100)));
		}
		
		void menuTick() {
			// Checks if start button is hightlighted
			if (isPointInRectangle(getMousePoint(this,this.ClientRectangle),newRectPoint2Point(allignPoint(mainWindow.area,31,60),allignPoint(mainWindow.area,70,71)))) {
			    fillAllignedRectPoint2Point(gfx,brushBlack,mainWindow.area,29,58,72,59);
			    fillAllignedRectPoint2Point(gfx,brushBlack,mainWindow.area,29,58,30,73);
			    fillAllignedRectPoint2Point(gfx,brushBlack,mainWindow.area,29,72,72,73);
			    fillAllignedRectPoint2Point(gfx,brushBlack,mainWindow.area,71,58,72,73);
			   // 31,60,70,71
			}
		}
		
		void gameTick() {
			int cursorX = getMouseX(this,mainWindow.area)/(TileSize*mainWindow.scale);
			int cursorY = getMouseY(this,mainWindow.area)/(TileSize*mainWindow.scale);
			if (inRange(cursorX,0,BoardSize)&&inRange(cursorY,0,BoardSize)) {
				//DisplayTile(gfx,new Tile(cursorX,cursorY,0,"cursor"),mainWindow);
			}
			rightWindow.ResizeMe(newRectPoint2Point(allignPoint(this.ClientRectangle,100-borderSize,0),allignPoint(this.ClientRectangle,100,100)));
			rightWindow.DrawWindow();
			handleEnemies();
			// Draws border
			fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,mainWindow.area,0,0,1,100);
			fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,mainWindow.area,0,0,100,1);
			fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,mainWindow.area,99,0,100,100);
			fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,mainWindow.area,0,99,100,100);
			if (mouseDown) {
				mouseDown=false;
				if (inRange(cursorX,0,BoardSize-1)&&inRange(cursorY,0,BoardSize-1)) {
					if (gameBoard.board[cursorX,cursorY].state.Equals("enemy")) {
						mainWindow.setState("Title");
						clicks=0; // resets score
					} else {
						clicks++;
					}
				}
			}
		}
		
		void fillOJ() {
			fillAllignedRectPoint2Point(gfx,brushWhite,leftWindow.area,10,10,40,40);
			fillAllignedRectPoint2Point(gfx,brushBlack,leftWindow.area,20,20,30,30);
			fillAllignedRectPoint2Point(gfx,brushWhite,leftWindow.area,50,10,80,20);
			fillAllignedRectPoint2Point(gfx,brushWhite,leftWindow.area,60,20,70,40);
			fillAllignedRectPoint2Point(gfx,brushWhite,leftWindow.area,50,30,60,40);
			
			fillLetter(gfx,brushWhite,brushBlack,leftWindow.area,"2",10,50,3);
			fillLetter(gfx,brushWhite,brushBlack,leftWindow.area,"0",31,50,3);
			fillLetter(gfx,brushWhite,brushBlack,leftWindow.area,"1",52,50,3);
			fillLetter(gfx,brushWhite,brushBlack,leftWindow.area,"8",73,50,3);
		}
		
		void spawnEnemies(int n, double speed) {
			Random RNG = new Random();
			int newX;
			int newY;
			enemies = new Enemy[n];
			for (int i = 0; i < n; i++) {
				//MessageBox.Show(i.ToString());
				newX=RNG.Next(0,gameBoard.boardSizeX);
				newY=RNG.Next(0,gameBoard.boardSizeY);
				enemies[i] = new Enemy(newX,newY,(Convert.ToDouble(RNG.Next(0,3600)))/10,speed,gameBoard);
			}
		}
		
		void handleEnemies() {
			for (int c = 0; c < enemies.Length; c++) {
				clearEnemy(c);
				moveEnemy(c);
				displayEnemy(c);
			}
		}
		
		void clearEnemy(int n) {
			enemies[n].ClearMe(gfx,mainWindow);
		}
		
		void moveEnemy(int n) {
			enemies[n].Move();
			enemies[n].CheckWalls();
			enemies[n].ChangeTile();
		}
		
		void displayEnemy(int n) {
			enemies[n].DisplayMe(gfx,mainWindow);
		}
		
		/// <summary>
		/// Creates a point mapped onto a 2x2 form.
		/// </summary>
		/// <param name="thisForm">Form to find point upon</param>
		/// <param name="XProg">X Progress along form</param>
		/// <param name="YProg">X Progress along form</param>
		/// <returns>A point mapped onto the form</returns>
		public static Point allignPoint(Rectangle area, int XProg, int YProg) {
			return new Point(((area.Width*XProg)/100)+area.X,((area.Height*YProg)/100)+area.Y);
		}
		
		/// <summary>
		/// Creates a rectangle from a centre point
		/// </summary>
		/// <param name="centre">Point at centre of rectange</param>
		/// <param name="XRad">Distance left/right from centre</param>
		/// <param name="YRad">Distance up/down from centre</param>
		/// <returns>A rectange from the centre point</returns>
		public static Rectangle newRectFromPoint(Point centre, int XRad, int YRad) {
			return new Rectangle(centre.X-XRad,centre.Y-YRad,XRad*2,YRad*2);
		}
		
		/// <summary>
		/// Creates a rectange between two points
		/// </summary>
		/// <param name="pointTopRight">Top Left corner of rectangle</param>
		/// <param name="pointBottomLeft">Bottom Right corner of rectangle</param>
		/// <returns>A rectange between two points</returns>
		public static Rectangle newRectPoint2Point(Point pointTopRight, Point pointBottomLeft) {
			return new Rectangle(pointTopRight.X,pointTopRight.Y,pointBottomLeft.X-pointTopRight.X,pointBottomLeft.Y-pointTopRight.Y);
		}
		
		public static void fillAllignedRectPoint2Point(Graphics GFX, SolidBrush brush, Rectangle area, int minXProg, int minYProg, int maxXProg, int maxYProg) {
			GFX.FillRectangle(brush,newRectPoint2Point(allignPoint(area,minXProg,minYProg),allignPoint(area,maxXProg,maxYProg)));
		}
		
		public static void fillLetter(Graphics GFX, SolidBrush brushMain, SolidBrush brushBack, Rectangle area, string letter, int oX, int oY, int s) {
			switch (letter) {
				case "0":
					// origin = 48,22
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+(s*1),oY+(s*1),oX+(s*4),oY+(s*6));
					break;
					
				case "1":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+s,oY,oX+(s*3),oY+s);
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*2),oY+s,oX+(s*3),oY+(s*6));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY+(s*6),oX+(s*5),oY+(s*7));
					break;
					
				case "2":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX,oY+s,oX+(s*4),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+(s*4),oX+(s*5),oY+(s*6));
					break;
					
				case "3":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX,oY+s,oX+(s*4),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX,oY+(s*4),oX+(s*4),oY+(s*6));
					break;
					
				case "4":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+s,oY+(s*4));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+s,oY+(s*3),oX+(s*5),oY+(s*4));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*3),oY,oX+(s*4),oY+(s*7));
					break;
					
				case "5":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*5),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX,oY+(s*4),oX+(s*4),oY+(s*6));
					break;
					
				case "6":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*5),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+(s*4),oX+(s*4),oY+(s*6));
					break;
					
				case "7":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+s);
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*4),oY+s,oX+(s*5),oY+(s*7));
					break;
					
				case "8":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*4),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+(s*4),oX+(s*4),oY+(s*6));
					break;
					
				case "9":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*4),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX,oY+(s*4),oX+(s*4),oY+(s*6));
					break;
					
				case "A":
					// origin=48,62  
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*4),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+(s*4),oX+(s*4),oY+(s*7));
					break;
					
				case "B":
					// origin = 34, 22
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+(s*4),oY,oX+(s*5),oY+(s*1));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+(s*4),oY+(s*6),oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+(s*4),oY+(s*3),oX+(s*5),oY+(s*4));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+(s*1),oY+(s*1),oX+(s*4),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+(s*1),oY+(s*4),oX+(s*4),oY+(s*6));
					break;
					
				case "C":
					// origin =55,22
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*5),oY+(s*6));
					break;
					
				case "E":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*5),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+(s*4),oX+(s*5),oY+(s*6));
					break;
					
				case "K":
					// origin =62,22
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+s,oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+s,oY+(s*3),oX+(s*2),oY+(s*4));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*2),oY+(s*2),oX+(s*3),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*3),oY+(s*1),oX+(s*4),oY+(s*2));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*4),oY,oX+(s*5),oY+s);
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*2),oY+(s*4),oX+(s*3),oY+(s*5));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*3),oY+(s*5),oX+(s*4),oY+(s*6));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*4),oY+(s*6),oX+(s*5),oY+(s*7));
					break;
					
				case "L":
					// origin = 41, 22
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+s,oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+s,oY+(s*6),oX+(s*5),oY+(s*7));
					break;
					
				case "O":
					// origin = 48,22
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+(s*1),oY+(s*1),oX+(s*4),oY+(s*6));
					break;
					
				case "R":
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*4),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+(s*4),oX+(s*4),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+(s*4),oY+(s*3),oX+(s*5),oY+(s*4));
					break;
					
				case "S":
					// origin=34,62
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX+s,oY+s,oX+(s*5),oY+(s*3));
					fillAllignedRectPoint2Point(GFX,brushBack,area,oX,oY+(s*4),oX+(s*4),oY+(s*6));
					break;
					
				case "T":
					// origin=41,62
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+s);
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX+(s*2),oY+s,oX+(s*3),oY+(s*7));
					break;
					
				default:
					fillAllignedRectPoint2Point(GFX,brushMain,area,oX,oY,oX+(s*5),oY+(s*7));
					break;
			}
		}
		
		public static int getMouseX(Form thisForm, Rectangle thisArea) {
			return Cursor.Position.X - thisForm.Location.X - thisArea.X - (lostWidth/2);
		}
		
		public static int getMouseY(Form thisForm, Rectangle thisArea) {
			return Cursor.Position.Y - thisForm.Location.Y - thisArea.Y - (lostHeight-(lostWidth/2));
		}
		
		public static Point getMousePoint(Form thisForm, Rectangle thisArea) {
			return new Point(getMouseX(thisForm,thisArea),getMouseY(thisForm,thisArea));
		}
		
		public static bool isPointInRectangle(Point point, Rectangle area) {
			if (inRange(point.X,area.X,area.X+area.Width)&&inRange(point.Y,area.Y,area.Y+area.Height)) {
				return true;
			}
			return false;
		}
		
		public static bool inRange(int input, int min, int max) {
			if ((input>=min)&&(input<=max)) {
				return true;
			}
			return false;
		}
		
		public static void DisplayBoard(PictureBox area, TileBoard board, gameWindow window) {
			Graphics gfx = area.CreateGraphics();
			int displayX=0;
			int displayY=0;
			for (int x = 0; x < board.boardSizeX; x++) {
				displayY=0;
				for (int y = 0; y < board.boardSizeY; y++) {
					DisplayTile(gfx,board.board[x,y],window);
					displayY+=TileSize;
				}
				displayX+=TileSize;
			}
		}
		
		public static void DisplayTile(Graphics gfx, Tile tile, gameWindow window) {
			switch (tile.state) {
				case "empty":
					fillAllignedRectPoint2Point(gfx,brushWhite,window.area,tile.x*TileSize,tile.y*TileSize,(tile.x*TileSize)+TileSize,(tile.y*TileSize)+TileSize);
					break;
				case "enemy":
					fillAllignedRectPoint2Point(gfx,brushBlack,window.area,tile.x*TileSize,tile.y*TileSize,(tile.x*TileSize)+TileSize,(tile.y*TileSize)+TileSize);
					break;
				case "cursor":
					fillAllignedRectPoint2Point(gfx,brushBlack,window.area,tile.x*TileSize,tile.y*TileSize,(tile.x*TileSize)+TileSize,(tile.y*TileSize)+TileSize);
					fillAllignedRectPoint2Point(gfx,brushWhite,window.area,(tile.x*TileSize)+1,(tile.y*TileSize)+1,((tile.x*TileSize)+TileSize)-1,((tile.y*TileSize)+TileSize)-1);
					break;
				case "click":
					fillAllignedRectPoint2Point(gfx,brushBlack,window.area,tile.x*TileSize,tile.y*TileSize,(tile.x*TileSize)+TileSize,(tile.y*TileSize)+TileSize);
					fillAllignedRectPoint2Point(gfx,brushWhite,window.area,(tile.x*TileSize)+1,(tile.y*TileSize)+1,((tile.x*TileSize)+TileSize)-1,((tile.y*TileSize)+TileSize)-1);
					fillAllignedRectPoint2Point(gfx,brushBlack,window.area,(tile.x*TileSize)+2,(tile.y*TileSize)+2,((tile.x*TileSize)+TileSize)-2,((tile.y*TileSize)+TileSize)-2);
					break;
				default:
					break;
			}
			
		}
		
		public static double DegtoRad(double degrees) {
			return degrees * Math.PI / 180.0;
		}
	}
	
	public class gameWindow {
		public Rectangle areaMax; // The space available to fill
		public Rectangle area; // The space being used
		
		public string gameState;
		
		PictureBox thisPic; // Selected picturebox to draw in
		Graphics gfx; // Graphics of said picturebox
		
		int width; // Fixed width of the window
		int height; // Fixed height of the window
		public int scale; // Current scale of window
		
		public gameWindow(Rectangle AreaMax,PictureBox ThisPic, int Width, int Height) {
			areaMax=AreaMax;
			thisPic=ThisPic;
			gfx=thisPic.CreateGraphics();
			width=Width;
			height=Height;
			ResizeMe(areaMax);
		}
		
		public void	ResizeMe(Rectangle AreaMax) {
			int limit; // The limiting size
			int limitsize; // The size the limit needs to adhere to
			areaMax=AreaMax; // Inputs the new max area
			if ((areaMax.Width-(width*scale))>(areaMax.Height-(height*scale))) {
				limit=areaMax.Height;
				limitsize=height;
			} else {
				limit=areaMax.Width;
				limitsize=width;
			}
			scale = limit / limitsize;
			area = MainForm.newRectFromPoint(MainForm.allignPoint(areaMax,50,50),(width/2)*scale,(height/2)*scale);
			DrawWindow(); // Draws the new window
		}
		
		public void DrawWindow() {
			// Draws border
			gfx=thisPic.CreateGraphics();
			
			switch (gameState) {
				case "Title":
					// Draws border
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,0,0,1,100);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,0,0,100,1);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,99,0,100,100);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,0,99,100,100);
					/// Draws title
					// Draws box
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,27,18,74,33);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushWhite,area,29,19,72,32);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,31,20,70,31);
					
					// Draws letters
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"B",34,22,1);
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"L",41,22,1);
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"O",48,22,1);
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"C",55,22,1);
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"K",62,22,1);
					
					/// Start button
					// Outer box
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,31,60,70,71);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushWhite,area,31,60,32,61);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushWhite,area,31,70,32,71);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushWhite,area,69,60,70,61);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushWhite,area,69,70,70,71);
					
					// Draws letters
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"S",34,62,1);
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"T",41,62,1);
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"A",48,62,1);
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"R",55,62,1);
					MainForm.fillLetter(gfx,MainForm.brushWhite,MainForm.brushBlack,area,"T",62,62,1);
					
					// Other decorations
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,5,0,10,100);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,90,0,95,100);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,0,5,100,10);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,0,90,100,95);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushWhite,area,0,7,100,8);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushWhite,area,0,92,100,93);
					break;
				case "Scores":
					int letterX=7; // for drawing each letter
					string numlength = MainForm.clicks.ToString();
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushWhite,area,0,0,100,100);
					MainForm.fillLetter(gfx,MainForm.brushBlack,MainForm.brushWhite,area,"S",7,10,3);
					MainForm.fillLetter(gfx,MainForm.brushBlack,MainForm.brushWhite,area,"C",25,10,3);
					MainForm.fillLetter(gfx,MainForm.brushBlack,MainForm.brushWhite,area,"O",43,10,3);
					MainForm.fillLetter(gfx,MainForm.brushBlack,MainForm.brushWhite,area,"R",61,10,3);
					MainForm.fillLetter(gfx,MainForm.brushBlack,MainForm.brushWhite,area,"E",79,10,3);
					for (int c = 0; c < numlength.Length; c++) {
						MainForm.fillLetter(gfx,MainForm.brushBlack,MainForm.brushWhite,area,MainForm.clicks.ToString().Substring(c,1),letterX,55,3);
						letterX = letterX + 18;
					}
					break;
					
				case "Main":
					// Draws border
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,0,0,1,100);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,0,0,100,1);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,99,0,100,100);
					MainForm.fillAllignedRectPoint2Point(gfx,MainForm.brushBlack,area,0,99,100,100);
					break;
			}
		}
		
		public void setState(string State) {
			gameState=State;
		}
	}
	
	
	public class TileBoard {
		public Tile[,] board;
		public int boardSizeX;
		public int boardSizeY;
		
		public TileBoard(int BoardSizeX, int BoardSizeY) {
			boardSizeX=BoardSizeX;
			boardSizeY=BoardSizeY;
			board = new Tile[boardSizeX,boardSizeY];
			for (int x = 0; x < boardSizeX; x++) {
				for (int y = 0; y < boardSizeY; y++) {
					board[x,y] = new Tile(x,y,0,"empty");
				}
			}
		}
		
		public int SetbyID(int ID, Tile tile, TileBoard setBoard) {
			int minX=boardSizeX;
			int maxX=0;
			int difX;
			int minY=boardSizeY;
			int maxY=0;
			int difY;
			for (int x = 0; x < boardSizeX; x++) {
				for (int y = 0; y < boardSizeY; y++) {
					if (board[x,y].ID==ID) {
						if (x<minX) {
							minX=x;
						}
						if (x>maxX) {
							maxX=x;
						}
						if (y<minY) {
							minY=y;
						}
						if (y>maxY) {
							maxY=y;
						}
					}
				}
			}
			difX=(maxX-minX)+1;
			difY=(maxY-minY)+1;
			TileBoard foundBoard = new TileBoard(difX,difY);
			for (int x = 0; x < difX; x++) {
				for (int y = 0; y < difY; y++) {
					foundBoard.board[x,y] = new Tile(board[x+minX,y+minY]);
				}
			}
			foundBoard.FloodBoard(new Tile(tile));
			setBoard.overlayOnMe(foundBoard,minX,minY);
			return difX*difY;
		}
		
		public bool checkByState(string State, int originX, int originY, int width, int height) {
			bool output = false;
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (board[originX+x,originY+y].state.Equals(State)) {
						output=true;
					}
				}
			}
			return output;
		}
		
		public void FloodRectangle(string State,int originX, int originY, int width, int height) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					board[originX+x,originY+y] = new Tile(originX+x,originY+y,0,State);
				}				
			}
		}
		
		public void FloodBoard(Tile floodTile) {
			for (int x = 0; x < boardSizeX; x++) {
				for (int y = 0; y < boardSizeY; y++) {
					if (((x+y)%2==0)) {
						board[x,y] = new Tile(floodTile);
						board[x,y].x=x;
						board[x,y].y=y;
					}
				}
			}
		}
		
		public void overlayOnMe(TileBoard overlayTileBoard, int originX, int originY, int width, int height) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					board[x+originX,y+originY] = new Tile(overlayTileBoard.board[x+originX,y+originY]);
				}
			}
		}
		
		public void overlayOnMe(TileBoard overlayTileBoard, int originX, int originY) {
			for (int x = 0; x < overlayTileBoard.boardSizeX; x++) {
				for (int y = 0; y < overlayTileBoard.boardSizeY; y++) {
					board[x+originX,y+originY] = new Tile(overlayTileBoard.board[x,y]);
					board[x+originX,y+originY].x = x+originX; // Updates their x and y
					board[x+originX,y+originY].y = y+originY;
				}
			}
		}
		
		public void ClearBoard() {
			for (int x = 0; x < boardSizeX; x++) {
				for (int y = 0; y < boardSizeY; y++) {
					board[x,y].state="empty";
				}
			}
		}
	}
		
	public class Tile {
		public int x;
		public int y;
		public int ID;
		public string state;
		
		public Tile(int X, int Y, int newID, string State) {
			x=X;
			y=Y;
			ID=newID;
			state=State;
		}
		
		public Tile(int newID, string State) {
			ID=newID;
			state=State;
		}
		
		public Tile(Tile baseTile) {
			x=baseTile.x;
			y=baseTile.y;
			ID=baseTile.ID;
			state=baseTile.state;
		}
	}
	
	public class Enemy {
		public double x;
		public double y;
		public double direction;
		public double speed;
		public Tile curTile;
		public TileBoard board;
		
		public Enemy(double X, double Y, double Direction, double Speed, TileBoard Board) {
			x=X;
			y=Y;
			direction=Direction;
			speed=Speed;
			board=Board;
			curTile=board.board[(int)x,(int)y];
		}
		
		public void Move() {
			double newx;
			double newy;
			newx = speed * Math.Cos(MainForm.DegtoRad(direction))+x;
			newy = speed * Math.Sin(MainForm.DegtoRad(direction))+y;
			x=newx;
			y=newy;
		}
		
		public void CheckWalls() {
			string inwall = "none";
			if (Convert.ToInt16(x)>=board.boardSizeX-1) {
				x=(double)board.boardSizeX-1;
				inwall="lr";
			}
			if (Convert.ToInt16(y)>=board.boardSizeY-1) {
				y=(double)board.boardSizeY-1;
				inwall="ud";
			}
			if (Convert.ToInt16(x)<=0) {
				x=1;
				inwall="lr";
			}
			if (Convert.ToInt16(y)<=0) {
				y=1;
				inwall="ud";
			}
			if (inwall.Equals("ud")) {
			    direction = 360-direction;
			}
			if (inwall.Equals("lr")) {
				direction = (180-direction)%360;
			}
			if ((x==board.boardSizeX-1)&&(y==board.boardSizeY-1)) {
				x=board.boardSizeX/2;
				y=board.boardSizeY/2;
			}
		}
		
		public void ClearMe(Graphics GFX, gameWindow window) {
			curTile.state="empty";
			MainForm.DisplayTile(GFX,curTile,window);
		}
		
		public void DisplayMe(Graphics GFX, gameWindow window) {
			MainForm.DisplayTile(GFX,curTile,window);
		}
		
		public void ChangeTile() {
			curTile=board.board[(int)x,(int)y];
			curTile.state = "enemy";
		}
	}
}
