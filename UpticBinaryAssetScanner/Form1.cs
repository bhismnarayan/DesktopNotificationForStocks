using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpticBinaryAssetScanner
{
    public partial class Form1 : Form
    {
        public static string emailaddress = "";
        private JArray quotes;

        public Form1()
        {
           
            InitializeComponent();

            using(var sr = new System.IO.StreamReader(Path.Combine(Application.StartupPath, "quotes.json")))
            {
                quotes = JArray.Parse(sr.ReadToEnd());
            }
           ShowDialog("Enter Email", "For mail send");
            timer1.Start();
        }

        public  void  TecnicalAnalisis_OnAnalisisResult(TechnicalAnalisis result)
        {
            var results = result.AnalisisResult;

          //  result.OnAnalisisResult -= TecnicalAnalisis_OnAnalisisResult;


            if (results.Keys.Contains<String>("Overbought"))
            {
                if (results["Overbought"]  >= 4)
                {
                    
                    // timestamp - <assetname> - Status (overbought or oversold)
                    notifyIcon1.ShowBalloonTip(
                        5000,
                        "UpticBinary Asset Scanner - by Jolat'e",
                        String.Format("{0} - {1} - Status Overbought", DateTime.Now.ToLongDateString(), result.Quote["Pair"] ),
                        ToolTipIcon.Info
                    );
                    string body = "Dear Uptick Analyst,\n \n The recent scan at " + DateTime.Now.ToLongDateString() + "on "+ result.Quote["Pair"] + " found the following saturated items: \nOverbought"+ result.assestsResult["Overbought"];
                    EmailSending.EmailSend(emailaddress, "Uptick Scanner", body);
                }
                else if (results["Overbought"] == 3)
                {
                    notifyIcon1.ShowBalloonTip(
                        5000,
                        "UpticBinary Asset Scanner - by Jolat'e",
                        String.Format("{0} - {1} - Status Pending", DateTime.Now.TimeOfDay, result.Quote["Pair"]),
                        ToolTipIcon.Info
                    );
                    string body = "Dear Uptick Analyst,\n \n The recent scan at " + DateTime.Now.ToLongDateString() + "on " + result.Quote["Pair"] + " found the following saturated items: \nOverbought" + result.assestsResult["Overbought"];
                    EmailSending.EmailSend(emailaddress, "Uptick Scanner", body);
                }
            }
            if (results.Keys.Contains<String>("Oversold"))
            {
                if (results["Oversold"] >= 4)
                {
                    notifyIcon1.ShowBalloonTip(
                        5000,
                        "UpticBinary Asset Scanner",
                        String.Format("{0} - {1} - Status Oversold", DateTime.Now.TimeOfDay, result.Quote["Pair"]),
                        ToolTipIcon.Info
                    );
                    string body = "Dear Uptick Analyst,\n \n The recent scan at " + DateTime.Now.ToLongDateString() + "on " + result.Quote["Pair"] + " found the following saturated items: \nOversold" + result.assestsResult["Oversold"];
                    EmailSending.EmailSend(emailaddress, "Uptick Scanner", body);

                }
                else if (results["Oversold"] == 3)
                {
                    notifyIcon1.ShowBalloonTip(
                        5000,
                        "UpticBinary Asset Scanner",
                        String.Format("{0} - {1} - Status Pending", DateTime.Now.TimeOfDay, result.Quote["Pair"]),
                        ToolTipIcon.Info
                    );
                    string body = "Dear Uptick Analyst,\n \n The recent scan at " + DateTime.Now.ToLongDateString() + "on " + result.Quote["Pair"] + " found the following saturated items: \nOversold" + result.assestsResult["Oversold"];
                    EmailSending.EmailSend(emailaddress, "Uptick Scanner", body);
                }

            }

          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (JObject qt in quotes)
            {
                var tecnicalAnalisis = new TechnicalAnalisis(qt);
                tecnicalAnalisis.OnAnalisisResult += TecnicalAnalisis_OnAnalisisResult;
                TecnicalAnalisis_OnAnalisisResult(tecnicalAnalisis);
                tecnicalAnalisis.Analize();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Hi ");
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    ShowDialog("Enter Email", "For mail send");
        //}
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            
            return prompt.ShowDialog() == DialogResult.OK ? emailaddress=textBox.Text : "";
            
          //  emailaddress = textBox.Text.ToString();
        }
    }
}
