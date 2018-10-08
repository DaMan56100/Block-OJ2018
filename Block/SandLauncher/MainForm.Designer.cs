/*
 * Created by SharpDevelop.
 * User: Sammy
 * Date: 05/10/2018
 * Time: 20:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace SandLauncher
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.picMain = new System.Windows.Forms.PictureBox();
			this.tckGame = new System.Windows.Forms.Timer(this.components);
			this.tckResize = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
			this.SuspendLayout();
			// 
			// picMain
			// 
			this.picMain.BackColor = System.Drawing.Color.White;
			this.picMain.Cursor = System.Windows.Forms.Cursors.Cross;
			this.picMain.Location = new System.Drawing.Point(0, 0);
			this.picMain.Name = "picMain";
			this.picMain.Size = new System.Drawing.Size(99, 90);
			this.picMain.TabIndex = 0;
			this.picMain.TabStop = false;
			this.picMain.Click += new System.EventHandler(this.PicMainClick);
			// 
			// tckGame
			// 
			this.tckGame.Enabled = true;
			this.tckGame.Tick += new System.EventHandler(this.TckGameTick);
			// 
			// tckResize
			// 
			this.tckResize.Enabled = true;
			this.tckResize.Tick += new System.EventHandler(this.TckResizeTick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.picMain);
			this.Name = "MainForm";
			this.Text = "SandLauncher";
			((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Timer tckGame;
		private System.Windows.Forms.Timer tckResize;
		private System.Windows.Forms.PictureBox picMain;
	}
}
