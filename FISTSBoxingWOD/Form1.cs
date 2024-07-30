using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;
using System.Timers;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics;

namespace FISTSBoxingWOD
{
    public partial class Form1 : Form
    {
        private readonly string warmUpsFolder = "C:\\Users\\jorr\\OneDrive - IGT PLC\\Desktop\\Projects\\FISTSBoxingWOD\\Workouts\\WarmUps";
        private readonly string pairedFolder = "C:\\Users\\jorr\\OneDrive - IGT PLC\\Desktop\\Projects\\FISTSBoxingWOD\\Workouts\\Paired";
        private readonly string BagWorkFolder = "C:\\Users\\jorr\\OneDrive - IGT PLC\\Desktop\\Projects\\FISTSBoxingWOD\\Workouts\\BagWork";
        private readonly string coolDownFolder = "C:\\Users\\jorr\\OneDrive - IGT PLC\\Desktop\\Projects\\FISTSBoxingWOD\\Workouts\\CoolDown";
       
        private ProgressBar[] progressBars;
        private System.Windows.Forms.Timer marqueeTimer;
        private Stopwatch stopwatch;
        private System.Windows.Forms.Timer elapsedTimer;
        private string PreviousDaysWorkouts = "C:\\Users\\jorr\\OneDrive - IGT PLC\\Desktop\\Projects\\FISTSBoxingWOD\\Workouts\\PreviousDaysWorkouts";
        private bool isStopwatchRunning = false;
        public Form1()
        {
            InitializeComponent();
            InitializeBackGroundWorker();
            IntializeProgressBars();
            InitializeDurTimer();
            InitializeStopwatch();
            InitializeElapsedTimer();
            progressBar1.Visible = false;
            progressBar2.Visible = false;
            progressBar3.Visible = false;
            progressBar4.Visible = false;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void InitializeBackGroundWorker()
        {
            backgroundWorkerPopulate = new BackgroundWorker();
            backgroundWorkerPopulate.WorkerSupportsCancellation = true;
            backgroundWorkerPopulate.DoWork += backgroundWorkerPopulate_DoWork;
            

        }

        private void IntializeProgressBars()
        {
            progressBars = new ProgressBar[] { progressBar1, progressBar2, progressBar3, progressBar4 };
            foreach (var progressBar in progressBars)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }
        }

        private void InitializeDurTimer()
        {
           
            marqueeTimer = new System.Windows.Forms.Timer();
            marqueeTimer.Interval = 1000;
            marqueeTimer.Tick += MarqueeTimer_Tick;


        }
     
        private void InitializeStopwatch()
        {
           stopwatch = new Stopwatch();

        }

        private void InitializeElapsedTimer()
        {
            elapsedTimer = new System.Windows.Forms.Timer();
            elapsedTimer.Interval = 100; //Update every 100 milliseconds
            elapsedTimer.Tick += ElapsedTimer_Tick;
        }
        private void StartStopwatch()
        {
            stopwatch.Start();
            elapsedTimer.Start();
            isStopwatchRunning = true;

        }
        private void StopStopwatch()
        {
            stopwatch.Stop();
            elapsedTimer.Stop();
            isStopwatchRunning = false;
        }

        private void ResetStopwatch()
        {
            stopwatch.Reset();
            elapsedTimer.Stop();
            label5.Text = "00:00:00";
            isStopwatchRunning = false;
        }

        private void ContinueStopWatch()
        {
            // Resume stopwatch
            stopwatch.Start();
            elapsedTimer.Start();
            isStopwatchRunning = true;
        }

        private void UpdateElapsedTime()
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            label5.Text = elapsed.ToString(@"hh\:mm\:ss");
        }

        private void ElapsedTimer_Tick(object sender, EventArgs e)
        {
            // Update the elapsed timer every sec
            UpdateElapsedTime();
        }

        private void MarqueeTimer_Tick(object sender, EventArgs e)
        {
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.MarqueeAnimationSpeed = 10;
            progressBar2.MarqueeAnimationSpeed = 0;
            progressBar2.MarqueeAnimationSpeed = 10;
            progressBar3.MarqueeAnimationSpeed = 0;
            progressBar3.MarqueeAnimationSpeed = 10;
            progressBar4.MarqueeAnimationSpeed = 0;
            progressBar4.MarqueeAnimationSpeed = 10;
        }


        private void PopulateWarmUpBox()
        {
            var files = Directory.GetFiles(warmUpsFolder, "*.txt");

            if (files.Length == 0)
            {
                MessageBox.Show($"No workout files found in folder 'WarmUps'!");
            }

            Random random = new Random();
            string randomFile = files[random.Next(files.Length)];

            try
            {
                string fullcontent = ReadUntilDuration(randomFile);
                richTextBox1.Text = fullcontent;

                string durationValue = ExtractDurationValue(fullcontent);
                richTextBox5.Text = $"{durationValue}";


            }
            catch (Exception ex)

            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }

        }
        private void PopulatePaired()
        {
            var files = Directory.GetFiles(pairedFolder, "*.txt");

            if (files.Length == 0)
            {
                MessageBox.Show($"No workout files found in folder 'Paired'!");
            }

            Random random = new Random();
            string randomFile = files[random.Next(files.Length)];

            try
            {
                string fullcontent = ReadUntilDuration(randomFile);
                richTextBox2.Text = fullcontent;

                string durationValue = ExtractDurationValue(fullcontent);
                richTextBox6.Text = $"{durationValue}";

            }
            catch (Exception ex)

            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }

        }
        private void PopulateBagWork()
        {
            var files = Directory.GetFiles(BagWorkFolder, "*.txt");

            if (files.Length == 0)
            {
                MessageBox.Show($"No workout files found in folder 'BagWork'!");
            }

            Random random = new Random();
            string randomFile = files[random.Next(files.Length)];

            try
            {
                string fullcontent = ReadUntilDuration(randomFile);
                richTextBox3.Text = fullcontent;

                string durationValue = ExtractDurationValue(fullcontent);
                richTextBox7.Text = $"{durationValue}";

            }
            catch (Exception ex)

            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }

        }
        private void PopulateCoolDown()
        {
            var files = Directory.GetFiles(coolDownFolder, "*.txt");

            if (files.Length == 0)
            {
                MessageBox.Show($"No workout files found in folder 'CoolDown'!");
            }

            Random random = new Random();
            string randomFile = files[random.Next(files.Length)];

            try
            {
                string fullcontent = ReadUntilDuration(randomFile);
                richTextBox4.Text = fullcontent;

                string durationValue = ExtractDurationValue(fullcontent);
                richTextBox8.Text = $"{durationValue}";

            }
            catch (Exception ex)

            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }

        }

        private string ReadUntilDuration(string filepath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    string line;
                    string result = "";

                    while ((line = reader.ReadLine()) != null)
                    {
                        result += line + Environment.NewLine;
                        //Check for the target word Duration:
                        if (line.Contains("Duration:"))
                            break;

                    }
                    return result;
                }
            }
            //}
        catch (Exception ex)
        {
            
            return string.Empty;
        }
        }

        private string ExtractDurationValue(string input)
        {
            // Use regular expression to extract the number value
            Match match = Regex.Match(input, @"Duration:\s*(.+)");
            if (match.Success)
            {

                return match.Groups[1].Value;
            }
            else
            { return "0"; }

        }




            private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            ClearAllBoxes(); //ClearAll Boxes
            StopStopwatch();
            ResetStopwatch();
            StartStopwatch();
            backgroundWorkerPopulate.RunWorkerAsync();
        }

        private void ShowProgressBar(ProgressBar progressBar)
        {
            
            if (progressBar.InvokeRequired) 
            {
                progressBar.Invoke(new Action<ProgressBar>(ShowProgressBar), progressBar);
            }
            else
            {
                progressBar.Visible = true;
                marqueeTimer.Start();
            }
        }

        private void HideProgressBar(ProgressBar progressBar)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<ProgressBar>(HideProgressBar), progressBar);
            }
            {
                progressBar.Visible = false;
                marqueeTimer.Stop();
            }
        }
        private void HideAllProgressBars()
        {
            foreach (var progressBar in progressBars)
            {

                HideProgressBar(progressBar);
            }


        }

        private void SaveWorkoutToFile()
        {
            // Create dir if it doesnt exist
            if (!Directory.Exists(PreviousDaysWorkouts))
            {
                Directory.CreateDirectory(PreviousDaysWorkouts);
            }

            // Generate a file with the current date
            string filename = Path.Combine(PreviousDaysWorkouts, $"{DateTime.Now:dddMMMddyyyy}.txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    // Save the content of the RTB
                    writer.WriteLine("WarmUp:");
                    writer.WriteLine(richTextBox1.Text);
                    //writer.WriteLine();

                    writer.WriteLine("OneOnOne:");
                    writer.WriteLine(richTextBox2.Text);
                    //writer.WriteLine();

                    writer.WriteLine("BagWork:");
                    writer.WriteLine(richTextBox3.Text);
                    //writer.WriteLine();

                    writer.WriteLine("CoolDown:");
                    writer.WriteLine(richTextBox4.Text);
                    //writer.WriteLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving workout: {ex.Message}");
            }
        }

        private void ClearAllBoxes()
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            richTextBox7.Clear();
            richTextBox8.Clear();

        }

      
        private void backgroundWorkerPopulate_DoWork(object sender, DoWorkEventArgs e)
        {
            //Update the current date
            this.Invoke((MethodInvoker)delegate
            {
                lblDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            });

            ShowProgressBar(progressBar4);
            Task.Delay(3000).Wait();
           
            PopulateWarmUpBox();
            HideProgressBar(progressBar4);

            ShowProgressBar(progressBar1);
            Task.Delay(3000).Wait();
            PopulatePaired();
            HideProgressBar(progressBar1);

            ShowProgressBar(progressBar2);
            Task.Delay(3000).Wait();
            PopulateBagWork();
            HideProgressBar(progressBar2);

            ShowProgressBar(progressBar3);
            Task.Delay(3000).Wait();
            PopulateCoolDown();
            HideProgressBar(progressBar3);

            HideAllProgressBars();
            SaveWorkoutToFile();


            //enable button
            this.Invoke((MethodInvoker)delegate
            {
                button1.Enabled = true;
            });
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            StopStopwatch();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetStopwatch();
        }

        private void backgroundWorkerPopulate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ContinueStopWatch();
        }
    }
}
