using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;

/**
 * NosteraFan v0.1
 * 
 * Written in C# using Visual Studio Community 2017
 * To run this Program in VS you need to reference Json.NET
 * 
 * This program is intended to show the newest videos of Nosterafus three biggest channels
 * Therefore this program takes advantage of the youtube api and its data which is stored
 * in json Format. The program will also notify you when a new video is uploaded.
 *      In order to implement this feature NosteraFan should be in autostart. Whenever the
 *      json data is downloaded from the YouTube Api it will be saved as json File. Before
 *      it is saved it will be compared to the old savefile though. If differences in the
 *      videoID are found NosteraFan will notify you, preferably with small Information-
 *      Windows in the bottom-right of the Screen.
 * It should also be possible to check the channels you want to be informed about in the
 * Settings Window.
 * 
 * 
 * To Dos:
 *  -Autoscale WindowsForms Components and Font size depending on Windowsize. Generally
 *   improving the way the Windows and its components scale (Especially the Labels). Its
 *   worth to mention that I wrote this Program on a 4K Monitor so improving the User
 *   Interface for usage on fullHD Monitors is an important step.
 *   
 *  -Improving the downloadspeed of the json data. At the moment Webclient is used for
 *   downloading the data but it tends to be quite slow. Setting proxy to null helped but
 *   still is not a perfect solution.
 *   
 *  -implementing the Settings Window which should allow the user to specify which channels
 *   they want to be informed about (Checkboxes). Also a checkbox to put Nosterafan into
 *   autostart
 *   
 *  -Saving json data to file and comparing it to the older file by videoIDs
 *  
 *  -Creating an Information Window which displays a new Video with thumbnail, Title and
 *   description. The user should be broght to youtube onClick
 * 
 *  -Make the progressbar work and display the download-completion in percent
 * **/

namespace nosteraFan
{
    public partial class Form_main : Form
    {
        //Declare some url variables and YTjson Objects which will be used when fetching Data into GUI. The data of each channel is stored in a different object.
        private String website = "http://www.nosterafu.de/";
        private String videoURL = "https://www.youtube.com/watch?v=", amazon = "https://www.amazon.de/gp/search?ie=UTF8&tag=nosterafutv-21&linkCode=ur2&linkId=6eeb5d55340fe7d08d46ff7c4c64fd5f&camp=1638&creative=6742&index=videogames&keywords=Videospiel";
        private String twitch = "https://www.twitch.tv/robin_nosterafu", patreon= "https://www.patreon.com/NosTeraFuTV/memberships", dTube = "https://d.tube/#!/c/nosterafu";
        private String AddressPlaylistTV = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId=UUetlRED-fvf1zp0dXXNeeSw&key=AIzaSyAj2PjjI2Ac9dgEfy4WMo9OcwXrKTVb_SQ";
        private String AddressPlaylistFucked = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId=UUGPfQX-DRhFaLDin2Wfhkyw&key=AIzaSyAj2PjjI2Ac9dgEfy4WMo9OcwXrKTVb_SQ";
        private String AddressPlaylistFurz = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId=UUkC1fdo5ZKwQEroZSzQeM0g&key=AIzaSyAj2PjjI2Ac9dgEfy4WMo9OcwXrKTVb_SQ";
        private YTjson ApiJsonDataTV, ApiJsonDataFucked, ApiJsonDataFurz;
        
        public Form_main()
        {
            InitializeComponent();

            //raising connection limit from default (2) to 3
            ServicePointManager.DefaultConnectionLimit = 3;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        //Click Events
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //A new Thread which executes a fetch-method is created for each Channel
            Thread t0 = new Thread(fetchColumnTV);
            t0.Start();
            Thread t1 = new Thread(fetchColumnFucked);
            t1.Start();
            Thread t2 = new Thread(fetchColumnFurz);
            t2.Start();
        }

        private void pictureBox_Video_1_0_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_1_0.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataTV.items[0].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_Video_1_1_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_1_1.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataTV.items[1].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_Video_1_2_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_1_2.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataTV.items[2].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_Video_2_0_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_2_0.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataFucked.items[0].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_Video_2_1_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_2_1.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataFucked.items[1].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_Video_2_2_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_2_2.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataFucked.items[2].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_Video_3_0_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_3_0.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataFurz.items[0].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_Video_3_1_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_3_1.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataFurz.items[1].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_Video_3_2_Click(object sender, EventArgs e)
        {
            if (pictureBox_Video_3_2.Image != null)
            {
                openUrlInBrowser(videoURL + ApiJsonDataFurz.items[2].snippet.resourceId.videoId);
            }
        }

        private void pictureBox_profile_tv_Click(object sender, EventArgs e)
        {
            openUrlInBrowser("www.youtube.com/user/BoarderleinNosTeraFu/videos");
        }

        private void pictureBox_profile_fucked_Click(object sender, EventArgs e)
        {
            openUrlInBrowser("www.youtube.com/user/NosTeraFucked/videos");
        }

        private void pictureBox_profile_furz_Click(object sender, EventArgs e)
        {
            openUrlInBrowser("www.youtube.com/user/NosTeraFurz/videos");
        }

        private void twitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openUrlInBrowser(twitch);
        }

        private void patreonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openUrlInBrowser(patreon);
        }

        private void dTubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openUrlInBrowser(dTube);
        }

        private void amazonReflinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openUrlInBrowser(amazon);
        }

        private void officialWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openUrlInBrowser(website);
        }


        // Program Methods

        private String downloadJSON(String url)
        {
            //This Method downloads content from the parameter url and returns it as string

            try
            {
                string r;

                using (var webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Encoding = Encoding.UTF8;

                    webClient.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                    Stream stream = webClient.OpenRead(url);
                    GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzipStream);

                    r = reader.ReadToEnd();

                    //====================Progress Bar not working yet==============
                    webClient.DownloadProgressChanged += (s, e) =>
                    {
                        Invoke(new Action(() =>
                        {
                            progressBar1.Value = e.ProgressPercentage;
                        }));
                    };
                    //==============================================================
                }

                return r;
            }
            catch (Exception download)
            {
                MessageBox.Show(download.Message, "Download Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private bool deserialize(String json, int channel)
        {
            /**
             * This Method deserializes String data into YTjson Objects and
             * returns true if successful
             * 
             * Channel 0->TV       1->Fucked       2->Furz
             * **/
            try
            {
                switch (channel)
                {
                    case 0:
                        ApiJsonDataTV = JsonConvert.DeserializeObject<YTjson>(json);
                        return true;
                    case 1:
                        ApiJsonDataFucked = JsonConvert.DeserializeObject<YTjson>(json);
                        return true;
                    case 2:
                        ApiJsonDataFurz = JsonConvert.DeserializeObject<YTjson>(json);
                        return true;
                }
                return false;
            }
            catch (Exception deserialize)
            {
                MessageBox.Show(deserialize.Message,"JSON Deserialize Exception",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }

        private void openUrlInBrowser(String url)
        {
            //This Method opens the url parameter in the default webbrowser
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception openUrl)
            {
                MessageBox.Show(openUrl.Message,"OpenUrl Exception",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void fetchColumnTV() {
            //Data from YTjson is assigned to NosterafuTV Column
            if (deserialize(downloadJSON(AddressPlaylistTV),0))
            {
                Invoke(new Action(() =>
                {
                    //Fetch in Title and Description -TV
                    label_Video_Title_1_0.Text = ApiJsonDataTV.items[0].snippet.title;
                    label_Video_Desc_1_0.Text = ApiJsonDataTV.items[0].snippet.description;

                    label_Video_Title_1_1.Text = ApiJsonDataTV.items[1].snippet.title;
                    label_Video_Desc_1_1.Text = ApiJsonDataTV.items[1].snippet.description;

                    label_Video_Title_1_2.Text = ApiJsonDataTV.items[2].snippet.title;
                    label_Video_Desc_1_2.Text = ApiJsonDataTV.items[2].snippet.description;


                    //Fetch in Thumbnails -TV
                    pictureBox_Video_1_0.Load(ApiJsonDataTV.items[0].snippet.thumbnails.medium.url);
                    pictureBox_Video_1_0.SizeMode = PictureBoxSizeMode.Zoom;

                    pictureBox_Video_1_1.Load(ApiJsonDataTV.items[1].snippet.thumbnails.medium.url);
                    pictureBox_Video_1_1.SizeMode = PictureBoxSizeMode.Zoom;

                    pictureBox_Video_1_2.Load(ApiJsonDataTV.items[2].snippet.thumbnails.medium.url);
                    pictureBox_Video_1_2.SizeMode = PictureBoxSizeMode.Zoom;
                }));
            }
                
        }

        private void fetchColumnFucked()
        {
            //Data from YTjson is assigned to NosTeraFucked Column
            if (deserialize(downloadJSON(AddressPlaylistFucked), 1))
            {
                Invoke(new Action(() =>
                {
                    //Fetch in Title and Description -TV
                    label_Video_Title_2_0.Text = ApiJsonDataFucked.items[0].snippet.title;
                    label_Video_Desc_2_0.Text = ApiJsonDataFucked.items[0].snippet.description;

                    label_Video_Title_2_1.Text = ApiJsonDataFucked.items[1].snippet.title;
                    label_Video_Desc_2_1.Text = ApiJsonDataFucked.items[1].snippet.description;

                    label_Video_Title_2_2.Text = ApiJsonDataFucked.items[2].snippet.title;
                    label_Video_Desc_2_2.Text = ApiJsonDataFucked.items[2].snippet.description;


                    //Fetch in Thumbnails -TV
                    pictureBox_Video_2_0.Load(ApiJsonDataFucked.items[0].snippet.thumbnails.medium.url);
                    pictureBox_Video_2_0.SizeMode = PictureBoxSizeMode.Zoom;

                    pictureBox_Video_2_1.Load(ApiJsonDataFucked.items[1].snippet.thumbnails.medium.url);
                    pictureBox_Video_2_1.SizeMode = PictureBoxSizeMode.Zoom;

                    pictureBox_Video_2_2.Load(ApiJsonDataFucked.items[2].snippet.thumbnails.medium.url);
                    pictureBox_Video_2_2.SizeMode = PictureBoxSizeMode.Zoom;
                }));
            }

        }

        private void fetchColumnFurz()
        {
            //Data from YTjson is assigned to NosTeraFurz Column
            if (deserialize(downloadJSON(AddressPlaylistFurz), 2))
            {
                Invoke(new Action(() =>
                {
                    //Fetch in Title and Description -TV
                    label_Video_Title_3_0.Text = ApiJsonDataFurz.items[0].snippet.title;
                    label_Video_Desc_3_0.Text = ApiJsonDataFurz.items[0].snippet.description;

                    label_Video_Title_3_1.Text = ApiJsonDataFurz.items[1].snippet.title;
                    label_Video_Desc_3_1.Text = ApiJsonDataFurz.items[1].snippet.description;

                    label_Video_Title_3_2.Text = ApiJsonDataFurz.items[2].snippet.title;
                    label_Video_Desc_3_2.Text = ApiJsonDataFurz.items[2].snippet.description;


                    //Fetch in Thumbnails -TV
                    pictureBox_Video_3_0.Load(ApiJsonDataFurz.items[0].snippet.thumbnails.medium.url);
                    pictureBox_Video_3_0.SizeMode = PictureBoxSizeMode.Zoom;

                    pictureBox_Video_3_1.Load(ApiJsonDataFurz.items[1].snippet.thumbnails.medium.url);
                    pictureBox_Video_3_1.SizeMode = PictureBoxSizeMode.Zoom;

                    pictureBox_Video_3_2.Load(ApiJsonDataFurz.items[2].snippet.thumbnails.medium.url);
                    pictureBox_Video_3_2.SizeMode = PictureBoxSizeMode.Zoom;
                }));
            }

        }

        
    }
}
