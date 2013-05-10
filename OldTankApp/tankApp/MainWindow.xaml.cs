using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;

namespace tankApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Boolean startButtonIsDown = false;
        public Boolean movingForward = false;
        public Boolean movingBackward = false;
        public Boolean rotatingRight = false;
        public Boolean rotatingLeft = false;
        public Boolean rotatingTurretLeft = false;
        public Boolean rotatingTurretRight = false;
        public Boolean movingMainGun = false;
        public Boolean firingMainGun = false;
        public static String tankIpAddress = "http://192.168.0.202/";
        public String tankOnButtonAddress = tankIpAddress + "tank/on";
        public String tankOffButtonAddress = tankIpAddress + "tank/off";
        public String tankForwardButtonAddress = tankIpAddress + "move/0";
        public String tankStopMovementButtonAddress = tankIpAddress + "move/127";
        public String tankBackwardButtonAddress = tankIpAddress + "move/255";
        public String tankRotateRightButtonAddress = tankIpAddress + "rotate/255";
        public String tankRotateLeftButtonAddress = tankIpAddress + "rotate/0";
        public String tankStopRotateButtonAddress = tankIpAddress + "rotate/127";
        public String tankRotateRightTurretButtonAddress = tankIpAddress + "turret/left/on";
        public String tankRotateLeftTurretButtonAddress = tankIpAddress + "turret/right/on";
        public String tankStopRotateRightTurretButtonAddress = tankIpAddress + "turret/right/off";
        public String tankStopRotateLeftTurretButtonAddress = tankIpAddress + "turret/left/off";
        public String tankMainGunMoveButtonAddress = tankIpAddress + "main_gun/move/on";
        public String tankStopMainGunMoveButtonAddress = tankIpAddress + "main_gun/move/off";
        public String tankMainGunFireButtonAddress = tankIpAddress + "main_gun/fire/on";
        public String tankStopMainGunFireButtonAddress = tankIpAddress + "main_gun/fire/off";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void startButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void startButton_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            sendTankCommand(tankOnButtonAddress);
            sendTankCommand(tankOffButtonAddress);
        }

        private void resetTankStates()
        {
            startButtonIsDown = false;
            movingForward = false;
            movingBackward = false;
            rotatingRight = false;
            rotatingLeft = false;
            rotatingTurretLeft = false;
            rotatingTurretRight = false;
            movingMainGun = false;
            firingMainGun = false;
        }

        //private void startButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (startButtonIsDown == false)
        //    {
        //        sendTankCommand(tankOnButtonAddress);
        //        startButtonIsDown = true;
        //    }
        //}

        //private void startButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (startButtonIsDown)
        //    {
        //        sendTankCommand(tankOffButtonAddress);
        //        startButtonIsDown = false;
        //    }
        //}

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (!movingForward)
            {
                sendTankCommand(tankForwardButtonAddress);
                resetTankStates();
                movingForward = true;
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            sendTankCommand(tankStopMovementButtonAddress);
            sendTankCommand(tankStopRotateButtonAddress);
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            sendTankCommand(tankStopMainGunMoveButtonAddress);
            sendTankCommand(tankStopMainGunFireButtonAddress);
            resetTankStates();
        }

        private void backwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (!movingBackward)
            {
                sendTankCommand(tankBackwardButtonAddress);
                resetTankStates();
                movingBackward = true;
            }
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            if (!rotatingRight)
            {
                sendTankCommand(tankRotateRightButtonAddress);
                resetTankStates();
                rotatingRight = true;
            }
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (!rotatingLeft)
            {
                sendTankCommand(tankRotateLeftButtonAddress);
                resetTankStates();
                rotatingLeft = true;
            }
        }

        private void turretLeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (!rotatingTurretLeft)
            {
                sendTankCommand(tankRotateLeftTurretButtonAddress);
                resetTankStates();
                rotatingTurretLeft = true;
            }
        }

        private void sendTankCommand(String tankCommand)
        {
            var request = (HttpWebRequest)WebRequest.Create(tankCommand);
            try
            {
                WebResponse serverResponse = request.GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(serverResponse.GetResponseStream(), encoding))
                {
                    consoleWindow.Text += reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Status);
            }
        }

        private void turretRightButton_Click(object sender, RoutedEventArgs e)
        {
            if (!rotatingTurretRight)
            {
                sendTankCommand(tankRotateRightTurretButtonAddress);
                resetTankStates();
                rotatingTurretRight = true;
            }
        }

        private void gunButton_Click(object sender, RoutedEventArgs e)
        {
            if (!movingMainGun)
            {
                sendTankCommand(tankMainGunMoveButtonAddress);
                resetTankStates();
                movingMainGun = true;
            }
        }

        private void fireButton_Click(object sender, RoutedEventArgs e)
        {
            if (!firingMainGun)
            {
                sendTankCommand(tankMainGunFireButtonAddress);
                resetTankStates();
                firingMainGun = true;
            }
        }
    }
}
