using PrimerApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PrimerApp.ViewModels
{
    public class MainPageViewModel : NotificationObject
    {
        private ObservableCollection<Survey> surveys;

        public ObservableCollection<Survey> Surveys
        {
            get { return surveys; }
            set 
            { 
                surveys = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }

        public ICommand AddRedirection { get; set; }

        public MainPageViewModel()
        {
            Surveys = new ObservableCollection<Survey>();

            AddCommand = new Command(AddCommandExecute);
            AddRedirection = new Command(AddCommandRedirectionExecute);

            MessagingCenter.Subscribe<SurveyDetailsViewModel, Survey>(this, "SaveSurvey", (a, s) =>
            {
                Surveys.Add(s);
            });
        }

        private void AddCommandRedirectionExecute(object obj)
        {

            throw new NotImplementedException();
        }

        private void AddCommandExecute(object obj)
        {
            MessagingCenter.Send(this, "AddSurvey");
        }
    }
}
