using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Android;
using Android.Webkit;
using Xamarin.Forms.PlatformConfiguration;


namespace CardFez
{
    public partial class MainPage : ContentPage
    {
        
        protected static RestClient _restClient = new RestClient();
        private string finalHtml;
        private string card;

        public ContactDetails contact { get; set; }
        public MainPage()
        {
            InitializeComponent();

            //// Get Metrics
            
            //var webView = new WebView();
            //var htmlSource = new UrlWebViewSource();
            //htmlSource.Url = DependencyService.Get<IBaseUrl>().Get() + "htmltest.html";

            //webView.Source = htmlSource;

            //Content = webView;
        }
        public struct Constant
        {
            public static double ScreenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            public static double ScreenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        }
        
       public async void CallRestApi(object sender, EventArgs e)

        {

           
            var nameValue = HQID.Text;
           
            var response = await _restClient.GetAsync<Root>("https://webfeztest.shrinenet.org/PublicAPI/v1/Nobles/" +
               nameValue +
                "/ShrineCard");


            var card2 = response.content.shrineCard;
            string headerString = "<header><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=5.0, minimum-scale=1.0, user-scalable=yes'>" +
                 "<style>img:nth-child(1){max-width:94% } div:nth-child(7) img{width:55% !important;}" +
                 "div:nth-child(1){font-size:24pt !important; left: 14px !important;  top: 11px !important}" +
                 "div:nth-child(2){font-size:26pt !important; left: 14px !important;  top: 40px !important}" +
                 "div:nth-child(3){font-size:15pt !important; left: 14px !important;  top: 90px !important}" +
                 "div:nth-child(4){font-size:11pt !important; left: 14px !important;  top: 120px !important}" +
                 "div:nth-child(5){font-size:11pt !important; left: 14px !important;  top: 140px !important}" +
                 "div:nth-child(6){font-size:11pt !important; left: 260px !important; top: 170px !important}" +
                 "div:nth-child(7){ left: 20px !important; top: 360px !important}</style>" +
                 "</header>";
            string headerStringLandscape = "<header><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=5.0, minimum-scale=1.0, user-scalable=yes'>" +
                "<style>" +
                //"img:nth-child(1){max-width:94% } div:nth-child(7) img{width:55% !important;}" +
                //"div:nth-child(1){font-size:24pt !important; left: 14px !important;  top: 11px !important}" +
                //"div:nth-child(2){font-size:26pt !important; left: 14px !important;  top: 40px !important}" +
                //"div:nth-child(3){font-size:15pt !important; left: 14px !important;  top: 90px !important}" +
                //"div:nth-child(4){font-size:11pt !important; left: 14px !important;  top: 120px !important}" +
                //"div:nth-child(5){font-size:11pt !important; left: 14px !important;  top: 140px !important}" +
                //"div:nth-child(6){font-size:11pt !important; left: 260px !important; top: 170px !important}" +
                "div:nth-child(7){ left: 20px !important; top: 33em !important}</style>" +
                "</header>";
            string finalHtml2 = headerString + card2;
            
            finalHtml = finalHtml2;
            card = headerStringLandscape+card2;
            webview3.Source = new HtmlWebViewSource
            {
                Html = finalHtml2
            };
           
            //  await DisplayAlert("Vales from Api", HtmlWebViewSource(html), "OK", "Cancel");
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called

            if (width > height)
            {
                // landscape
                webview3.Source = new HtmlWebViewSource
                {
                    Html = card
                };
            }
            else
            {
                // portrait
                webview3.Source = new HtmlWebViewSource
                {
                    Html = finalHtml
                };
            }
        }
        public class ContactDetails
        {
            public string HQID { get; set; }

        }
        
       
    }
}
