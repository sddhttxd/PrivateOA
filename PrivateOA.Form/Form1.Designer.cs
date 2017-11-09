namespace PrivateOA.Forms
{
    partial class frm_add
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
            this.lb_form1 = new System.Windows.Forms.Label();
            this.dt_start = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lb_kqlx = new System.Windows.Forms.Label();
            this.lb_kssj = new System.Windows.Forms.Label();
            this.lb_jssj = new System.Windows.Forms.Label();
            this.dt_end = new System.Windows.Forms.DateTimePicker();
            this.lb_sc = new System.Windows.Forms.Label();
            this.txt_sc = new System.Windows.Forms.TextBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_form1
            // 
            this.lb_form1.AutoSize = true;
            this.lb_form1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_form1.Location = new System.Drawing.Point(152, 22);
            this.lb_form1.Name = "lb_form1";
            this.lb_form1.Size = new System.Drawing.Size(126, 26);
            this.lb_form1.TabIndex = 0;
            this.lb_form1.Text = "个人考勤记录";
            // 
            // dt_start
            // 
            this.dt_start.AllowDrop = true;
            this.dt_start.Location = new System.Drawing.Point(175, 153);
            this.dt_start.Name = "dt_start";
            this.dt_start.Size = new System.Drawing.Size(167, 21);
            this.dt_start.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(175, 93);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(167, 20);
            this.comboBox1.TabIndex = 2;
            // 
            // lb_kqlx
            // 
            this.lb_kqlx.AutoSize = true;
            this.lb_kqlx.Location = new System.Drawing.Point(86, 96);
            this.lb_kqlx.Name = "lb_kqlx";
            this.lb_kqlx.Size = new System.Drawing.Size(65, 12);
            this.lb_kqlx.TabIndex = 3;
            this.lb_kqlx.Text = "考勤类型：";
            // 
            // lb_kssj
            // 
            this.lb_kssj.AutoSize = true;
            this.lb_kssj.Location = new System.Drawing.Point(86, 159);
            this.lb_kssj.Name = "lb_kssj";
            this.lb_kssj.Size = new System.Drawing.Size(65, 12);
            this.lb_kssj.TabIndex = 4;
            this.lb_kssj.Text = "开始时间：";
            // 
            // lb_jssj
            // 
            this.lb_jssj.AutoSize = true;
            this.lb_jssj.Location = new System.Drawing.Point(86, 218);
            this.lb_jssj.Name = "lb_jssj";
            this.lb_jssj.Size = new System.Drawing.Size(65, 12);
            this.lb_jssj.TabIndex = 5;
            this.lb_jssj.Text = "结束时间：";
            // 
            // dt_end
            // 
            this.dt_end.Location = new System.Drawing.Point(175, 218);
            this.dt_end.Name = "dt_end";
            this.dt_end.Size = new System.Drawing.Size(167, 21);
            this.dt_end.TabIndex = 6;
            this.dt_end.Value = new System.DateTime(2017, 11, 3, 16, 39, 53, 0);
            // 
            // lb_sc
            // 
            this.lb_sc.AutoSize = true;
            this.lb_sc.Location = new System.Drawing.Point(86, 273);
            this.lb_sc.Name = "lb_sc";
            this.lb_sc.Size = new System.Drawing.Size(65, 12);
            this.lb_sc.TabIndex = 7;
            this.lb_sc.Text = "考勤时长：";
            // 
            // txt_sc
            // 
            this.txt_sc.Location = new System.Drawing.Point(181, 272);
            this.txt_sc.Name = "txt_sc";
            this.txt_sc.Size = new System.Drawing.Size(161, 21);
            this.txt_sc.TabIndex = 8;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(175, 337);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 9;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // frm_add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 404);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.txt_sc);
            this.Controls.Add(this.lb_sc);
            this.Controls.Add(this.dt_end);
            this.Controls.Add(this.lb_jssj);
            this.Controls.Add(this.lb_kssj);
            this.Controls.Add(this.lb_kqlx);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dt_start);
            this.Controls.Add(this.lb_form1);
            this.Name = "frm_add";
            this.Text = "添加记录";
            this.Load += new System.EventHandler(this.frm_add_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_form1;
        private System.Windows.Forms.DateTimePicker dt_start;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lb_kqlx;
        private System.Windows.Forms.Label lb_kssj;
        private System.Windows.Forms.Label lb_jssj;
        private System.Windows.Forms.DateTimePicker dt_end;
        private System.Windows.Forms.Label lb_sc;
        private System.Windows.Forms.TextBox txt_sc;
        private System.Windows.Forms.Button btn_OK;
    }
}

