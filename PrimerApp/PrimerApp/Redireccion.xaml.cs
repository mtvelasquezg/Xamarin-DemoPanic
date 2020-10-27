using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrimerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Redireccion : ContentPage
    {
        String mensaje;
        public Redireccion()
        {
            InitializeComponent();


        }
        public async void Enviar(object sender, EventArgs e)
        {
            mensaje = msg.Text;
            await DisplayAlert("Alert", "Se ha enviado tu mensaje:" + mensaje , "OK");
            await Navigation.PushModalAsync(new TimerPage());

        }

    }

    
}