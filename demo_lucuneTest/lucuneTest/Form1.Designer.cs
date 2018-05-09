namespace lucuneTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtWords = new System.Windows.Forms.TextBox();
            this.btnCutWords = new System.Windows.Forms.Button();
            this.txtWords2 = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(413, 48);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtWords2);
            this.panel1.Controls.Add(this.btnCutWords);
            this.panel1.Controls.Add(this.txtWords);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 92);
            this.panel1.TabIndex = 5;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(36, 29);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(328, 21);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "天龙八部";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtResult);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 92);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(884, 378);
            this.panel2.TabIndex = 6;
            // 
            // txtWords
            // 
            this.txtWords.Location = new System.Drawing.Point(548, 12);
            this.txtWords.Multiline = true;
            this.txtWords.Name = "txtWords";
            this.txtWords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtWords.Size = new System.Drawing.Size(324, 38);
            this.txtWords.TabIndex = 7;
            // 
            // btnCutWords
            // 
            this.btnCutWords.Location = new System.Drawing.Point(413, 19);
            this.btnCutWords.Name = "btnCutWords";
            this.btnCutWords.Size = new System.Drawing.Size(75, 23);
            this.btnCutWords.TabIndex = 8;
            this.btnCutWords.Text = "分词--》";
            this.btnCutWords.UseVisualStyleBackColor = true;
            this.btnCutWords.Click += new System.EventHandler(this.btnCutWords_Click);
            // 
            // txtWords2
            // 
            this.txtWords2.Location = new System.Drawing.Point(548, 51);
            this.txtWords2.Multiline = true;
            this.txtWords2.Name = "txtWords2";
            this.txtWords2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtWords2.Size = new System.Drawing.Size(324, 38);
            this.txtWords2.TabIndex = 9;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(0, 0);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(884, 378);
            this.txtResult.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(513, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "盘古";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(513, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "标准";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 470);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnCutWords;
        private System.Windows.Forms.TextBox txtWords;
        private System.Windows.Forms.TextBox txtWords2;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

