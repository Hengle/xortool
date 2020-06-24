using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace xor
{
	public class Form1 : Form
	{
		private IContainer components;

		private Button button1;

		private TextBox textBox1;

		private Button button2;

		private Button button3;

		private Label label1;

		private Button button4;

		private Label label2;

		public Form1()
		{
			InitializeComponent();
			XorTool.SetText = (Action<string>)Delegate.Combine(XorTool.SetText, new Action<string>(LableSetText));
			XorTool.enablebutton = (Action)Delegate.Combine(XorTool.enablebutton, new Action(enablebut));
		}

		private void enablebut()
		{
			if (base.InvokeRequired)
			{
				BeginInvoke((Action)delegate
				{
					button4.Enabled = true;
				});
			}
			else
			{
				button4.Enabled = true;
			}
		}

		private void LableSetText(string value)
		{
			if (base.InvokeRequired)
			{
				BeginInvoke((Action)delegate
				{
					label2.Text = value;
				});
			}
			else
			{
				label2.Text = value;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (File.Exists(textBox1.Text))
			{
				XorTool.Decrypt(textBox1.Text);
			}
			else
			{
				label2.Text = "不是文件或文件不存在.";
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			FileDialog file = new OpenFileDialog();
			if (file.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = file.FileName;
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			XorTool.Encrypt(textBox1.Text);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			button4.Enabled = false;
			FolderBrowserDialog folder = new FolderBrowserDialog();
			if (folder.ShowDialog() == DialogResult.OK)
			{
				ThreadPool.QueueUserWorkItem(delegate
				{
					XorTool.DecryptFolder(folder.SelectedPath);
					enablebut();
				});
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			button1 = new System.Windows.Forms.Button();
			textBox1 = new System.Windows.Forms.TextBox();
			button2 = new System.Windows.Forms.Button();
			button3 = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			button4 = new System.Windows.Forms.Button();
			label2 = new System.Windows.Forms.Label();
			SuspendLayout();
			button1.Location = new System.Drawing.Point(49, 61);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 0;
			button1.Text = "解密";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(button1_Click);
			textBox1.Location = new System.Drawing.Point(49, 13);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(207, 21);
			textBox1.TabIndex = 1;
			button2.Location = new System.Drawing.Point(277, 12);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(75, 23);
			button2.TabIndex = 2;
			button2.Text = "打开";
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(button2_Click);
			button3.Location = new System.Drawing.Point(160, 62);
			button3.Name = "button3";
			button3.Size = new System.Drawing.Size(75, 23);
			button3.TabIndex = 3;
			button3.Text = "加密";
			button3.UseVisualStyleBackColor = true;
			button3.Click += new System.EventHandler(button3_Click);
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(8, 18);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(41, 12);
			label1.TabIndex = 4;
			label1.Text = "文件：";
			button4.Location = new System.Drawing.Point(277, 62);
			button4.Name = "button4";
			button4.Size = new System.Drawing.Size(75, 23);
			button4.TabIndex = 7;
			button4.Text = "批量解密";
			button4.UseVisualStyleBackColor = true;
			button4.Click += new System.EventHandler(button4_Click);
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(12, 98);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(0, 12);
			label2.TabIndex = 8;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(364, 119);
			base.Controls.Add(label2);
			base.Controls.Add(button4);
			base.Controls.Add(label1);
			base.Controls.Add(button3);
			base.Controls.Add(button2);
			base.Controls.Add(textBox1);
			base.Controls.Add(button1);
			base.Name = "Form1";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
