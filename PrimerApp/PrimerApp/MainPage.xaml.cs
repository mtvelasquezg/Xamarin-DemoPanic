using PrimerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading;
using Plugin.AudioRecorder;

namespace PrimerApp
{

    public partial class MainPage : ContentPage
    {
        AudioRecorderService recorder;
        AudioPlayer player;

        public MainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<MainPageViewModel>(this, "AddSurvey", async (a) =>
            {
                await Navigation.PushModalAsync(new SurveyDetailsView());
            });


            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = false, //will stop recording after 2 seconds (default) 
                StopRecordingAfterTimeout = false, //stop recording after a max timeout (defined below) 
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds 
            };

            player = new AudioPlayer();
          

        }


        async void RecordButton_Click(object sender, EventArgs e)
        {
            await CheckAndRequestRecordAudioPermission();
            await RecordAudio();
            var filePath = recorder.GetAudioFilePath();
            player.Play(filePath);
        }

        async Task RecordAudio()
        {
            try
            {
                Task<string> recordTask;

                if (!recorder.IsRecording)
                {
                   recordTask = await recorder.StartRecording();
                }
                else
                {
                    await recorder.StopRecording();
                }


                

            }
            catch (Exception ex)
            {
                
            }

        }

        public async Task<PermissionStatus> CheckAndRequestSmsPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Sms>();

            if (status == PermissionStatus.Granted)
                return status;


            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            status = await Permissions.RequestAsync<Permissions.Sms>();

            return status;
        }

        public async Task<PermissionStatus> CheckAndRequestRecordAudioPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();

            if (status == PermissionStatus.Granted)
                return status;


            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            status = await Permissions.RequestAsync<Permissions.Microphone>();

            return status;
        }


        public async Task<Location> GetCurrentLocation()
        {
            CancellationTokenSource cts;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
    
                
                return location;
            }
            catch
            {
                return null;
            }

         }

        public async void Panic(object sender, EventArgs e)
        {
            var location = await GetCurrentLocation();

            await DisplayAlert("Alert", "Se ha enviado tu mensaje: " + "Latitude: " + location.Latitude + " Longitude: " + location.Longitude + " Altitude: " +location.Altitude + "  -  La policia va en camino.", "OK");

        }


        public async void Mensaje(object sender, EventArgs e)
        {
            var resp = await CheckAndRequestSmsPermission();
            var location = await GetCurrentLocation();
            if (resp == PermissionStatus.Granted)
            {

                await Sms.ComposeAsync(new SmsMessage
                {
                    Body = "ayuda! en la ubicacion: " + "Latitude: " + location.Latitude + " Longitude: " + location.Longitude + " Altitude: " + location.Altitude,
                    Recipients = new List<string> { "123" }
                });
            }
            else
            {
                await DisplayAlert("Alert", "You have been alerted", "OK");

            }
        }

        public async void Call2(object sender, EventArgs e)
        {

            PhoneDialer.Open("123");

        }


    }
}

