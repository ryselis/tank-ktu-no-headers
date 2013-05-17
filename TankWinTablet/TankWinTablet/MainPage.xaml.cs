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
using Windows.UI.Xaml.Media.Imaging;
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
        public Boolean movingForwardAccelerometer = false;
        public Boolean movingBackwardAccelerometer = false;
        public Boolean rotatingRightAccelerometer = false;
        public Boolean rotatingLeftAccelerometer = false;
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
        public String tankStopRotateButtonAddress = tankIpAddress + "rotate/128";
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
        private int crashCount = 0;
        private Boolean onOffState = false;

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
                _accelerometer.Shaken += _accelerometer_Shaken;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0,0,0,0,70);
                timer.Tick+=main_timer_Tick;
                timer.Start();
            }
            catch 
            {
                _accelerometer = null;
            }
        }

        private void main_timer_Tick(object sender, object e)
        {
            var reading = _accelerometer.GetCurrentReading();
            var x = reading.AccelerationX;
            var y = reading.AccelerationY;
            var z = reading.AccelerationZ;
            //textBox.Text = String.Format("x: {0}, y: {1}, z:{2}", x, y, z);
            if (!movingForwardAccelerometer && y > 0.2)
            {
                movingForwardAccelerometer = true;
                sendTankCommand(tankForwardButtonAddress);
            }
            if (movingForwardAccelerometer && y < 0)
            {
                movingForwardAccelerometer = false;
                sendTankCommand(tankStopMovementButtonAddress);
            }
            if (!movingBackwardAccelerometer && z > 0.3)
            {
                movingBackwardAccelerometer = true;
                sendTankCommand(tankBackwardButtonAddress);
            }
            if (movingBackwardAccelerometer && z < 0)
            {
                movingBackwardAccelerometer = false;
                sendTankCommand(tankStopMovementButtonAddress);
            }
            if (!rotatingLeftAccelerometer && x < -0.4)
            {
                rotatingLeftAccelerometer = true;
                sendTankCommand(tankRotateLeftButtonAddress);
            }
            if (rotatingLeftAccelerometer && x > -0.2)
            {
                rotatingLeftAccelerometer = false;
                sendTankCommand(tankStopRotateButtonAddress);
            }
            if (!rotatingRightAccelerometer && x > 0.4)
            {
                rotatingRightAccelerometer = true;
                sendTankCommand(tankRotateRightButtonAddress);
            }
            if (rotatingRightAccelerometer && x < 0.2)
            {
                rotatingRightAccelerometer = false;
                sendTankCommand(tankStopRotateButtonAddress);
            }
        }

        void _accelerometer_Shaken(Accelerometer sender, AccelerometerShakenEventArgs args)
        {
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

        private void ReadWebRequestCallback(IAsyncResult callbackResult)
        {
            while (true)
            {
                try
                {

                    HttpWebRequest myRequest = (HttpWebRequest) callbackResult.AsyncState;
                    HttpWebResponse myResponse = (HttpWebResponse) myRequest.EndGetResponse(callbackResult);

                    using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
                    {

                        string results = httpwebStreamReader.ReadToEnd();
                    }
                    break;
                }
                catch
                {
                    //textBox.Text = String.Format("{0}", ++crashCount);
                }
            }

        }

        private void sendTankCommand(String tankCommand)
        {
            System.Uri targetUri = new System.Uri(tankCommand);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
        }

        private void ChangeImageBackground(Image image, string adress)
        {
            BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/" + adress + ".png"));
            image.Source = bmp;
        }
//----------------------------------------NEW CONTROLLERS FROM THIS POINT--------------------------------

        private void controlImage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(ForwardButtonImage, "Forward_On");
        }

        private void controlImage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(ForwardButtonImage, "Forward");
         //   sendTankCommand(tankStopMovementButtonAddress);
         //   resetTankStates();
        }

        private void controlImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //ChangeImageBackground(controlImage, "Forward");
        }

        private void controlImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(ForwardButtonImage, "Forward_On");
            if (!movingForward)
            {
                sendTankCommand(tankForwardButtonAddress);
                resetTankStates();
                movingForward = true;
            }
        }

        private void controlImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(ForwardButtonImage, "Forward");
            sendTankCommand(tankStopMovementButtonAddress);
            resetTankStates();
        }

        private void turretLeftImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(turretLeftImageButton, "TLeft_On");
        }

        private void turretLeftImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(turretLeftImageButton, "TLeft");
          //  sendTankCommand(tankStopRotateLeftTurretButtonAddress);
          //  sendTankCommand(tankStopRotateRightTurretButtonAddress);
          //  resetTankStates();
        }

        private void turretLeftImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(turretLeftImageButton, "TLeft_On");
            if (!rotatingTurretLeft)
            {
                sendTankCommand(tankRotateLeftTurretButtonAddress);
                resetTankStates();
                rotatingTurretLeft = true;
            }
        }

        private void turretLeftImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(turretLeftImageButton, "TLeft");
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            resetTankStates();
        }

        private void turretRightImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(turretRightImageButton, "TRight_On");
        }

        private void turretRightImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(turretRightImageButton, "TRight");
          //  sendTankCommand(tankStopRotateLeftTurretButtonAddress);
          //  sendTankCommand(tankStopRotateRightTurretButtonAddress);
          //  resetTankStates();
        }

        private void turretRightImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(turretRightImageButton, "TRight_On");
            if (!rotatingTurretRight)
            {
                sendTankCommand(tankRotateRightTurretButtonAddress);
                resetTankStates();
                rotatingTurretRight = true;
            }
        }

        private void turretRightImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(turretRightImageButton, "TRight");
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            resetTankStates();
        }

        private void stopImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(stopImageButton, "Stop_On");
        }

        private void stopImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(stopImageButton, "Stop");
        }

        private void stopImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(stopImageButton, "Stop_On");
            stopEvents();
        }

        private void stopImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(stopImageButton, "Stop");
        }

        private void leftImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(leftImageButton, "Left_On");
        }

        private void leftImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(leftImageButton, "Left");
           // sendTankCommand(tankStopRotateButtonAddress);
           // resetTankStates();
        }

        private void leftImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(leftImageButton, "Left_On");
            if (!rotatingLeft)
            {
                sendTankCommand(tankRotateLeftButtonAddress);
                resetTankStates();
                rotatingLeft = true;
            }
        }

        private void leftImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(leftImageButton, "Left");
            sendTankCommand(tankStopRotateButtonAddress);
            resetTankStates();
        }

        private void rightImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(rightImageButton, "Right_On");
        }

        private void rightImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(rightImageButton, "Right");
           // sendTankCommand(tankStopRotateButtonAddress);
           // resetTankStates();
        }

        private void rightImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(rightImageButton, "Right_On");
            if (!rotatingRight)
            {
                sendTankCommand(tankRotateRightButtonAddress);
                resetTankStates();
                rotatingRight = true;
            }
        }

        private void rightImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(rightImageButton, "Right");
            sendTankCommand(tankStopRotateButtonAddress);
            resetTankStates();
        }

        private void backImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(backImageButton, "Back_On");
        }

        private void backImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(backImageButton, "Back");
           // sendTankCommand(tankStopMovementButtonAddress);
           // resetTankStates();
        }

        private void backImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(backImageButton, "Back_On");
            if (!movingBackward)
            {
                sendTankCommand(tankBackwardButtonAddress);
                resetTankStates();
                movingBackward = true;
            }
        }

        private void backImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(backImageButton, "Back");
            sendTankCommand(tankStopMovementButtonAddress);
            resetTankStates();
        }

        private void gunLiftImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(gunLiftImageButton, "GLift_On");
        }

        private void gunLiftImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(gunLiftImageButton, "GLift");
            //sendTankCommand(tankStopMainGunMoveButtonAddress);
            //resetTankStates();
        }

        private void gunLiftImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(gunLiftImageButton, "GLift_On");
            if (!movingMainGun)
            {
                sendTankCommand(tankMainGunMoveButtonAddress);
                resetTankStates();
                movingMainGun = true;
            }
        }

        private void gunLiftImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(gunLiftImageButton, "GLift");
            sendTankCommand(tankStopMainGunMoveButtonAddress);
            resetTankStates();
        }

        private void fireImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(fireImageButton, "CFire_On");
        }

        private void fireImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(fireImageButton, "CFire");
            //sendTankCommand(tankStopMainGunFireButtonAddress);
            //resetTankStates();
        }

        private void fireImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(fireImageButton, "CFire_On");
            if (!firingMainGun)
            {
                sendTankCommand(tankMainGunFireButtonAddress);
                resetTankStates();
                firingMainGun = true;
            }
        }

        private void fireImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ChangeImageBackground(fireImageButton, "CFire");
            sendTankCommand(tankStopMainGunFireButtonAddress);
            resetTankStates();
        }

        private void onOffImageButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!onOffState)
            {
                ChangeImageBackground(onOffImageButton, "button-on-push");
            }
            else
            {
                ChangeImageBackground(onOffImageButton, "button-off-push");
            }
        }

        private void onOffImageButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!onOffState)
            {
                ChangeImageBackground(onOffImageButton, "button-on");
            }
            else
            {
                ChangeImageBackground(onOffImageButton, "button-off");
            }
        }

        private void onOffImageButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            sendTankCommand(tankOnButtonAddress);
            sendTankCommand(tankOffButtonAddress);
            if (!onOffState)
            {
                ChangeImageBackground(onOffImageButton, "button-on-push");

            }
            else
            {
                ChangeImageBackground(onOffImageButton, "button-off-push");
            }
        }

        private void onOffImageButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!onOffState)
            {
                ChangeImageBackground(onOffImageButton, "button-off");
                onOffState = true;
            }
            else
            {
                ChangeImageBackground(onOffImageButton, "button-on");
                onOffState = false;
            }
        }
    }
}
