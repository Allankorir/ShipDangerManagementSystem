using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ShipDangerManagementSystem
{
    public partial class Form1 : Form
    {
        private Color selectedDangerColor;
        private List<DangerEvent> currentDangers = new List<DangerEvent>();
        private List<DangerEvent> dangerHistory = new List<DangerEvent>();
        private Button selectedButton = null;
        private DateTime startTime;
        public class DangerEvent
        {
            public string RoomName { get; set; }
            public string DangerType { get; set; }
            public DateTime TimeStamp { get; set; }
            public bool IsDeleted { get; set; }

            public DangerEvent(string roomName, string dangerType, DateTime timeStamp, bool isDeleted = false)
            {
                RoomName = roomName;
                DangerType = dangerType;
                TimeStamp = timeStamp;
                IsDeleted = isDeleted;
            }
        }

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1000; 
            timer1.Tick += timer1_Tick;

        }
        private void HighlightButtonStroke(Button button)
        {
            using (var pen = new Pen(button.ForeColor, 3))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                button.CreateGraphics().DrawRectangle(pen, button.ClientRectangle);
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        

        private void addButton_Click(object sender, EventArgs e)
        {
           
            pictureBox1.BackColor = Color.Red;
            lblRoomName.Font = new Font(lblRoomName.Font, FontStyle.Bold);
            using (var pen = new Pen(Color.Black, 3))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                pictureBox1.CreateGraphics().DrawRectangle(pen, pictureBox1.ClientRectangle);
                pictureBox3.BackColor = Color.Blue;
                pictureBox4.BackColor = Color.Yellow;
                lblRoomName.Font = new Font(lblRoomName.Font, FontStyle.Regular);
                pictureBox3.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Set the background color for the selected room (PictureBox1) to indicate water damage
            pictureBox3.BackColor = Color.Blue;
            // Set the font of the room name label to a regular font
            lblRoomName.Font = new Font(lblRoomName.Font, FontStyle.Regular);
            // Remove the dashed border (if any) around the room
            pictureBox3.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           pictureBox4.BackColor = Color.Yellow;
            lblRoomName.Font = new Font(lblRoomName.Font, FontStyle.Regular);
            pictureBox4.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.Red;
            lblRoomName.Font = new Font(lblRoomName.Font, FontStyle.Bold);
            HighlightButtonStroke(btnFireHazard);

            selectedDangerColor = pictureBox.BackColor;
            btnFireHazard.ForeColor = selectedDangerColor;

            var dangerEvent = new DangerEvent(lblRoomName.Text, "Fire Hazard", DateTime.Now);
            currentDangers.Add(dangerEvent);
            listBoxCurrent.Items.Add($"{dangerEvent.RoomName} - {dangerEvent.DangerType} ({dangerEvent.TimeStamp})");
            startTime = DateTime.Now;
            timer1.Start();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.Blue;
            label2.Font = new Font(label2.Font, FontStyle.Regular);
            HighlightButtonStroke(button2);

            selectedDangerColor = pictureBox.BackColor;

            

            button2.ForeColor = selectedDangerColor;
            var dangerEvent = new DangerEvent(label2.Text, "Water Damage", DateTime.Now);
            currentDangers.Add(dangerEvent);

            
            listBoxCurrent.Items.Add($"{dangerEvent.RoomName} - {dangerEvent.DangerType} ({dangerEvent.TimeStamp})");

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.Yellow;
            label1.Font = new Font(label1.Font, FontStyle.Regular);

            selectedDangerColor = pictureBox.BackColor;

            

            button3.ForeColor = selectedDangerColor;
            var dangerEvent = new DangerEvent(label1.Text, "Electrical Fault", DateTime.Now);
            currentDangers.Add(dangerEvent);

            // Add the danger event to the onscreen list
            listBoxCurrent.Items.Add($"{dangerEvent.RoomName} - {dangerEvent.DangerType} ({dangerEvent.TimeStamp})");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string selectedItem = listBoxCurrent.SelectedItem?.ToString();
            if (selectedItem != null)
            {
                // Find the corresponding danger event in the current danger list
                var dangerEvent = currentDangers.Find(d => $"{d.RoomName} - {d.DangerType} ({d.TimeStamp})".Equals(selectedItem));

                // If the danger event is found, mark it as deleted and move it to the history list
                if (dangerEvent != null)
                {
                    dangerEvent.IsDeleted = true;
                    dangerEvent.TimeStamp = DateTime.Now;
                    dangerHistory.Add(dangerEvent);

                    // Remove the danger event from the current danger list
                    currentDangers.Remove(dangerEvent);

                    // Remove the danger event from the onscreen list
                    listBoxCurrent.Items.Remove(selectedItem);

                    // Add the danger event to the history list onscreen
                    listBoxHistory.Items.Add($"{dangerEvent.RoomName} - {dangerEvent.DangerType} (Deleted at {dangerEvent.TimeStamp})");
                }
            }
        }

        private void listBoxCurrent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now - startTime;
            lblElapsedTime.Text = $"{elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all inputs and lists?", "Clear All Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Clear the PictureBoxes
                pictureBox1.BackColor = SystemColors.Control;
                pictureBox3.BackColor = SystemColors.Control;
                pictureBox4.BackColor = SystemColors.Control;

                // Reset the labels and button colors
                lblRoomName.Font = new Font(lblRoomName.Font, FontStyle.Regular);
               btnFireHazard.ForeColor = Color.Black;
                button2.ForeColor = Color.Black;
                button3.ForeColor = Color.Black;

                // Clear the current danger list and list box
                currentDangers.Clear();
                listBoxCurrent.Items.Clear();
                listBoxHistory.Items.Clear();

                // Stop and reset the timer
                timer1.Stop();
                lblElapsedTime.Text = "00:00:00";

                SaveHistoryLists();
            }
        }
            private void SaveHistoryLists()
            {
                if (MessageBox.Show("Do you want to save history lists?", "Save History Lists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Save History Lists";
                    saveFileDialog.FileName = "history_lists.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            foreach (var dangerEvent in dangerHistory)
                            {
                                writer.WriteLine($"{dangerEvent.RoomName} - {dangerEvent.DangerType} ({dangerEvent.TimeStamp})");
                            }
                        }
                    }
                }
            }

        

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
    }
