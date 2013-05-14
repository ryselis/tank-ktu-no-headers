using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TankWinTablet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public static class WebRequestExtensions
    {
        public static WebResponse GetResponse(this WebRequest request)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            IAsyncResult asyncResult = request.BeginGetResponse(r => autoResetEvent.Set(), null);

            // Wait until the call is finished
            autoResetEvent.WaitOne();

            return request.EndGetResponse(asyncResult);
        }

        public static Stream GetRequestStream(this WebRequest request)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            IAsyncResult asyncResult = request.BeginGetRequestStream(r => autoResetEvent.Set(), null);

            // Wait until the call is finished
            autoResetEvent.WaitOne();

            return request.EndGetRequestStream(asyncResult);
        }
    }

    public sealed partial class MainPage : Page
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
        public bool canSend = true;
        private Accelerometer _accelerometer;

        public MainPage()
        {
            this.InitializeComponent(); 
            InitializeAccelerometer();
        }

        private void InitializeAccelerometer()
        {
            try
            {
                _accelerometer = Accelerometer.GetDefault();
                _accelerometer.ReportInterval = 10;
                _accelerometer.ReadingChanged += _accelerometer_ReadingChanged;
                textBox.Text = "Reading event ok";
                _accelerometer.Shaken += _accelerometer_Shaken;
                textBox.Text = "Skaen event ok";
                textBox.Text = String.Format("{0}", _accelerometer.GetCurrentReading().AccelerationX);
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0,0,0,0,70);
                timer.Tick+=main_timer_Tick;
                timer.Start();
            }
            catch 
            {
                _accelerometer = null;
                textBox.Text = "phailed";
            }
        }

        private void main_timer_Tick(object sender, object e)
        {
            var reading = _accelerometer.GetCurrentReading();
            var x = reading.AccelerationX;
            var y = reading.AccelerationY;
            var z = reading.AccelerationZ;
            textBox.Text = String.Format("x: {0}, y: {1}, z:{2}", x, y, z);
            if (!movingForward && y > 0.2)
            {
                movingForward = true;
                sendTankCommand(tankForwardButtonAddress);
                textBox.Text += tankForwardButtonAddress;
            }
            if (movingForward && y < 0)
            {
                movingForward = false;
                sendTankCommand(tankStopMovementButtonAddress);
                textBox.Text += tankStopMovementButtonAddress;
            }
            if (!movingBackward && z > 0.3)
            {
                movingBackward = true;
                sendTankCommand(tankBackwardButtonAddress);
                textBox.Text += tankBackwardButtonAddress;
            }
            if (movingBackward && z < 0)
            {
                movingBackward = false;
                sendTankCommand(tankStopMovementButtonAddress);
                textBox.Text += tankStopMovementButtonAddress;
            }
            if (!rotatingLeft && x < -0.4)
            {
                rotatingLeft = true;
                sendTankCommand(tankRotateLeftButtonAddress);
            }
            if (rotatingLeft && x > -0.2)
            {
                rotatingLeft = false;
                sendTankCommand(tankStopRotateButtonAddress);
            }
            if (!rotatingRight && x > 0.4)
            {
                rotatingRight = true;
                sendTankCommand(tankRotateRightButtonAddress);
            }
            if (rotatingRight && x < 0.2)
            {
                rotatingRight = false;
                sendTankCommand(tankStopRotateButtonAddress);
            }
        }

        void _accelerometer_Shaken(Accelerometer sender, AccelerometerShakenEventArgs args)
        {
            textBox.Text = "Shaken!!!!";
            sendTankCommand(tankMainGunFireButtonAddress);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan( 0, 0, 0, 2, 0);
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, object e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            timer.Stop();
            sendTankCommand(tankStopMainGunFireButtonAddress);
        }

        void _accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            var reading = _accelerometer.GetCurrentReading();
            var x = reading.AccelerationX;
            var y = reading.AccelerationY;
            var z = reading.AccelerationZ;
            textBox.Text = String.Format("x: {0}, y: {1}, z:{2}", x, y, z);
        }

        private void startButton_ClickNew(object sender, RoutedEventArgs e)
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

        private void forwardButton_ClickNew(object sender, RoutedEventArgs e)
        {
            if (!movingForward)
            {
                sendTankCommand(tankForwardButtonAddress);
                resetTankStates();
                movingForward = true;
            }
        }

        private void stopButton_ClickNew(object sender, RoutedEventArgs e)
        {
            stopEvents();
        }

        private void stopEvents()
        {
            sendTankCommand(tankStopMovementButtonAddress);
            sendTankCommand(tankStopRotateButtonAddress);
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            sendTankCommand(tankStopMainGunMoveButtonAddress);
            sendTankCommand(tankStopMainGunFireButtonAddress);
            resetTankStates();
        }

        private void backwardButton_ClickNew(object sender, RoutedEventArgs e)
        {
            if (!movingBackward)
            {
                sendTankCommand(tankBackwardButtonAddress);
                resetTankStates();
                movingBackward = true;
            }
        }

        private void rightButton_ClickNew(object sender, RoutedEventArgs e)
        {
            if (!rotatingRight)
            {
                sendTankCommand(tankRotateRightButtonAddress);
                resetTankStates();
                rotatingRight = true;
            }
        }

        private void LeftButton_ClickNew(object sender, RoutedEventArgs e)
        {
            if (!rotatingLeft)
            {
                sendTankCommand(tankRotateLeftButtonAddress);
                resetTankStates();
                rotatingLeft = true;
            }
        }

        private void turretLeftButton_ClickNew(object sender, RoutedEventArgs e)
        {
            if (!rotatingTurretLeft)
            {
                sendTankCommand(tankRotateLeftTurretButtonAddress);
                resetTankStates();
                rotatingTurretLeft = true;
            }
        }


        private void ReadWebRequestCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);

            using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
            {
                string results = httpwebStreamReader.ReadToEnd();
            }
        }

        private void sendTankCommand(String tankCommand)
        {
            System.Uri targetUri = new System.Uri(tankCommand);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
        }

        private void DispatcherTimerEventHandler(object sender, object e)
        {
            canSend = true;
        }

        private void turretRightButton_ClickNew(object sender, RoutedEventArgs e)
        {
            if (!rotatingTurretRight)
            {
                sendTankCommand(tankRotateRightTurretButtonAddress);
                resetTankStates();
                rotatingTurretRight = true;
            }
        }

        private void gunButton_ClickNew(object sender, RoutedEventArgs e)
        {
            if (!movingMainGun)
            {
                sendTankCommand(tankMainGunMoveButtonAddress);
                resetTankStates();
                movingMainGun = true;
            }
        }

        private void fireButton_ClickNew(object sender, RoutedEventArgs e)
        {
            if (!firingMainGun)
            {
                sendTankCommand(tankMainGunFireButtonAddress);
                resetTankStates();
                firingMainGun = true;
            }
        }

        private void tankBodyImage_DragEnter(object sender, DragEventArgs e)
        {
         
        }

        private void gunLiftButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!movingMainGun)
            {
                sendTankCommand(tankMainGunMoveButtonAddress);
                resetTankStates();
                movingMainGun = true;
            }
        }

        private void gunLiftButton_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sendTankCommand(tankStopMainGunMoveButtonAddress);
            resetTankStates();
        }

        private void gunLiftButton_Copy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankStopMainGunMoveButtonAddress);
            resetTankStates();
        }

        private void backButton_Copy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankStopMovementButtonAddress);
            resetTankStates();
        }

        private void backButton_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sendTankCommand(tankStopMovementButtonAddress);
            resetTankStates();
        }

        private void fireButton_Copy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankStopMainGunFireButtonAddress);
            resetTankStates();
        }

        private void fireButton_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sendTankCommand(tankStopMainGunFireButtonAddress);
            resetTankStates();
        }

        private void goRightButton_Copy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankStopRotateButtonAddress);
            resetTankStates();
        }

        private void goRightButton_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sendTankCommand(tankStopRotateButtonAddress);
            resetTankStates();
        }

        private void goLeftButton_Copy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankStopRotateButtonAddress);
            resetTankStates();
        }

        private void goLeftButton_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sendTankCommand(tankStopRotateButtonAddress);
            resetTankStates();
        }

        private void turretleftButton_Copy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            resetTankStates();
        }

        private void turretleftButton_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            resetTankStates();
        }

        private void forwardButton_Copy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankStopMovementButtonAddress);
            resetTankStates();
        }

        private void forwardButton_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sendTankCommand(tankStopMovementButtonAddress);
            resetTankStates();
        }

        private void turretRightButton_Copy_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            resetTankStates();
        }

        private void turretRightButton_Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            resetTankStates();
        }


    }
}
