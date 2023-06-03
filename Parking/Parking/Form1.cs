using Parking.Entities;
using Parking.Entities.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Parking
{
    public partial class Parking : Form
    {
        List<ParkingSpot> parkingSpots = new List<ParkingSpot>();
        private Form currentChildForm;

        internal bool CheckNumberPlate(Vehicle vehicle)
        {
            bool result = parkingSpots.Any(obj => obj.Vehicle != null && obj.Vehicle.NumberPlate == vehicle.NumberPlate);

            return result;
        }


        internal bool CheckParkingAvailability(int x)
        {
            bool result = parkingSpots.Any(obj => obj.Id == x && obj.Status == Availability.Available);

            return result;
        }

        public Parking()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
            {
                ParkingSpot parkingSpot = new ParkingSpot(i + 1);
                parkingSpots.Add(parkingSpot);
            }
        }

        private void Parking_Load(object sender, EventArgs e)
        {
            lvVehicleEntrance.View = View.Details;
            lvVehicleEntrance.FullRowSelect = true;
            lvVehicleEntrance.Columns.Add("N°", 60, HorizontalAlignment.Center);
            lvVehicleEntrance.Columns.Add("Type Vehicle", 110, HorizontalAlignment.Left);
            lvVehicleEntrance.Columns.Add("Number Plate", 120, HorizontalAlignment.Left);
            lvVehicleEntrance.Columns.Add("Entry Time", 200, HorizontalAlignment.Left);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = int.Parse(cbxParkingSpace.Text);
            Vehicle vehicle = new Vehicle(cbxVehicleType.Text, txbNumberPlate.Text);

            if (!CheckParkingAvailability(n))
            {
                MessageBox.Show("The spot is already taken by a vehicle", "Registration status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CheckNumberPlate(vehicle))
            {
                MessageBox.Show($"The vehicle is already occupying a spot", "Registration status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                foreach (ParkingSpot obj in parkingSpots)
                {
                    if (obj.Id == n)
                    {
                        obj.AddVehicle(vehicle);
                        MessageBox.Show("Vehicle successfully registered", "Registration status", MessageBoxButtons.OK);

                        ListViewItem listView = new ListViewItem(obj.Id.ToString());
                        listView.SubItems.Add(obj.Vehicle.TypeOfVehicle);
                        listView.SubItems.Add(obj.Vehicle.NumberPlate);
                        listView.SubItems.Add(obj.EntryTime.ToString());
                        listView.SubItems.Add(obj.TimeOfPermanency.ToString());

                        lvVehicleEntrance.Items.Add(listView);

                        string sPath = AppDomain.CurrentDomain.BaseDirectory;
                        string sFile = Path.Combine(sPath, @"..\..\..\Report\ReportEntryVehicles.txt");


                        try
                        {
                            File.AppendAllText(sFile, obj.ToString() + "\n");
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show($"The data was not registered in the report\n{ex.Message}", "Registration status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        txbNumberPlate.Clear();
                        cbxParkingSpace.SelectedIndex = -1;
                        cbxVehicleType.SelectedIndex = -1;
                    }
                }
            }
        }



        private void buttonRemoveVehicle_Click(object sender, EventArgs e)
        {

            textBoxPaymentParkingSpace.Text = lvVehicleEntrance.SelectedItems[0].SubItems[0].Text;
            textBoxPaymentVehicleType.Text = lvVehicleEntrance.SelectedItems[0].SubItems[1].Text;
            textBoxPaymentNumberPalte.Text = lvVehicleEntrance.SelectedItems[0].SubItems[2].Text;
            textBoxPaymentEntryTime.Text = lvVehicleEntrance.SelectedItems[0].SubItems[3].Text;
            textBoxPaymentExistTime.Text = DateTime.Now.ToString();


            int n = int.Parse(lvVehicleEntrance.SelectedItems[0].SubItems[0].Text);


            foreach (ParkingSpot obj in parkingSpots)
            {
                if (obj.Id == n)
                {
                    obj.ExitTime = DateTime.Now;
                    obj.TimePermanency();
                    textBoxPaymentPricePerPeriod.Text = "$ " + obj.PricePerPeriod().ToString("F2");
                    labelTotalTime.Text = obj.TimeOfPermanency.ToString(@"hh\:mm\:ss");


                    string sPath = AppDomain.CurrentDomain.BaseDirectory;
                    string sFile = Path.Combine(sPath, @"..\..\..\Report\ReportExitVehicles.txt");

                    try
                    {
                        File.AppendAllText(sFile, obj.ToString() + "\n");
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"The data was not registered in the report\n{ex.Message}", "Registration status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    obj.RemoveVehicle();

                    lvVehicleEntrance.Items.RemoveAt(lvVehicleEntrance.SelectedIndices[0]);

                }


            }

        }

        private void textBoxPaymentPricePerPeriod_TextChanged(object sender, EventArgs e)
        {

        }
    }


}

