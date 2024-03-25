namespace EscPosPlugin
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.Console = new System.Windows.Forms.ListBox();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.wNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Console
            // 
            this.Console.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(14)))));
            this.Console.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Console.Dock = System.Windows.Forms.DockStyle.Top;
            this.Console.ForeColor = System.Drawing.SystemColors.Window;
            this.Console.FormattingEnabled = true;
            this.Console.Location = new System.Drawing.Point(0, 24);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(384, 325);
            this.Console.TabIndex = 0;
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wNToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(384, 24);
            this.menu.TabIndex = 1;
            this.menu.Text = "menuStrip1";
            // 
            // wNToolStripMenuItem
            // 
            this.wNToolStripMenuItem.Name = "wNToolStripMenuItem";
            this.wNToolStripMenuItem.Size = new System.Drawing.Size(133, 20);
            this.wNToolStripMenuItem.Text = "Registrar Notificacion";
            this.wNToolStripMenuItem.Click += new System.EventHandler(this.wNToolStripMenuItem_Click);
            // 
            // Browser
            // 
            this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Browser.Location = new System.Drawing.Point(0, 349);
            this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(384, 20);
            this.Browser.TabIndex = 2;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.MaximumSize = new System.Drawing.Size(400, 400);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "GUI";
            this.Text = "EscPosPlugin GUI";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Console;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem wNToolStripMenuItem;
        private System.Windows.Forms.WebBrowser Browser;
    }
}