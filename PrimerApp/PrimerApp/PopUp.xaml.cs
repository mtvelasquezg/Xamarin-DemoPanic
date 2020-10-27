using System;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;


namespace AdvancedPopUpSample
{
    public partial class PopUp //: PopupPage
    {
        AudioRecorderService recorder;
        AudioPlayer player;
        public PopUp()
        {
            InitializeComponent();
            ocultarBotones();
            ocultarReproducir();
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = false, //will stop recording after 2 seconds (default) 
                StopRecordingAfterTimeout = false, //stop recording after a max timeout (defined below) 
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds 
                
        };

            player = new AudioPlayer();
        }

        //Eventos

        async void RecordButton_Click(object sender, EventArgs e)
        {
            await CheckAndRequestRecordAudioPermission();
            await RecordAudio();
        }

        async Task RecordAudio()
        {
            try
            {
                Task<string> recordTask;

                if (!recorder.IsRecording)
                {
                    recordTask = await recorder.StartRecording();
                    //cambia imagen del boton grabar
                    boton_grabar.Source = "stop.png";
                    label_grabando_audio.SetValue(IsVisibleProperty, true);
                    ocultarReproducir();
                    mostrarGrabar();

                }
                else
                {
                    await recorder.StopRecording();
                    label_grabando_audio.SetValue(IsVisibleProperty, false);
                    mostrarBotones();
                    mostrarReproducir();
                    ocultarGrabar();
                }

            }
            catch (Exception ex)
            {

            }

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

        public void PlayButton_Click(object sender, EventArgs e)
        {
            var filePath = recorder.GetAudioFilePath();
            player.Play(filePath);
        }

        private void boton_enviar_click(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
            DisplayAlert("Su grabación ha sido enviada", "Una patrulla llegará pronto", "OK");
        }

        private void boton_cancelar_click(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

   
        private void boton_nuevo_click(object sender, EventArgs e)
        {
            ocultarReproducir();
            ocultarBotones();
            mostrarGrabar();
            //cambia imagen del boton grabar
            boton_grabar.Source = "microfono.png";

        }

        //Metodos para ocultar o mostrar elementos del pop-up
        private void mostrarGrabar()
        {

            mostrarTituloGrabar.SetValue(IsVisibleProperty, true);
            espacioTituloGrabar.SetValue(IsVisibleProperty, true);
            boton_grabar.SetValue(IsVisibleProperty, true);
        }

        private void ocultarGrabar()
        {
            mostrarTituloGrabar.SetValue(IsVisibleProperty, false);
            espacioTituloGrabar.SetValue(IsVisibleProperty, false);
            boton_grabar.SetValue(IsVisibleProperty, false);
        }

        private void mostrarReproducir()
        {
            boton_grabar.Source = "play.png";
            boton_play.SetValue(IsVisibleProperty, true);
            mostrarReproducirLabel.SetValue(IsVisibleProperty, true);
            espacioTituloReproducir.SetValue(IsVisibleProperty, true);
        }

        private void ocultarReproducir()
        {
            boton_play.SetValue(IsVisibleProperty, false);
            mostrarReproducirLabel.SetValue(IsVisibleProperty, false);
            espacioTituloReproducir.SetValue(IsVisibleProperty, false);
        }

        private void ocultarBotones()
        {
            boton_enviar.SetValue(IsVisibleProperty, false);
            boton_nuevo.SetValue(IsVisibleProperty, false);
            boton_cancelar.SetValue(IsVisibleProperty, false);
        }

        private void mostrarBotones()
        {
            boton_enviar.SetValue(IsVisibleProperty, true);
            boton_nuevo.SetValue(IsVisibleProperty, true);
            boton_cancelar.SetValue(IsVisibleProperty, true);
        }

    }
}
