# NosteraFan
**NosteraFan v0.1** <br />
**A program which keeps the user up to date with the newest videos from [NosterafuTV](https://www.youtube.com/user/BoarderleinNosTeraFu) YouTube.**


 
Written in C# using Visual Studio Community 2017.
To run this Program in VS you need to reference Json.NET
 
 
 
This program is intended to show the newest videos of Nosterafus three biggest Youtube channels. Therefore this program takes advantage of the youtube api and its data which is stored in json Format. In the future the program will also notify you when a new video is uploaded. In order to implement this feature NosteraFan should be in autostart. It should also be possible to check the channels you want to be informed about in the
Settings Window.

<br />
<br />

**Achievements:**
  * *Download json data from youtube using WebClient*
  
  * *Deserializing json data using json.NET to make working with the data easy*
  
  * *using a seperate thread when refreshing in order to keep the GUI working*
  
  * *Creating maybe not a pretty but functional user interface*
  
  <br />
  <br />
  
**To Dos:**
 * *Autoscale WindowsForms Components and Font size depending on Windowsize. Generally
  improving the way the Windows and its components scale (Especially the Labels). Its
  worth to mention that I wrote this Program on a 4K Monitor so improving the User
  Interface for usage on fullHD Monitors is an important step.*
  
 * *Improving the downloadspeed of the json data. At the moment Webclient is used for
  downloading the data but it tends to be quite slow. Setting proxy to null helped but
  still is not a perfect solution.*
  
 * *implementing the Settings Window which should allow the user to specify which channels
  they want to be informed about (Checkboxes). Also a checkbox to put Nosterafan into
  autostart*
  
 * *Saving json data to file and comparing it to the older file by videoIDs*
 
 * *Creating an Information Window which displays a new Video with thumbnail, Title and
  description. The user should be broght to youtube onClick*

 * *Make the progressbar work and display the download-completion in percent*

<br />
<br />

**Screenshot:**
![Screenshot:](/screenshot.JPG)
