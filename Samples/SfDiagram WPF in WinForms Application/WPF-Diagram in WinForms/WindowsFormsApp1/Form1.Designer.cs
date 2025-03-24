namespace WindowsFormsApp1
{
    partial class Form1
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
           
            this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
            this.diagramComponent1 = new WPFDiagramLibrary.DiagramComponent();
            this.SuspendLayout();
            // 
            // 
            // elementHost2
            // 
            this.elementHost2.BackColor = System.Drawing.SystemColors.Window;
            this.elementHost2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost2.Location = new System.Drawing.Point(280, 0);
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Size = new System.Drawing.Size(520, 450);
            this.elementHost2.TabIndex = 1;
            this.elementHost2.Text = "elementHost2";
            //Assign a diagram user control as a child for the elementHost2
            this.elementHost2.Child = this.diagramComponent1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            //Add a both Diagram and Stencil elementHosts to the Form1's Control
            this.Controls.Add(this.elementHost2);
           
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Integration.ElementHost elementHost2;
        private WPFDiagramLibrary.DiagramComponent diagramComponent1;
    }
}

