using System;
using System.Windows.Forms;
using System.Drawing;
using Autodesk.AutoCAD.Runtime;
using acad = Autodesk.AutoCAD.ApplicationServices.Application;

namespace TestApp
{
 
    public class MyDialog : Form
    {
        public Button buttonOk;
        public Button buttonCancel;
        public TextBox textBoxMinutes;
        public TextBox textBoxSeconds;
        public Label labelMessage;

        private int remainingTime;
        private Timer countdownTimer;
       

        public MyDialog()
        {
            this.buttonOk = new Button();
            this.buttonCancel = new Button();
            this.textBoxMinutes = new TextBox();
            this.textBoxSeconds = new TextBox();
            this.labelMessage = new Label();

            this.buttonOk.Location = new Point(40, 140);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Start";
            this.buttonOk.UseVisualStyleBackColor = true;

            this.buttonCancel.Location = new Point(133, 140);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;

            this.textBoxMinutes.Location = new Point(40, 40);
            this.textBoxMinutes.Name = "textBoxMinutes";
            this.textBoxMinutes.Size = new Size(75, 20);
            this.textBoxMinutes.TabIndex = 6;

            this.textBoxSeconds.Location = new Point(133, 40);
            Label labelseconds = new Label();
            labelseconds.Text = "seconds";
            labelseconds.Location = new Point(133,20);
            this.textBoxSeconds.Name = "textBoxSeconds";
            this.textBoxSeconds.Size = new Size(75, 20);
            this.textBoxSeconds.TabIndex = 7;

            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new Point(40, 80);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new Size(0, 13);
            this.labelMessage.TabIndex = 8;

            this.ClientSize = new Size(250, 180);
            this.Controls.Add((Control)this.labelMessage);
            this.Controls.Add((Control)this.textBoxSeconds);
            this.Controls.Add((Control)this.textBoxMinutes);
            this.Controls.Add((Control)this.buttonCancel);
            this.Controls.Add((Control)this.buttonOk);

            this.Name = nameof(MyDialog);
            this.Text = "Dialog ";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);

            this.countdownTimer = new Timer();
            this.countdownTimer.Interval = 1000;
            this.countdownTimer.Tick += new EventHandler(CountdownTimer_Tick);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            int minutes;
            int seconds;

            if (int.TryParse(textBoxMinutes.Text, out minutes) && int.TryParse(textBoxSeconds.Text, out seconds))
            {
            
                labelMessage.Text = "Введено корректное время.";
                remainingTime = minutes * 60 + seconds; 
                countdownTimer.Start();
            }
            else
            {
                labelMessage.Text = "Некорректный ввод времени.";
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            countdownTimer.Stop();
            this.Close();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            remainingTime--;
            if (remainingTime <= 0)
            {
                countdownTimer.Stop();
                labelMessage.Text = "Время истекло.";
            }
            else
            {
                int minutes = remainingTime / 60;
                int seconds = remainingTime % 60;
                labelMessage.Text = $"{minutes:D2}:{seconds:D2}";
            }
        }
    }

    public class Commands : IExtensionApplication
    {
     
        public void Initialize()
        {
            acad.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\nEnter \"TestCommand\" to bring up the application...\n" + Environment.NewLine);
        }

        
        public void Terminate()
        {

        }

      
        [CommandMethod("TestCommand")]
        public void MyCommand()
        {
            var newDialog = new MyDialog();
            newDialog.Show();
        }
    }
}
