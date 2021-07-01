using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notebook.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        static int counter = 0;

        private void button1_Clicked(object sender, EventArgs e)
        {
            Button oldButton = sender as Button;
            oldButton.Text = $"Clicked {counter}";
            oldButton.BackgroundColor = Color.Coral;

            counter++;

            Button newButton = new Button() { Text = $"Not clicked {counter}" };
            newButton.Clicked += button1_Clicked;
            newButton.BackgroundColor = Color.Teal;
            Grid.SetRow(newButton, counter / 2);
            Grid.SetColumn(newButton, counter % 2);

            magicGrid.Children.Add(newButton);
        }
    }
}