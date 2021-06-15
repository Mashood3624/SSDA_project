using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using DarrenLee.Media;
using MySql.Data.MySqlClient;
using System.IO;
using System.Configuration;
//using System.DateTime;

namespace Camera
{
    public partial class Form1 : Form
    {
        String base_name="";
        DateTime start_time;
        String start_string;
        public Form1(string x)
        {


            InitializeComponent();
            //constr = ConfigurationManager.ConnectionStrings["CameraString"].ConnectionString;
            base_name = x;
            label6.Text = DateTime.Now.ToString("dd/MM/yyyy");
            // something before delay
            

            
            
            String constr = ConfigurationManager.ConnectionStrings["CameraString"].ConnectionString;
            connection = new MySqlConnection(constr);
            
        }
        // date.Text = DateTime.now("MM/dd/yyyy");


        
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        MySqlConnection connection;
        MySqlCommand command;
        Size resize = new Size(640, 480);//image size
        bool video = false;
        private void Start_Click(object sender, EventArgs e)
        {
            if (combocourse.Text == "")
            {
                MessageBox.Show("Add course value");
            }
            else
            {
                video = true;
                combocourse.Enabled = false;
                combocamera.Enabled = false;
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[combocamera.SelectedIndex].MonikerString);
                videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                videoCaptureDevice.Start();

                //Timer for uploading images
                t1 = new Timer();
                t1.Interval = 30000;  //uploading data after 10sec
                t1.Tick += T1_Tick;
                t1.Start();

                // Timer for count starts below
                CountDownTimer timer = new CountDownTimer();
                timer.SetTime(60, 0);        //set to 30 mins
                timer.Start();
                start_string = DateTime.Now.ToString("HH:mm:ss");
                start_time = Convert.ToDateTime(start_string);

                timer.TimeChanged += () => label2.Text = timer.TimeLeftMsStr;   //update label text
                timer.CountDownFinished += () => System.Windows.Forms.Application.Exit(); //when timer is finished
                                                                                          //timer step. By default is 1 second
                timer.StepMs = 77; // for nice milliseconds time switch
            }
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap resizeImage = new Bitmap((Bitmap)eventArgs.Frame.Clone(), resize);
            pic.Image = resizeImage;
            pic.SizeMode = PictureBoxSizeMode.Normal;
            //resizeImage.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
                combocamera.Items.Add(filterInfo.Name);

            combocamera.SelectedIndex = 0;
            try
            {

                connection.Open();
                String select_query = "SELECT DISTINCT Course FROM SSDA.images WHERE Name='" + base_name + "'";
                command = new MySqlCommand(select_query, connection);

                MySqlDataReader dataReader = command.ExecuteReader();
                combocourse.Items.Clear();
                while (dataReader.Read())
                {
                    combocourse.Items.Add(dataReader["Course"]);
                }
                dataReader.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //videoCaptureDevice = new VideoCaptureDevice();
        }

        int unuploaded_cnt = 0;
        int c = 1;
        String[] time_stamp = new String[50];
        string strCmdText;

        private void T1_Tick(object sender, EventArgs e)
        {
            String x1 = DateTime.Now.ToString("HH:mm:ss");
            DateTime current_ = Convert.ToDateTime(x1);
            //b.Subtract(a).TotalMinutes
            double ctime = current_.Subtract(start_time).TotalSeconds;
            //inserting image into database
            //MessageBox.Show((Convert.ToDateTime(start_time).Subtract(Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss"))).TotalSeconds)+"");
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                
                while (unuploaded_cnt >= c)
                {
                    var bitmap = new Bitmap(@"C:\\Users\\masho\\OneDrive\\Desktop\\NUST\\2nd Semester\\SE-861 Software System Design & Architecture\\Assignments\\Paper\\C#\\Camera\\Camera\\not_uploaded\\" + c + ".jpeg");
                    var stream_temp = new MemoryStream();
                    bitmap.Save(stream_temp, System.Drawing.Imaging.ImageFormat.Jpeg);

                    byte[] img_temp = stream_temp.ToArray();

                    string insertQuery_temp = "INSERT INTO SSDA.images(Image,Name,Course,Date,Time,Start_Time) VALUES(@img,@name,'" + combocourse.Text + "','" + label6.Text + "','" + time_stamp[c] + "','" + start_string + "')";
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    command = new MySqlCommand(insertQuery_temp, connection);

                    command.Parameters.Add("@img", MySqlDbType.Blob);
                    command.Parameters.Add("@name", MySqlDbType.VarChar);

                    command.Parameters["@img"].Value = img_temp;
                    command.Parameters["@name"].Value = base_name;

                    command.ExecuteNonQuery();
                    
                    
                    stream_temp.Dispose();
                    connection.Close();
                    bitmap.Dispose();
                    strCmdText = "/C python C:\\Users\\masho\\OneDrive\\Desktop\\process.py " + base_name + " " + combocourse.Text + " " + label6.Text + " " + start_time + " " + time_stamp[c];
                    System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                    File.Delete(@"C:\\Users\\masho\\OneDrive\\Desktop\\NUST\\2nd Semester\\SE-861 Software System Design & Architecture\\Assignments\\Paper\\C#\\Camera\\Camera\\not_uploaded\\" + c + ".jpeg");
                    c = c + 1;
                    
                }
                unuploaded_cnt = 0;
                c = 1;
                var stream = new MemoryStream();
                pic.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] img = stream.ToArray();

                String insertQuery = "INSERT INTO SSDA.images(Image,Name,Course,Date,Time,Start_Time) VALUES(@img,@name,'" + combocourse.Text + "','" + label6.Text + "','" + ctime + "','" + start_string + "')";
                if (connection.State == ConnectionState.Closed) {
                    connection.Open();
                }
                

                command = new MySqlCommand(insertQuery, connection);

                command.Parameters.Add("@img", MySqlDbType.Blob);
                command.Parameters.Add("@name", MySqlDbType.VarChar);

                command.Parameters["@img"].Value = img;
                command.Parameters["@name"].Value = base_name;

                command.ExecuteNonQuery();
                
                stream.Dispose();
                connection.Close();
                strCmdText = "/C python C:\\Users\\masho\\OneDrive\\Desktop\\process.py " + base_name + " " + combocourse.Text + " " + label6.Text + " " + start_string + " " + ctime;
                //strCmdText = "/C python C:\\Users\\masho\\OneDrive\\Desktop\\process.py ";// + "mashood" + " " + "SSDA" + " " + "07/06/2021" + " " + "19:35:15" + " " + "30";
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                
                




            }
            catch (Exception ex)
            {
                unuploaded_cnt = unuploaded_cnt +1 ;
                pic.Image.Save("C:\\Users\\masho\\OneDrive\\Desktop\\NUST\\2nd Semester\\SE-861 Software System Design & Architecture\\Assignments\\Paper\\C#\\Camera\\Camera\\not_uploaded\\" + unuploaded_cnt + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                time_stamp[unuploaded_cnt] = ctime + "";
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (video == true)
            {
                if (videoCaptureDevice.IsRunning == true)
                {
                    videoCaptureDevice.Stop();
                    //string strCmdText;
                    // Name course date
                    // base_name textbox1.Text label6.Text
                    //strCmdText = "/C python C:\\Users\\masho\\OneDrive\\Desktop\\process.py "+base_name+" "+combocourse.Text+" "+ label6.Text;
                    //System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                }
            }
            System.Windows.Forms.Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void combocamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}


public class CountDownTimer : IDisposable
{
    public Stopwatch _stpWatch = new Stopwatch();

    public Action TimeChanged;
    public Action CountDownFinished;

    public bool IsRunnign => timer.Enabled;

    public int StepMs
    {
        get => timer.Interval;
        set => timer.Interval = value;
    }

    private Timer timer = new Timer();

    private TimeSpan _max = TimeSpan.FromMilliseconds(30000);

    public TimeSpan TimeLeft => (_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) > 0 ? TimeSpan.FromMilliseconds(_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) : TimeSpan.FromMilliseconds(0);

    private bool _mustStop => (_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) < 0;

    public string TimeLeftStr => TimeLeft.ToString(@"\mm\:ss");

    public string TimeLeftMsStr => TimeLeft.ToString(@"mm\:ss\.fff");

    private void TimerTick(object sender, EventArgs e)
    {
        TimeChanged?.Invoke();

        if (_mustStop)
        {
            CountDownFinished?.Invoke();
            _stpWatch.Stop();
            timer.Enabled = false;
        }
    }

    public CountDownTimer(int min, int sec)
    {
        SetTime(min, sec);
        Init();
    }

    public CountDownTimer(TimeSpan ts)
    {
        SetTime(ts);
        Init();
    }

    public CountDownTimer()
    {
        Init();
    }

    private void Init()
    {
        StepMs = 1000;
        timer.Tick += new EventHandler(TimerTick);
    }

    public void SetTime(TimeSpan ts)
    {
        _max = ts;
        TimeChanged?.Invoke();
    }

    public void SetTime(int min, int sec = 0) => SetTime(TimeSpan.FromSeconds(min * 60 + sec));

    public void Start()
    {
        timer.Start();
        _stpWatch.Start();
    }

    public void Pause()
    {
        timer.Stop();
        _stpWatch.Stop();
    }

    public void Stop()
    {
        Reset();
        Pause();
    }

    public void Reset()
    {
        _stpWatch.Reset();
    }

    public void Restart()
    {
        _stpWatch.Reset();
        timer.Start();
    }

    public void Dispose() => timer.Dispose();
}