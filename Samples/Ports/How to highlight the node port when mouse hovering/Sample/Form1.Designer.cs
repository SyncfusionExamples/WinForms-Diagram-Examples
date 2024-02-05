
namespace HighLightPortSample
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
            this.components = new System.ComponentModel.Container();
            Syncfusion.Windows.Forms.Diagram.Binding binding1 = new Syncfusion.Windows.Forms.Diagram.Binding();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.diagram1 = new Syncfusion.Windows.Forms.Diagram.Controls.Diagram(this.components);
            this.model1 = new Syncfusion.Windows.Forms.Diagram.Model(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.diagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.model1)).BeginInit();
            this.SuspendLayout();
            // 
            // diagram1
            // 
            binding1.DefaultConnector = null;
            binding1.DefaultNode = null;
            binding1.Diagram = this.diagram1;
            binding1.Id = null;
            binding1.Label = ((System.Collections.Generic.List<string>)(resources.GetObject("binding1.Label")));
            binding1.ParentId = null;
            this.diagram1.Binding = binding1;
            this.diagram1.Controller.Constraint = Syncfusion.Windows.Forms.Diagram.Constraints.PageEditable;
            this.diagram1.Controller.DefaultConnectorTool = Syncfusion.Windows.Forms.Diagram.ConnectorTool.OrgLineConnectorTool;
            this.diagram1.Controller.PasteOffset = new System.Drawing.SizeF(10F, 10F);
            this.diagram1.EnableTouchMode = false;
            this.diagram1.LayoutManager = null;
            this.diagram1.Location = new System.Drawing.Point(12, 12);
            this.diagram1.Model = this.model1;
            this.diagram1.Name = "diagram1";
            this.diagram1.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagram1.ScrollVirtualBounds")));
            this.diagram1.Size = new System.Drawing.Size(911, 542);
            this.diagram1.SmartSizeBox = false;
            this.diagram1.TabIndex = 0;
            this.diagram1.Text = "diagram1";
            // 
            // 
            // 
            this.diagram1.View.ClientRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.diagram1.View.Controller = this.diagram1.Controller;
            this.diagram1.View.Grid.MinPixelSpacing = 4F;
            this.diagram1.View.ScrollVirtualBounds = ((System.Drawing.RectangleF)(resources.GetObject("resource.ScrollVirtualBounds")));
            this.diagram1.View.ZoomType = Syncfusion.Windows.Forms.Diagram.ZoomType.Center;
            // 
            // model1
            // 
            this.model1.AlignmentType = Syncfusion.Windows.Forms.Diagram.AlignmentType.SelectedNode;
            this.model1.BackgroundStyle.PathBrushStyle = Syncfusion.Windows.Forms.Diagram.PathGradientBrushStyle.RectangleCenter;
            this.model1.DocumentScale.DisplayName = "No Scale";
            this.model1.DocumentScale.Height = 1F;
            this.model1.DocumentScale.Width = 1F;
            this.model1.DocumentSize.Height = 1169F;
            this.model1.DocumentSize.Width = 827F;
            this.model1.LineStyle.DashPattern = null;
            this.model1.LineStyle.LineColor = System.Drawing.Color.Black;
            this.model1.LogicalSize = new System.Drawing.SizeF(827F, 1169F);
            this.model1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.model1.ShadowStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.model1.ShadowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.diagram1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.diagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.model1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Diagram.Controls.Diagram diagram1;
        private Syncfusion.Windows.Forms.Diagram.Model model1;
    }
}

