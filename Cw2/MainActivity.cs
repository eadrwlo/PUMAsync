using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace Cw2
{
    [Activity(Label = "Cw2", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Int32 counter = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var CounterText = FindViewById<EditText>(Resource.Id.CounterText);
            var CounterButton = FindViewById<Button>(Resource.Id.CounterButton);
            

            //if (savedInstanceState != null )
            //{
            //    counter = savedInstanceState.GetInt("counter_Count", 0);
            //}
            CounterText.Text = counter.ToString();
            CounterButton.Click += (object sender, EventArgs e) =>
            {
                counter++;
                CounterText.Text = counter.ToString(); 
            };
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("counter_Count", counter);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedState)
        {
            base.OnRestoreInstanceState(savedState);
            counter = savedState.GetInt("counter_Count", 0);
        }


    }
}

