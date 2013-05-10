using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


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

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>

        /*private void OnOffButtonClick(object sender, RoutedEventArgs e)
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

        private void stopButtonClick(object sender, RoutedEventArgs e)
        {
            sendTankCommand(tankStopMovementButtonAddress);
            sendTankCommand(tankStopRotateButtonAddress);
            sendTankCommand(tankStopRotateLeftTurretButtonAddress);
            sendTankCommand(tankStopRotateRightTurretButtonAddress);
            sendTankCommand(tankStopMainGunMoveButtonAddress);
            sendTankCommand(tankStopMainGunFireButtonAddress);
            resetTankStates();
        }

        private async void sendTankCommand(String tankCommand)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tankCommand);
            HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
            int k = 0;


            /*HttpClient httpClient = new HttpClient();
            string result = "";
            try
            {
                result = await httpClient.GetStringAsync(tankCommand);
            }
            catch
            {
                //Error handling
            }
        }
    
        private void ForwardButtonClick(object sender, KeyRoutedEventArgs e)
        {
            if (!movingForward)
            {
                sendTankCommand(tankForwardButtonAddress);
                resetTankStates();
                movingForward = true;
            }
        }

        private void TurretRightClick(object sender, KeyRoutedEventArgs e)
        {
            if (!rotatingTurretRight)
            {
                sendTankCommand(tankRotateRightTurretButtonAddress);
                resetTankStates();
                rotatingTurretRight = true;
            }
        }

        private void TurretLeftClick(object sender, KeyRoutedEventArgs e)
        {
            {
                sendTankCommand(tankRotateLeftTurretButtonAddress);
                resetTankStates();
                rotatingTurretLeft = true;
            }
        }

        private void leftButtonClick(object sender, KeyRoutedEventArgs e)
        {
            if (!rotatingLeft)
            {
                sendTankCommand(tankRotateLeftButtonAddress);
                resetTankStates();
                rotatingLeft = true;
            }
        }

        private void rightButtonClick(object sender, KeyRoutedEventArgs e)
        {
            if (!rotatingRight)
            {
                sendTankCommand(tankRotateRightButtonAddress);
                resetTankStates();
                rotatingRight = true;
            }
        }

        private void gunLiftButtonClick(object sender, KeyRoutedEventArgs e)
        {
            if (!movingMainGun)
            {
                sendTankCommand(tankMainGunMoveButtonAddress);
                resetTankStates();
                movingMainGun = true;
            }
        }

        private void backButtonLift(object sender, KeyRoutedEventArgs e)
        {
            if (!movingBackward)
            {
                sendTankCommand(tankBackwardButtonAddress);
                resetTankStates();
                movingBackward = true;
            }
        }

        private void fireButtonLift(object sender, KeyRoutedEventArgs e)
        {
            if (!firingMainGun)
            {
                sendTankCommand(tankMainGunFireButtonAddress);
                resetTankStates();
                firingMainGun = true;
            }
        }

        private void TurretLeftClickOff(object sender, KeyRoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }

        private void ForwardButtonClickOff(object sender, KeyRoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }

        private void TurretRightClickOff(object sender, KeyRoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }

        private void leftButtonClickOff(object sender, KeyRoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }

        private void fireButtonLiftOff(object sender, KeyRoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }

        private void rightButtonClickOff(object sender, KeyRoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }

        private void backButtonLiftOff(object sender, KeyRoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }

        private void gunLiftButtonClickOff(object sender, KeyRoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }

        private void stopButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            stopButtonClick(sender, e);
        }



        //Single click

        private void turretleftButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            {
                sendTankCommand(tankRotateLeftTurretButtonAddress);
                resetTankStates();
                rotatingTurretLeft = true;
            }
        }

        private void forwardButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!movingForward)
            {
                sendTankCommand(tankForwardButtonAddress);
                resetTankStates();
                movingForward = true;
            }
        }

        private void turretRightButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!rotatingTurretRight)
            {
                sendTankCommand(tankRotateRightTurretButtonAddress);
                resetTankStates();
                rotatingTurretRight = true;
            }
        }

        private void goLeftButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!rotatingLeft)
            {
                sendTankCommand(tankRotateLeftButtonAddress);
                resetTankStates();
                rotatingLeft = true;
            }
        }

        private void goRightButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!rotatingRight)
            {
                sendTankCommand(tankRotateRightButtonAddress);
                resetTankStates();
                rotatingRight = true;
            }
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

        private void backButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!movingBackward)
            {
                sendTankCommand(tankBackwardButtonAddress);
                resetTankStates();
                movingBackward = true;
            }
        }

        private void fireButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!firingMainGun)
            {
                sendTankCommand(tankMainGunFireButtonAddress);
                resetTankStates();
                firingMainGun = true;
            }
        }*/
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

       /* private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Uri targetUri = new System.Uri(TextBlockTargetUri.Text);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
        }*/

        // STEP4 STEP4 STEP4
        private void ReadWebRequestCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);

            using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
            {
                string results = httpwebStreamReader.ReadToEnd();
                //TextBlockResults.Text = results; //-- on another thread!
               
            }
           
        }

        private void sendTankCommand(String tankCommand)
        {

           // WebRequestExtensions request = new WebRequestExtensions();
            System.Uri targetUri = new System.Uri(tankCommand);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
            
            /*if (canSend)
                canSend = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tankCommand);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                var timer = new DispatcherTimer();
                timer.Tick += DispatcherTimerEventHandler;
                timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                timer.Start();
            }
            int i = 0;*/
            /*var httpWebRequestResult = new HttpWebRequestResult(tankCommand);
            yield return httpWebRequestResult;
            var result = httpWebRequestResult.Result;*/

            /*var request = (HttpWebRequest)WebRequest.Create(tankCommand);
            try
            {
                WebResponse serverResponse = request.GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(serverResponse.GetResponseStream(), encoding))
                {
                    
                }
            }
            catch (WebException ex)
            {
                
            }*/
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

        /*static async Task<Stream> AsynchronousDownload(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            return (response.GetResponseStream());
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var myResponse = await AsynchronousDownload();
        }*/
    }
}
