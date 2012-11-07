using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace SingleFileSolution
{
	public class ContentView : UIView
	{
		protected UIColor fillColor;
		protected String extraText; 
		
		public ContentView (UIColor fillColor, String extraText)
		{
			this.fillColor = fillColor;
			this.extraText = extraText;
		}
		
		public override void Draw (System.Drawing.RectangleF rect)
		{
			var frameDescription = String.Format ("ContentView frame is [{0},{1}],[{2},{3}]", Frame.Left, Frame.Top, Frame.Right, Frame.Bottom);
			
			var ulTolr = new PointF[] { rect.Location, rect.Location + rect.Size };
			var urToll = new PointF[] { new PointF(rect.Right, rect.Top), new PointF(rect.Left, rect.Bottom) };
			using(var g = UIGraphics.GetCurrentContext ())
			{
				//Fill the view with color
				fillColor.SetFill();
				g.FillRect(rect);
				
				//Draw diagonals to indicate off-screen geometry
				var red = UIColor.Red;
				red.SetFill();
				red.SetStroke();
				g.AddLines(ulTolr);
				g.AddLines(urToll);
				g.DrawPath(CGPathDrawingMode.Stroke);
				
				//Show some explanatory text
				g.ScaleCTM(1f,-1f);
				g.SelectFont("Arial",16f,CGTextEncoding.MacRoman);
				UIColor.Black.SetFill();
				g.SetTextDrawingMode(CGTextDrawingMode.Fill);
				g.ShowTextAtPoint(10,-50,frameDescription);
				g.ShowTextAtPoint(10,-75,extraText);
			}
		}
	}

	public class UIScrollViewRemarksViewController : UIViewController
	{
		public UIScrollViewRemarksViewController () : base ()
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			//Create a ScrollView with a ContentSize sized to desired scrolling extents
			var scrollView = new UIScrollView();
			scrollView.ContentSize = new SizeF(500,500);
			scrollView.ZoomScale = 1.0f;
			
			//Create a series of content views that will show various aspects of scrolling
			//Basic view; neatly aligned
			var contentView0 = new ContentView(UIColor.Green, "Normal case: neatly aligned");
			contentView0.Frame = new RectangleF(0, 0, 300, 300);
			
			//View with area not reachable by scrolling. Also creates "gap" between contentView1 and contentView2
			var contentView1 = new ContentView(UIColor.Purple, "Frame extends outside of ContentSize");
			contentView1.Frame = new Rectangle(300, -20, 300, 300);
			
			//Basic view; neatly aligned. Sized to require scrolling but aligned with scrollView.ContentSize
			var contentView2 = new ContentView(UIColor.Yellow, "Note how you can't scroll from gap (black) between views");
			contentView2.Frame = new Rectangle(0, 300, 500, 200);
			
			scrollView.AddSubview(contentView0);
			scrollView.AddSubview(contentView1);
			scrollView.AddSubview(contentView2);
			this.View = scrollView;
			
			scrollView.Scrolled += (s,e) => {
				Console.WriteLine (String.Format ("ContentOffset = {0}", scrollView.ContentOffset));
			};
			
			scrollView.DraggingStarted += (object sender, EventArgs e) => {
				Console.WriteLine ("Scrolling started");
			};
		}
	}	

	[Register ("AppDelegate")]
	public  class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		UIScrollViewRemarksViewController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new UIScrollViewRemarksViewController ();
			window.RootViewController = viewController;

			window.MakeKeyAndVisible ();
			
			return true;
		}
	}

	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
