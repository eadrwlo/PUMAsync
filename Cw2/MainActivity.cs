using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;
using System.Net;
using System.Threading;

namespace Cw2
{
    [Activity(Label = "Cw2", MainLauncher = true)]
    public class MainActivity : Activity
    {
        WebClient webClient = new WebClient();
        Int32 counter = 0;
        EditText urlTextBox;
        ImageView imageView;
        ProgressBar progressBar;
        byte[] bytes;
        Task downloadTask;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var CounterText = FindViewById<EditText>(Resource.Id.CounterText);
            var CounterButton = FindViewById<Button>(Resource.Id.CounterButton);
            var downloadButton = FindViewById<Button>(Resource.Id.downloadButton);
            var cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            downloadButton.Click += DownloadButtonAsync_Click;
            cancelButton.Click += CancelButonClick;
            urlTextBox = FindViewById<EditText>(Resource.Id.urlTextBox);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
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

        private void CancelButonClick(object sender, EventArgs e)
        {
            try
            {
                webClient.CancelAsync();
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Status == WebExceptionStatus.RequestCanceled)
                {

                }

            }
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
        private async void DownloadButtonAsync_Click(object sender, System.EventArgs e)
        {
           // CancellationTokenSource token;
           //var url = urlTextBox.Text;
           // await DownloadImageAsync(url);
        }

        private async Task DownloadImageAsync(string url)
        {
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;   
            bytes = await webClient.DownloadDataTaskAsync(url);
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string localFilename = "downloaded.png";
            string localPath = System.IO.Path.Combine(documentsPath, localFilename);
            FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
            await fs.WriteAsync(bytes, 0, bytes.Length);

            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            await BitmapFactory.DecodeFileAsync(localPath, options);
            options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight / imageView.Height : options.OutWidth / imageView.Width;
            options.InJustDecodeBounds = false;
            Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath, options);
            imageView.SetImageBitmap(bitmap);
            fs.Close();
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Progress = (int)(e.BytesReceived);
            progressBar.Max = (int)e.TotalBytesToReceive;
        }
    }
}

