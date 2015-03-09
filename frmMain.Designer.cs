namespace TestApp1
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpAABB = new System.Windows.Forms.GroupBox();
            this.lblNudBox_H = new System.Windows.Forms.Label();
            this.nudBox_H = new System.Windows.Forms.NumericUpDown();
            this.lblNudBox_W = new System.Windows.Forms.Label();
            this.nudBox_W = new System.Windows.Forms.NumericUpDown();
            this.grpAABB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBox_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBox_W)).BeginInit();
            this.SuspendLayout();
            // 
            // grpAABB
            // 
            this.grpAABB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAABB.Controls.Add(this.lblNudBox_H);
            this.grpAABB.Controls.Add(this.nudBox_H);
            this.grpAABB.Controls.Add(this.lblNudBox_W);
            this.grpAABB.Controls.Add(this.nudBox_W);
            this.grpAABB.Location = new System.Drawing.Point(674, 513);
            this.grpAABB.Name = "grpAABB";
            this.grpAABB.Size = new System.Drawing.Size(98, 73);
            this.grpAABB.TabIndex = 2;
            this.grpAABB.TabStop = false;
            this.grpAABB.Text = "Box";
            // 
            // lblNudBox_H
            // 
            this.lblNudBox_H.AutoSize = true;
            this.lblNudBox_H.Location = new System.Drawing.Point(6, 42);
            this.lblNudBox_H.Name = "lblNudBox_H";
            this.lblNudBox_H.Size = new System.Drawing.Size(18, 13);
            this.lblNudBox_H.TabIndex = 8;
            this.lblNudBox_H.Text = "H:";
            // 
            // nudBox_H
            // 
            this.nudBox_H.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudBox_H.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudBox_H.Location = new System.Drawing.Point(35, 40);
            this.nudBox_H.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudBox_H.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBox_H.Name = "nudBox_H";
            this.nudBox_H.Size = new System.Drawing.Size(57, 20);
            this.nudBox_H.TabIndex = 7;
            this.nudBox_H.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudBox_H.ValueChanged += new System.EventHandler(this.nudBox_ValueChanged);
            // 
            // lblNudBox_W
            // 
            this.lblNudBox_W.AutoSize = true;
            this.lblNudBox_W.Location = new System.Drawing.Point(6, 16);
            this.lblNudBox_W.Name = "lblNudBox_W";
            this.lblNudBox_W.Size = new System.Drawing.Size(21, 13);
            this.lblNudBox_W.TabIndex = 6;
            this.lblNudBox_W.Text = "W:";
            // 
            // nudBox_W
            // 
            this.nudBox_W.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudBox_W.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudBox_W.Location = new System.Drawing.Point(35, 14);
            this.nudBox_W.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudBox_W.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBox_W.Name = "nudBox_W";
            this.nudBox_W.Size = new System.Drawing.Size(57, 20);
            this.nudBox_W.TabIndex = 5;
            this.nudBox_W.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudBox_W.ValueChanged += new System.EventHandler(this.nudBox_ValueChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 598);
            this.Controls.Add(this.grpAABB);
            this.Name = "frmMain";
            this.Text = "AABB tests";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.grpAABB.ResumeLayout(false);
            this.grpAABB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBox_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBox_W)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpAABB;
        private System.Windows.Forms.Label lblNudBox_H;
        private System.Windows.Forms.NumericUpDown nudBox_H;
        private System.Windows.Forms.Label lblNudBox_W;
        private System.Windows.Forms.NumericUpDown nudBox_W;
    }
}

