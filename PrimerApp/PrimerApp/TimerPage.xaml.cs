using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PrimerApp.ViewModels;
using PrimerApp.Models;

namespace PrimerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerPage : ContentPage
    {
        TimerViewModel tvm = new TimerViewModel();


        public TimerPage()
        {

            InitializeComponent();
            tvm.StartTimerCommand();


        }

        public async void Enviar(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Se ha enviado tu mensaje:", "OK");
        }

        
    }
}