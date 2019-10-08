using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using nuitrack;
using nuitrack.issues;

using Exception = System.Exception;
using System.Numerics;
using Vector3 = nuitrack.Vector3;
using System.Timers;
using Accord.Video.FFMPEG;
using System.Drawing.Drawing2D;
using AForge.Video;
using System.Threading;
//using transform to rotate vectors
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private DirectBitmap _bitmap;
        private bool _visualizeColorImage = false;
        private bool _colorStreamEnabled = false;
        private bool save_state = true;
        private DepthSensor _depthSensor;
        private ColorSensor _colorSensor;
        private UserTracker _userTracker;
        private SkeletonTracker _skeletonTracker;
        private GestureRecognizer _gestureRecognizer;
        private HandTracker _handTracker;

        private int start_num=1;
        private int end_num=99;
        private int frame_count=0;
        private List<Bitmap> record_bitmaps = new List<Bitmap> { };
        private Bitmap final_plot;
        private Thread record_video_thread;
        private double motion_distance_saving = 0;

        private DepthFrame _depthFrame;
        private SkeletonData _skeletonData;
        private HandTrackerData _handTrackerData;
        private IssuesData _issuesData = null;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg,
                                             bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;
        private const int WM_PAINT = 0xf;
        private const int WM_CREATE = 0x1;

        int side_item =0;
        string checked_direct = "";
        List <JointType> measured_joints = new List<JointType> {};
        //calib line variable

        private bool calib_start_line = false;
        private bool calib_end_line = false;
        private Joint start_shoulder;
        private Joint start_hand;
        private PointF calib_hand_start;
        private PointF calib_shoulder;
        private PointF calib_hand_end;

        private double arm_line_length;
        private double real_arm_length;
        private double real_gap_angle;
        private double real_area;
        private double round_count;




        List<PointF> split_point_list = new List<PointF> { };
        List<Joint> split_point_list_3d = new List<Joint> { };
        int left_count=0;
        //List<PointF> right_split_point_list = new List<PointF> { };
        List<float> acceleration = new List<float> { };
        List<float> time_intervals = new List<float> { };
        
        List<DateTime> split_second = new List<DateTime> { };
        
        
        public float split_angle_pie=10;
        private Bitmap bmp;
        private Boolean save_bmp = false;
        private List<Bone> bones = new List<Bone>()
        {
            new Bone(JointType.Waist, JointType.Torso, new Vector3(0, 1, 0)),
            new Bone(JointType.Neck, JointType.Head, new Vector3(0, 1, 0)),

            new Bone(JointType.LeftCollar, JointType.LeftShoulder, new Vector3(1, 0, 0)),
            new Bone(JointType.LeftShoulder, JointType.LeftElbow, new Vector3(1, 0, 0)),
            new Bone(JointType.LeftElbow, JointType.LeftWrist, new Vector3(1, 0, 0)),
            new Bone(JointType.LeftWrist, JointType.LeftHand, new Vector3(1, 0, 0)),

            new Bone(JointType.Waist, JointType.LeftHip, new Vector3(1, 0, 0)),
            new Bone(JointType.LeftHip, JointType.LeftKnee, new Vector3(0, -1, 0)),
            new Bone(JointType.LeftKnee, JointType.LeftAnkle, new Vector3(0, -1, 0)),

            new Bone(JointType.RightCollar,JointType.RightShoulder, new Vector3(-1, 0, 0)),
            new Bone(JointType.RightShoulder,JointType. RightElbow, new Vector3(-1, 0, 0)),
            new Bone(JointType.RightElbow,JointType. RightWrist, new Vector3(-1, 0, 0)),
            new Bone(JointType.RightWrist,JointType. RightHand, new Vector3(-1, 0, 0)),

            new Bone(JointType.Waist,JointType. RightHip, new Vector3(-1, 0, 0)),
            new Bone(JointType.RightHip,JointType. RightKnee, new Vector3(0, -1, 0)),
            new Bone(JointType.RightKnee,JointType. RightAnkle, new Vector3(0, -1, 0)),
        };

        void Capture() {

            
            //record_bitmaps.Add(bp);
            //Thread.Sleep(100);
        }
        //public static Bitmap MatToBitmap(Mat image)
        //{
            
        //    return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
       // }
        //public static Mat BitmapToMat(Bitmap image)
        //{
        //    return OpenCvSharp.Extensions.BitmapConverter.ToMat(image);
        //}
        public static List<double> Shoulder_angle (Joint torso, Joint collar, Joint shoulder, Joint hand,bool obtain_shoulder_length) {

            List<double> output = new List<double> { };
            Vector3 v1 = new Vector3(hand.Real.X - shoulder.Real.X,
                hand.Real.Y - shoulder.Real.Y, hand.Real.Z - shoulder.Real.Z);
            Vector3 v2 = new Vector3(torso.Real.X-collar.Real.X , torso.Real.Y- collar.Real.Y ,
               torso.Real.Z- collar.Real.Z);
            Vector3 v3=new Vector3(0,0,0);
            if (obtain_shoulder_length == true) {
                v3 = new Vector3(shoulder.Real.X - collar.Real.X, shoulder.Real.Y - collar.Real.Y,
               shoulder.Real.Z - collar.Real.Z);
            }

            double v1mag = Math.Sqrt(Math.Pow(v1.X,2) + Math.Pow(v1.Y, 2) + Math.Pow(v1.Z, 2));
         
            double v2mag = Math.Sqrt(Math.Pow(v2.X, 2) + Math.Pow(v2.Y, 2) + Math.Pow(v2.Z, 2));
            double v3mag = 0;
            if (v3.X != 0 && v3.Y != 0 && v3.Z != 0) {
                v3mag = Math.Sqrt(Math.Pow(v3.X, 2) + Math.Pow(v3.Y, 2) + Math.Pow(v3.Z, 2));
            }

            double v1_v2 = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
            double res = v1_v2/(v1mag*v2mag);
            double angle = 0;
            angle = Math.Acos(res) * 180 / Math.PI;
            
            output.Add(v1mag);
            if (v3mag != 0)
            {
                output.Add(v3mag);
            }
            output.Add(angle);
            return output;

        }
        public static Boolean collinear(PointF p1, PointF p2 , PointF p3)
        {
            // Calculation the area of  
            // triangle. We have skipped  
            // multiplication with 0.5  
            // to avoid floating point  
            // computations  
            Vector2 v1 = Vector2.Normalize( new Vector2(p2.X - p1.X, p2.Y - p1.Y));
            Vector2 v2 = Vector2.Normalize(new Vector2(p3.X - p1.X, p3.Y - p1.Y));
            //double a = (p1.Y - p2.Y) * (p1.X - p3.X) - (p1.Y - p3.Y) * (p1.X - p2.X);
            double distance = Math.Abs(Vector2.Distance(v1, v2));
            Console.WriteLine("distance: {0}", distance);
            if (distance<0.085)
                return true;
            else
                return false;
        }
        public static double vect2d_angle(PointF center_p, PointF start_p, PointF end_p)
        {

            
            Vector2 v1 = new Vector2(start_p.X - center_p.X,start_p.Y-center_p.Y);
            Vector2 v2 = new Vector2(end_p.X - center_p.X, end_p.Y - center_p.Y);

            double v1mag = Math.Sqrt(Math.Pow(v1.X, 2) + Math.Pow(v1.Y, 2));

            double v2mag = Math.Sqrt(Math.Pow(v2.X, 2) + Math.Pow(v2.Y, 2));

            double v1_v2 = v1.X * v2.X + v1.Y* v2.Y ;
            double res = v1_v2 / (v1mag * v2mag);
            double angle = 0;
            
            angle = Math.Acos(res) * 180 / Math.PI;
            
     
            return angle;

        }
        public static Vector2 RotateBy(Vector2 v, float a, bool bUseRadians = false)
        {
            if (!bUseRadians) 
                a *= (float) Math.PI/180;
            var ca = Math.Cos(a);
            var sa = Math.Sin(a);
            var rx = v.X * ca - v.Y * sa;

            return new Vector2((float)rx, (float)(v.X * sa + v.Y * ca));
        }

        public static PointF[] split_point_sequence_3d(float max_angle,
            PointF start_point, PointF end_point)
        {

            PointF[] points = { };
            return points;
        }
        private static PointF CalculatePoint(PointF a, PointF b, double distance)
        {

            // a. calculate the vector from o to g:
            double vectorX = b.X - a.X;
            double vectorY = b.Y - a.Y;

            // b. calculate the proportion of hypotenuse
            double factor = distance / Math.Sqrt(vectorX * vectorX + vectorY * vectorY);

            // c. factor the lengths
            vectorX *= factor;
            vectorY *= factor;

            // d. calculate and Draw the new vector,
            return new PointF((int)(a.X + vectorX), (int)(a.Y + vectorY));
            
        }
        private static List<PointF> findSplit_Points (PointF hand_point,PointF shoulder_point,float split_angle,bool checked_left){
            Vector2 start_vector = new
                            Vector2(hand_point.X - shoulder_point.X, hand_point.Y - shoulder_point.Y);
            List<PointF> points = new List<PointF> { };
            double rounds = Math.Floor(180 / split_angle);
            for (int i = 0; i < ((int)rounds); i++)
            {
                Vector2 new_vector = new Vector2();
                if (checked_left == true)
                {
                    new_vector = RotateBy(start_vector, -split_angle, false);
                }
                else {
                    new_vector = RotateBy(start_vector, split_angle, false);
                }
                points.Add(
                    new PointF(new_vector.X + shoulder_point.X, new_vector.Y + shoulder_point.Y));
                start_vector = new_vector;
            }
            return points;
        }
        private static double compute_seconds(DateTime start,DateTime end) {
            var diffseconds = (end - start).TotalSeconds;
            return diffseconds;
        }

        public static void SuspendDrawing(Form parent)
        {
            SendMessage(parent.Handle, WM_PAINT, false, 0);
        }

        public static void ResumeDrawing(Form parent)
        {
            SendMessage(parent.Handle, WM_PAINT, true, 0);
            //   parent.Refresh();
        }

        private String saveFile(Bitmap bmp) {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Png Image|*.png";
            saveFileDialog1.Title = "Save an Image File";
            
            saveFileDialog1.ShowDialog(this);
            String filename="";
            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "" )
            {
                // Saves the Image via a FileStream created by the OpenFile method.  
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the  
                // File type selected in the dialog box.  
                // NOTE that the FilterIndex property is one-based.  
                switch (saveFileDialog1.FilterIndex)
                { 
                    case 1:
                        
                        bmp.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Jpeg);
                        filename = saveFileDialog1.FileName;
                        break;

                    case 2:
                        bmp.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Bmp);
                        filename = saveFileDialog1.FileName;
                        break;

                    case 3:
                        bmp.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Png);
                        filename = saveFileDialog1.FileName;
                        break;
                    
                }

                fs.Close();
                return filename;
            }
            return filename;
        }
        public Form1() {
            
            InitializeComponent();

            this.panel_setting.Visible = true;
            this.panel_process.Visible = false;
            try
            {
                string config_path = "C://Program Files//Nuitrack//nuitrack//nuitrack//data//nuitrack.config";
                string license_path = "C://Program Files//Nuitrack//nuitrack//nuitrack//data//license.json";
                Nuitrack.Init(config_path);

                
                //Nuitrack.SetConfigValue("DepthProvider.Depth2ColorRegistration", "true");
                Nuitrack.SetConfigValue("Skeletonization.MaxDistance", "6000");
                Nuitrack.SetConfigValue("Skeletonization.ActiveUsers", "1");
                
                Nuitrack.SetConfigValue("LicenseFile", license_path);
            }
            catch (Exception exception)
            {
                //Console.WriteLine("Cannot initialize Nuitrack.");
                throw exception;
            }

            try
            {
                // Create and setup all required modules
                _depthSensor = DepthSensor.Create();
                _colorSensor = ColorSensor.Create();
                _userTracker = UserTracker.Create();
                _skeletonTracker = SkeletonTracker.Create();
                _handTracker = HandTracker.Create();
                _gestureRecognizer = GestureRecognizer.Create();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Can't find available camera!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                throw exception;
            }
            
            _depthSensor.SetMirror(false);

            // Add event handlers for all modules
            _depthSensor.OnUpdateEvent += onDepthSensorUpdate;
            _colorSensor.OnUpdateEvent += onColorSensorUpdate;
            _userTracker.OnUpdateEvent += onUserTrackerUpdate;
            _skeletonTracker.OnSkeletonUpdateEvent += onSkeletonUpdate;
            _handTracker.OnUpdateEvent += onHandTrackerUpdate;
            _gestureRecognizer.OnNewGesturesEvent += onNewGestures;

            // Add an event handler for the IssueUpdate event 
            Nuitrack.onIssueUpdateEvent += onIssueDataUpdate;

            // Create and configure the Bitmap object according to the depth sensor output mode
            OutputMode mode = _depthSensor.GetOutputMode();
            OutputMode colorMode = _colorSensor.GetOutputMode();

            if (mode.XRes < colorMode.XRes)
                mode.XRes = colorMode.XRes;
            if (mode.YRes < colorMode.YRes)
                mode.YRes = colorMode.YRes;

            _bitmap = new DirectBitmap(mode.XRes, mode.YRes);
            for (int y = 0; y < mode.YRes; ++y)
            {
                for (int x = 0; x < mode.XRes; ++x)
                    _bitmap.SetPixel(x, y, Color.FromKnownColor(KnownColor.Aqua));
            }

            // Set fixed form size
            //pictureBox1.MinimumSize = pictureBox1.MaximumSize = new System.Drawing.Size(mode.XRes, mode.YRes);

            // Disable unnecessary caption bar buttons
            this.MinimizeBox = this.MaximizeBox = false;
         
            // Enable double buffering to prevent flicker
            this.DoubleBuffered = true;

            pictureBox1.Paint += new PaintEventHandler(this.pictureBox1_Paint);
            //Initial DataGridView
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "left arm length";
            dataGridView1.Columns[1].Name = "left shoulder length";
            dataGridView1.Columns[2].Name = "left shoulder angle";
            dataGridView1.Columns[3].Name = "right arm length";
            dataGridView1.Columns[4].Name = "right shoulder length";
            dataGridView1.Columns[5].Name = "right shoulder angle";
            // Run Nuitrack. This starts sensor data processing.
            bmp = new Bitmap(pictureBox1.ClientSize.Width,
                               pictureBox1.ClientSize.Height);
            final_plot = new Bitmap(pictureBox1.ClientSize.Width,
                               pictureBox1.ClientSize.Height);
            try
            {
                Nuitrack.Run();
            }
            catch (Exception exception)
            {
                //Console.WriteLine("Cannot start Nuitrack.");
                throw exception;
            }
            
            this.Show();

        }
        ~Form1()
        {
            _bitmap.Dispose();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Release Nuitrack and remove all modules
            try
            {
                Nuitrack.onIssueUpdateEvent -= onIssueDataUpdate;

                _depthSensor.OnUpdateEvent -= onDepthSensorUpdate;
                _colorSensor.OnUpdateEvent -= onColorSensorUpdate;
                _userTracker.OnUpdateEvent -= onUserTrackerUpdate;
                _skeletonTracker.OnSkeletonUpdateEvent -= onSkeletonUpdate;
                _handTracker.OnUpdateEvent -= onHandTrackerUpdate;
                _gestureRecognizer.OnNewGesturesEvent -= onNewGestures;
                
                Nuitrack.Release();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Nuitrack release failed.");
                throw exception;
            }
        }

        // Switch visualization mode on a mouse click
        //protected override void OnClick(EventArgs args)
        //{
        //    base.OnClick(args);
        //
        //    _visualizeColorImage = !_visualizeColorImage;
        //}
        
        private void pictureBox1_Paint(object sender,
            System.Windows.Forms.PaintEventArgs e)
        {
            
            //base.OnPaint(args);

            // Update Nuitrack data. Data will be synchronized with skeleton time stamps.
            try
            {
                Nuitrack.Update(_skeletonTracker);
            }
            catch (LicenseNotAcquiredException exception)
            {
                Console.WriteLine("LicenseNotAcquired exception. Exception: ", exception);
                throw exception;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Nuitrack update failed. Exception: ", exception);
            }

            // Draw a bitmap
            
            e.Graphics.DrawImage(_bitmap.Bitmap, new System.Drawing.Point(0, 0));
            
            // Draw skeleton joints
            //Mat skeleton_img= BitmapToMat(_bitmap.Bitmap);
            try
            {
                if (_skeletonData != null)
                {


                    const int jointSize = 22;


                    foreach (var skeleton in _skeletonData.Skeletons)
                    {
                        SolidBrush brush = new SolidBrush(Color.FromArgb(255 - 40 * skeleton.ID, 0, 0));
                        //System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 15);
                        SolidBrush brush_text = new SolidBrush(Color.White);
                        SolidBrush brush_measured = new SolidBrush(Color.LightBlue);
                        Pen pen = new Pen(Color.WhiteSmoke);
                        foreach (var joint in skeleton.Joints)
                        {

                            PointF centerp = new PointF((joint.Proj.X * _bitmap.Width - jointSize / 2), (joint.Proj.Y * _bitmap.Height - jointSize / 2));
                            //OpenCvSharp.Point2f centerp = new OpenCvSharp.Point2f(joint.Proj.X * _bitmap.Width - jointSize / 2, joint.Proj.Y * _bitmap.Height - jointSize / 2);
                            //RotatedRect rect = new RotatedRect(centerp,joint_size,360);
                            //Cv2.Ellipse(skeleton_img,rect, Scalar.White,-1);
                            //Draw joint index in the picturebox
                            int joint_index = (int)joint.Type;
                            // index 5~9
                            if (checked_direct == "left" && joint_index >= 5 && joint_index <= 9)
                            {

                                e.Graphics.FillEllipse(brush_measured, joint.Proj.X * _bitmap.Width - jointSize / 2,
                                                     joint.Proj.Y * _bitmap.Height - jointSize / 2, jointSize, jointSize);
                            }
                            //index 11~15
                            else if (checked_direct == "right" && joint_index >= 11 && joint_index <= 15)
                            {
                                e.Graphics.FillEllipse(brush_measured, joint.Proj.X * _bitmap.Width - jointSize / 2,
                                                     joint.Proj.Y * _bitmap.Height - jointSize / 2, jointSize, jointSize);
                            }
                            else
                            {
                                e.Graphics.FillEllipse(brush_text, joint.Proj.X * _bitmap.Width - jointSize / 2,
                                                     joint.Proj.Y * _bitmap.Height - jointSize / 2, jointSize, jointSize);
                            }


                            //e.Graphics.DrawString(joint_index,drawFont, brush_text, centerp);

                        }
                        //Connect Joint using line
                        for (int i = 0; i < bones.Count; i++)
                        {

                            Joint j1 = skeleton.Joints[(int)bones[i].from];
                            Joint j2 = skeleton.Joints[(int)bones[i].to];

                            if (j1.Confidence < 0.4 || j2.Confidence < 0.4)
                            {
                                continue;
                            }
                            PointF p1
                                = new PointF(j1.Proj.X * _bitmap.Width, j1.Proj.Y * _bitmap.Height);
                            PointF p2
                                = new PointF(j2.Proj.X * _bitmap.Width, j2.Proj.Y * _bitmap.Height);
                            e.Graphics.DrawLine(pen, p1, p2);
                            //Cv2.Line(skeleton_img,p1,p2,Scalar.Red);
                        }

                        Joint left_shoulder = skeleton.Joints[(int)JointType.LeftShoulder];
                        Joint left_hand = skeleton.Joints[(int)JointType.LeftHand];
                        Joint left_collar = skeleton.Joints[(int)JointType.LeftCollar];
                        Joint right_shoulder = skeleton.Joints[(int)JointType.RightShoulder];
                        Joint right_hand = skeleton.Joints[(int)JointType.RightHand];
                        Joint right_collar = skeleton.Joints[(int)JointType.RightCollar];
                        Joint body_torso = skeleton.Joints[(int)JointType.Torso];
                        List<double> left_measure = Shoulder_angle(body_torso, left_collar, left_shoulder, left_hand, obtain_shoulder_length: true);
                        List<double> right_measure = Shoulder_angle(body_torso, right_collar, right_shoulder, right_hand, obtain_shoulder_length: true);


                        PointF hand_points = new PointF();
                        Joint hand_joints = new Joint();
                        if (checked_direct == "left")
                        {
                            hand_points
                            = new PointF(left_hand.Proj.X * _bitmap.Width, left_hand.Proj.Y * _bitmap.Height);
                            hand_joints = left_hand;
                        }
                        else if (checked_direct == "right")
                        {
                            hand_points
                            = new PointF(right_hand.Proj.X * _bitmap.Width, right_hand.Proj.Y * _bitmap.Height);
                            hand_joints = right_hand;
                        }


                        dataGridView1.Rows[0].Cells[0].Value = ((left_measure.ElementAt(0) / 10)).ToString("0.00") + "cm";
                        dataGridView1.Rows[0].Cells[1].Value = ((left_measure.ElementAt(1) / 10)).ToString("0.00") + "cm";
                        dataGridView1.Rows[0].Cells[2].Value = ((int)left_measure.ElementAt(2)).ToString() + "*";
                        dataGridView1.Rows[0].Cells[3].Value = ((right_measure.ElementAt(0) / 10)).ToString("0.00") + "cm";
                        dataGridView1.Rows[0].Cells[4].Value = (right_measure.ElementAt(1) / 10).ToString("0.00") + "cm";
                        dataGridView1.Rows[0].Cells[5].Value = ((int)right_measure.ElementAt(2)).ToString() + "*";

                        
                        
                        //plot calibration line
                        if (side_item == 0 || side_item == 1)
                        {
                            
                            
                            if (calib_start_line == true)
                            {
                                //obtain left Side body part joint 
                                Joint j_shoulder = new Joint();
                                Joint j_hand = new Joint();
                                bool checked_left = true;
                                if (checked_direct == "left")
                                {
                                    j_shoulder = skeleton.Joints[(int)JointType.LeftShoulder];
                                    j_hand = skeleton.Joints[(int)JointType.LeftHand];
                                    checked_left = true;
                                }
                                else if (checked_direct == "right")
                                {
                                    j_shoulder = skeleton.Joints[(int)JointType.RightShoulder];
                                    j_hand = skeleton.Joints[(int)JointType.RightHand];
                                    checked_left = false;
                                }
                                PointF p_shoulder
                                    = new PointF(j_shoulder.Proj.X * _bitmap.Width, j_shoulder.Proj.Y * _bitmap.Height);
                                PointF p_hand
                                    = new PointF(j_hand.Proj.X * _bitmap.Width, j_hand.Proj.Y * _bitmap.Height);


                                calib_shoulder = p_shoulder;
                                calib_hand_start = p_hand;
                                start_shoulder = j_shoulder;
                                start_hand = j_hand;

                                Vector3 real_arm = new Vector3(j_shoulder.Real.X - j_hand.Real.X, j_shoulder.Real.Y - j_hand.Real.Y,
                                    j_shoulder.Real.Z - j_hand.Real.Z);
                                double real_arm_mags = Math.Sqrt(Math.Pow(real_arm.X, 2) + Math.Pow(real_arm.Y, 2) + Math.Pow(real_arm.Z, 2));

                                arm_line_length = Math.Sqrt(Math.Pow(p_shoulder.X -
                                    p_hand.X, 2) + Math.Pow(p_shoulder.Y -
                                    p_hand.Y, 2));

                                real_arm_length = real_arm_mags / 1000;


                                split_point_list = findSplit_Points(p_hand, p_shoulder, split_angle_pie, checked_left);

                                split_second.Add(DateTime.Now);
                                left_count = 0;
                                calib_start_line = false;
                                start_num = frame_count;


                                

                            }

                            if (!calib_hand_start.IsEmpty)
                            {

                                Pen pen_start = new Pen(Color.Red);
                                //pen_start.Width = 5;
                                e.Graphics.DrawLine(pen_start, calib_shoulder, calib_hand_start);
                                

                            }
                            if (!calib_hand_start.IsEmpty)
                            {
                                //Console.WriteLine("hand_pints: {0}", hand_points);
                                if (collinear(calib_shoulder, hand_points, split_point_list[left_count]))
                                {
                                    Console.WriteLine("Record time!!");
                                    split_second.Add(DateTime.Now);
                                    split_point_list_3d.Add(hand_joints);
                                    left_count++;
                                }
                                
                            }

                            //Plot Calibration end line 

                            if (calib_end_line == true)
                            {
                                //obtain left Side body part joint 

                                Joint j_hand = new Joint();
                                if (checked_direct == "left")
                                {

                                    j_hand = skeleton.Joints[(int)JointType.LeftHand];
                                }
                                else if (checked_direct == "right")
                                {

                                    j_hand = skeleton.Joints[(int)JointType.RightHand];
                                }
                                PointF p_hand
                                    = new PointF(j_hand.Proj.X * _bitmap.Width, j_hand.Proj.Y * _bitmap.Height);

                                calib_hand_end = p_hand;


                                calib_end_line = false;
                                real_gap_angle = Shoulder_angle(j_hand, start_shoulder, start_shoulder, start_hand, false).ElementAt(1);
                                real_area = Math.PI * Math.Pow(real_arm_length, 2) * (real_gap_angle / 360);
                                double motion_distance = 2 * Math.PI * real_arm_length * (split_angle_pie / 360);
                                double last_speed = 0;
                                motion_distance_saving = motion_distance;
                                for (int i = 1; i < split_second.Count; i++)
                                {
                                    double time_interval = compute_seconds
                                        (split_second[i - 1], split_second[i]);
                                    double accele = Math.Abs((motion_distance - last_speed * time_interval) * 2 / (time_interval * time_interval));
                                    //double accele = Math.Abs(((motion_distance / time_interval) - last_speed) / time_interval);
                                    last_speed = accele * time_interval;
                                    time_intervals.Add((float)time_interval);
                                    acceleration.Add((float)accele);
                                    Console.WriteLine("Time{0}: {1}, accele: {2}", i, time_interval, accele);
                                }
                                
                            }
                            if (!calib_hand_end.IsEmpty)
                            {

                                Pen pen_start = new Pen(Color.Black);
                                pen_start.Width = 3;
                                PointF fixed_calib_hand_end = CalculatePoint
                                    (calib_shoulder, calib_hand_end, arm_line_length);

                                e.Graphics.DrawLine(pen_start, calib_shoulder, fixed_calib_hand_end);





                                //left
                                SolidBrush brush_fill = new SolidBrush(Color.FromArgb(236, 228, 228));
                                SolidBrush brush_fill_quick = new SolidBrush(Color.LightSeaGreen);
                                SolidBrush brush_fill_slow = new SolidBrush(Color.Red);
                                RectangleF left_rect =
                                    new RectangleF(calib_shoulder.X - (float)arm_line_length,
                                    calib_shoulder.Y - (float)arm_line_length
                                    , (float)arm_line_length * 2, (float)arm_line_length * 2);
                                double start_angle = vect2d_angle(calib_shoulder, new PointF(calib_shoulder.X, calib_shoulder.Y + (float)arm_line_length), calib_hand_start);
                                double between_angle = vect2d_angle(calib_shoulder, calib_hand_start, calib_hand_end);


                                //Vector2 start_vector = new 
                                //    Vector2(calib_left_hand_start.X- calib_left_shoulder.X, calib_left_hand_start.Y- calib_left_shoulder.Y);
                                round_count = Math.Floor(between_angle / split_angle_pie);
                                Console.WriteLine("Angle: {0}", between_angle);
                                //for (int i = 0; i < ((int)round_count); i++) {
                                //    Vector2 new_vector= RotateBy(start_vector,-split_angle_pie,false);
                                //    left_split_point_list.Add(
                                //        new PointF(new_vector.X+calib_left_shoulder.X,new_vector.Y+calib_left_shoulder.Y));
                                //    start_vector = new_vector;   
                                //}

                                float final_angle = 0;
                                if ((between_angle - round_count * split_angle_pie) > 0)
                                {
                                    final_angle = (float)between_angle - (float)round_count * split_angle_pie;

                                }
                                Pen pen_split = new Pen(Color.Black);
                                pen_split.Width = 3;
                                

                                Color[] colors = new Color[] {Color.Orange,Color.Yellow,Color.Gold,Color.LightBlue
                        ,Color.Blue,Color.White,Color.Orange,Color.Yellow,Color.Gold,Color.LightBlue
                        ,Color.Blue,Color.White,Color.Orange,Color.Yellow,Color.Gold,Color.LightBlue
                        ,Color.Blue,Color.White};

                                Console.WriteLine("acce: {0}, round_count: {1}", acceleration.Count, round_count);
                                if (acceleration.Count >= round_count && round_count > 0)
                                {

                                    for (int i = 0; i <round_count; i++)
                                    {
                                        //Pen pen_arrow = new Pen(Color.White, (float)1);
                                        Pen pen_arrow = new Pen(colors[i], (float)1.5);
                                        GraphicsPath capPath = new GraphicsPath();
                                        capPath.AddLine(-10, 0, 10, 0);
                                        capPath.AddLine(-10, 0, 0, 10);
                                        capPath.AddLine(0, 10, 10, 0);

                                        pen_arrow.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);

                                        //System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
                                        //SolidBrush brush_string = new SolidBrush(Color.White);


                                        Console.WriteLine(split_point_list[i]);

                                        if (i == 0)
                                        {
                                            e.Graphics.FillPolygon(brush_fill_quick,
                                                new PointF[] { calib_shoulder, calib_hand_start, split_point_list[i] });
                                            //e.Graphics.DrawLine(pen_start, calib_left_hand_start, left_split_point_list[i]);

                                            e.Graphics.DrawLine(pen_arrow, split_point_list[i],
                                                CalculatePoint(split_point_list[i], split_point_list[i + 1], 40));
                                            //String accele_text = String.Format("{0:F} m/s2", acceleration[i]);
                                            //e.Graphics.DrawString(accele_text, drawFont, brush_string, split_point_list[i]);
                                        }
                                        else
                                        {   
                                            if(acceleration[i]>acceleration[i-1])
                                                e.Graphics.FillPolygon(brush_fill_quick,
                                                    new PointF[] { calib_shoulder, split_point_list[i - 1], split_point_list[i] });
                                            else
                                                e.Graphics.FillPolygon(brush_fill_slow,
                                                    new PointF[] { calib_shoulder, split_point_list[i - 1], split_point_list[i] });
                                            //e.Graphics.DrawLine(pen_start, left_split_point_list[i - 1], left_split_point_list[i]);

                                            e.Graphics.DrawLine(pen_arrow, split_point_list[i],
                                                CalculatePoint(split_point_list[i], split_point_list[i + 1], 40));
                                            //String accele_text = String.Format("{0:F} m/s2", acceleration[i]);
                                            //e.Graphics.DrawString(accele_text, drawFont, brush_string, split_point_list[i]);
                                        }
                                        e.Graphics.DrawLine(pen_split, calib_shoulder, split_point_list[i]);
                                        pen_arrow.Dispose();
                                    }
                                    
                                   e.Graphics.FillPolygon(brush_fill_slow,
                                               new PointF[] { calib_shoulder, split_point_list[(int)round_count - 1], fixed_calib_hand_end });

                                    e.Graphics.DrawLine(pen_start, split_point_list[(int)round_count - 1], fixed_calib_hand_end);


                                    for (int i = 0; i < round_count; i++)
                                    {
                                        //Pen pen_arrow = new Pen(Color.White, (float)1);
                                        ////Pen pen_arrow = new Pen(colors[i], (float)1.5);
                                        //GraphicsPath capPath = new GraphicsPath();
                                        //capPath.AddLine(-10, 0, 10, 0);
                                        //capPath.AddLine(-10, 0, 0, 10);
                                        //capPath.AddLine(0, 10, 10, 0);

                                        //pen_arrow.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);

                                        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
                                        SolidBrush brush_string = new SolidBrush(Color.White);


                                       

                                        if (i == 0)
                                        {
                                            //e.Graphics.FillPolygon(brush_fill_quick,
                                            //    new PointF[] { calib_shoulder, calib_hand_start, split_point_list[i] });
                                            //e.Graphics.DrawLine(pen_start, calib_left_hand_start, left_split_point_list[i]);

                                            //e.Graphics.DrawLine(pen_arrow, split_point_list[i],
                                            //    CalculatePoint(split_point_list[i], split_point_list[i + 1], 40));
                                            String accele_text = String.Format("{0:F} m/s2", acceleration[i]);
                                            e.Graphics.DrawString(accele_text, drawFont, brush_string, split_point_list[i]);
                                        }
                                        else
                                        {
                                            //if (acceleration[i] > acceleration[i - 1])
                                            //    e.Graphics.FillPolygon(brush_fill_quick,
                                            //        new PointF[] { calib_shoulder, split_point_list[i - 1], split_point_list[i] });
                                            //else
                                            //    e.Graphics.FillPolygon(brush_fill_slow,
                                            //        new PointF[] { calib_shoulder, split_point_list[i - 1], split_point_list[i] });
                                            ////e.Graphics.DrawLine(pen_start, left_split_point_list[i - 1], left_split_point_list[i]);

                                            //e.Graphics.DrawLine(pen_arrow, split_point_list[i],
                                            //    CalculatePoint(split_point_list[i], split_point_list[i + 1], 40));
                                            String accele_text = String.Format("{0:F} m/s2", acceleration[i]);
                                            e.Graphics.DrawString(accele_text, drawFont, brush_string, split_point_list[i]);
                                        }
                                        //e.Graphics.DrawLine(pen_split, calib_shoulder, split_point_list[i]);
                                        //pen_arrow.Dispose();
                                    }


                                    if (save_state)
                                    {
                                        end_num = frame_count;
                                        save_state = false;
                                        pictureBox1.DrawToBitmap(final_plot, pictureBox1.ClientRectangle);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please make sure human skeleton tracking is stable and try more movement of your arm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                                    clear_line_bt_Click(sender, e);
                                }

                            }
                        }
                        //Arm-axis
                        else if (side_item == 2)
                        {
                            if (calib_start_line == true)
                            {
                                //obtain left Side body part joint 
                                Joint j_shoulder = new Joint();
                                Joint j_hand = new Joint();
                                bool checked_left = true;
                                if (checked_direct == "left")
                                {
                                    j_shoulder = skeleton.Joints[(int)JointType.LeftElbow];
                                    j_hand = skeleton.Joints[(int)JointType.LeftHand];
                                    checked_left = true;
                                }
                                else if (checked_direct == "right")
                                {
                                    j_shoulder = skeleton.Joints[(int)JointType.RightElbow];
                                    j_hand = skeleton.Joints[(int)JointType.RightHand];
                                    checked_left = false;
                                }
                                PointF p_shoulder
                                    = new PointF(j_shoulder.Proj.X * _bitmap.Width, j_shoulder.Proj.Y * _bitmap.Height);
                                PointF p_hand
                                    = new PointF(j_hand.Proj.X * _bitmap.Width, j_hand.Proj.Y * _bitmap.Height);

                                calib_shoulder = p_shoulder;
                                calib_hand_start = p_hand;
                                start_shoulder = j_shoulder;
                                start_hand = j_hand;

                                Vector3 real_arm = new Vector3(j_shoulder.Real.X - j_hand.Real.X, j_shoulder.Real.Y - j_hand.Real.Y,
                                   j_shoulder.Real.Z - j_hand.Real.Z);
                                double real_arm_mags = Math.Sqrt(Math.Pow(real_arm.X, 2) + Math.Pow(real_arm.Y, 2) + Math.Pow(real_arm.Z, 2));

                                arm_line_length = Math.Sqrt(Math.Pow(p_shoulder.X -
                                    p_hand.X, 2) + Math.Pow(p_shoulder.Y -
                                    p_hand.Y, 2));

                                real_arm_length = real_arm_mags / 1000;


                                split_point_list = findSplit_Points(p_hand, p_shoulder, split_angle_pie, checked_left);

                                split_second.Add(DateTime.Now);
                                left_count = 0;
                                calib_start_line = false;

                                start_num = frame_count;
                            }

                            if (!calib_hand_start.IsEmpty)
                            {

                                Pen pen_start = new Pen(Color.Red);
                                //pen_start.Width = 5;
                                e.Graphics.DrawLine(pen_start, calib_shoulder, calib_hand_start);

                            }
                            if (!calib_hand_start.IsEmpty)
                            {
                                //Console.WriteLine("hand_pints: {0}", hand_points);
                                if (collinear(calib_shoulder, hand_points, split_point_list[left_count]))
                                {
                                    Console.WriteLine("Record time!!");
                                    split_second.Add(DateTime.Now);
                                    split_point_list_3d.Add(hand_joints);
                                    left_count++;
                                }
                            }

                            //Plot Calibration end line 

                            if (calib_end_line == true)
                            {
                                //obtain left Side body part joint 

                                Joint j_hand = new Joint();
                                if (checked_direct == "left")
                                {

                                    j_hand = skeleton.Joints[(int)JointType.LeftHand];
                                }
                                else if (checked_direct == "right")
                                {

                                    j_hand = skeleton.Joints[(int)JointType.RightHand];
                                }
                                PointF p_hand
                                    = new PointF(j_hand.Proj.X * _bitmap.Width, j_hand.Proj.Y * _bitmap.Height);

                                calib_hand_end = p_hand;


                                calib_end_line = false;
                                real_gap_angle = Shoulder_angle(j_hand, start_shoulder, start_shoulder, start_hand, false).ElementAt(1);
                                real_area = Math.PI * Math.Pow(real_arm_length, 2) * (real_gap_angle / 360);
                                double motion_distance = 2 * Math.PI * real_arm_length * (split_angle_pie / 360);
                                double last_speed = 0;
                                motion_distance_saving = motion_distance;
                                for (int i = 1; i < split_second.Count; i++)
                                {
                                    double time_interval = compute_seconds
                                        (split_second[i - 1], split_second[i]);
                                    double accele = Math.Abs((motion_distance - last_speed * time_interval) * 2 / (time_interval * time_interval));
                                    last_speed = accele * time_interval;
                                    time_intervals.Add((float)time_interval);
                                    acceleration.Add((float)accele);
                                    Console.WriteLine("Time{0}: {1}, accele: {2}", i, time_interval, accele);
                                }
                            }
                            if (!calib_hand_end.IsEmpty)
                            {

                                Pen pen_start = new Pen(Color.Black);
                                pen_start.Width = 3;
                                PointF fixed_calib_hand_end = CalculatePoint
                                    (calib_shoulder, calib_hand_end, arm_line_length);

                                e.Graphics.DrawLine(pen_start, calib_shoulder, fixed_calib_hand_end);





                                //left
                                SolidBrush brush_fill = new SolidBrush(Color.FromArgb(236, 228, 228));
                                SolidBrush brush_fill_quick = new SolidBrush(Color.LightSeaGreen);
                                SolidBrush brush_fill_slow = new SolidBrush(Color.Red);
                                RectangleF left_rect =
                                    new RectangleF(calib_shoulder.X - (float)arm_line_length,
                                    calib_shoulder.Y - (float)arm_line_length
                                    , (float)arm_line_length * 2, (float)arm_line_length * 2);
                                double start_angle = vect2d_angle(calib_shoulder, new PointF(calib_shoulder.X, calib_shoulder.Y + (float)arm_line_length), calib_hand_start);
                                double between_angle = vect2d_angle(calib_shoulder, calib_hand_start, calib_hand_end);


                                //Vector2 start_vector = new 
                                //    Vector2(calib_left_hand_start.X- calib_left_shoulder.X, calib_left_hand_start.Y- calib_left_shoulder.Y);
                                round_count = Math.Floor(between_angle / split_angle_pie);
                                Console.WriteLine("Angle: {0}", between_angle);
                                //for (int i = 0; i < ((int)round_count); i++) {
                                //    Vector2 new_vector= RotateBy(start_vector,-split_angle_pie,false);
                                //    left_split_point_list.Add(
                                //        new PointF(new_vector.X+calib_left_shoulder.X,new_vector.Y+calib_left_shoulder.Y));
                                //    start_vector = new_vector;   
                                //}

                                float final_angle = 0;
                                if ((between_angle - round_count * split_angle_pie) > 0)
                                {
                                    final_angle = (float)between_angle - (float)round_count * split_angle_pie;

                                }
                                Pen pen_split = new Pen(Color.Black);
                                pen_split.Width = 3;
                                //pen_split.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

                                Color[] colors = new Color[] {Color.Orange,Color.Yellow,Color.Gold,Color.LightBlue
                        ,Color.Blue,Color.White,Color.Orange,Color.Yellow,Color.Gold,Color.LightBlue
                        ,Color.Blue,Color.White,Color.Orange,Color.Yellow,Color.Gold,Color.LightBlue
                        ,Color.Blue,Color.White};
                                if (round_count <= acceleration.Count && round_count > 0)
                                {
                                    for (int i = 0; i < round_count; i++)
                                    {

                                        //Pen pen_arrow = new Pen(color, (float)1);
                                        Pen pen_arrow = new Pen(colors[i], (float)1);
                                        GraphicsPath capPath = new GraphicsPath();
                                        capPath.AddLine(-10, 0, 10, 0);
                                        capPath.AddLine(-10, 0, 0, 10);
                                        capPath.AddLine(0, 10, 10, 0);

                                        pen_arrow.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);

                                        //System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10);
                                        //SolidBrush brush_string = new SolidBrush(colors[i]);


                                        Console.WriteLine(split_point_list[i]);

                                        if (i == 0)
                                        {
                                            e.Graphics.FillPolygon(brush_fill_quick,
                                                new PointF[] { calib_shoulder, calib_hand_start, split_point_list[i] });
                                            //e.Graphics.DrawLine(pen_start, calib_left_hand_start, left_split_point_list[i]);

                                            e.Graphics.DrawLine(pen_arrow, split_point_list[i],
                                                CalculatePoint(split_point_list[i], split_point_list[i + 1], 40));
                                            //String accele_text = String.Format("{0:F} m/s2", acceleration[i]);
                                            //e.Graphics.DrawString(accele_text, drawFont, brush_string, split_point_list[i]);
                                        }
                                        else
                                        {   
                                            if(acceleration[i]>acceleration[i-1])
                                                e.Graphics.FillPolygon(brush_fill_quick,
                                                    new PointF[] { calib_shoulder, split_point_list[i - 1], split_point_list[i] });
                                            else
                                                e.Graphics.FillPolygon(brush_fill_slow,
                                                   new PointF[] { calib_shoulder, split_point_list[i - 1], split_point_list[i] });
                                            //e.Graphics.DrawLine(pen_start, left_split_point_list[i - 1], left_split_point_list[i]);

                                            e.Graphics.DrawLine(pen_arrow, split_point_list[i],
                                                CalculatePoint(split_point_list[i], split_point_list[i + 1], 40));
                                            //String accele_text = String.Format("{0:F} m/s2", acceleration[i]);
                                            //e.Graphics.DrawString(accele_text, drawFont, brush_string, split_point_list[i]);
                                        }
                                        e.Graphics.DrawLine(pen_split, calib_shoulder, split_point_list[i]);
                                        pen_arrow.Dispose();
                                    }
                                    e.Graphics.FillPolygon(brush_fill_slow,
                                                new PointF[] { calib_shoulder, split_point_list[(int)round_count - 1], fixed_calib_hand_end });
                                    e.Graphics.DrawLine(pen_start, split_point_list[(int)round_count - 1], fixed_calib_hand_end);

                                    for (int i = 0; i < round_count; i++)
                                    {

                                        ////Pen pen_arrow = new Pen(color, (float)1);
                                        //Pen pen_arrow = new Pen(colors[i], (float)1);
                                        //GraphicsPath capPath = new GraphicsPath();
                                        //capPath.AddLine(-10, 0, 10, 0);
                                        //capPath.AddLine(-10, 0, 0, 10);
                                        //capPath.AddLine(0, 10, 10, 0);

                                        //pen_arrow.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);

                                        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10);
                                        SolidBrush brush_string = new SolidBrush(Color.White);


                                       // Console.WriteLine(split_point_list[i]);

                                        if (i == 0)
                                        {
                                            //e.Graphics.FillPolygon(brush_fill_quick,
                                            //    new PointF[] { calib_shoulder, calib_hand_start, split_point_list[i] });
                                            ////e.Graphics.DrawLine(pen_start, calib_left_hand_start, left_split_point_list[i]);

                                            //e.Graphics.DrawLine(pen_arrow, split_point_list[i],
                                            //    CalculatePoint(split_point_list[i], split_point_list[i + 1], 40));
                                            String accele_text = String.Format("{0:F} m/s2", acceleration[i]);
                                            e.Graphics.DrawString(accele_text, drawFont, brush_string, split_point_list[i]);
                                        }
                                        else
                                        {
                                            //if (acceleration[i] > acceleration[i - 1])
                                            //    e.Graphics.FillPolygon(brush_fill_quick,
                                            //        new PointF[] { calib_shoulder, split_point_list[i - 1], split_point_list[i] });
                                            //else
                                            //    e.Graphics.FillPolygon(brush_fill_slow,
                                            //       new PointF[] { calib_shoulder, split_point_list[i - 1], split_point_list[i] });
                                            ////e.Graphics.DrawLine(pen_start, left_split_point_list[i - 1], left_split_point_list[i]);

                                            //e.Graphics.DrawLine(pen_arrow, split_point_list[i],
                                            //    CalculatePoint(split_point_list[i], split_point_list[i + 1], 40));
                                            String accele_text = String.Format("{0:F} m/s2", acceleration[i]);
                                            e.Graphics.DrawString(accele_text, drawFont, brush_string, split_point_list[i]);
                                        }
                                        //e.Graphics.DrawLine(pen_split, calib_shoulder, split_point_list[i]);
                                        //pen_arrow.Dispose();
                                    }

                                    if (save_state)
                                    {
                                        end_num = frame_count;
                                        save_state = false;
                                        pictureBox1.DrawToBitmap(final_plot, pictureBox1.ClientRectangle);
                                    }
                                }
                                else
                                {

                                    MessageBox.Show("Please make sure human skeleton tracking is stable and try more movement of your arm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                                    clear_line_bt_Click(sender, e);

                                }

                            }

                        }
                        
                        
                        


                    }

                }
                Bitmap bp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics s = Graphics.FromImage(bp);
                s.CopyFromScreen(0, 0, 0, 0, bp.Size);
                record_bitmaps.Add(bp);
                //pictureBox1.DrawToBitmap(bmp, pictureBox1.ClientRectangle);

                frame_count++;
                
                pictureBox1.Invalidate();
            }
            catch (System.IndexOutOfRangeException index_out_exception)
            {

                if (index_out_exception.Source != null)
                {
                    MessageBox.Show("Detected human is out of bound!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    Console.WriteLine("IOException source: {0}", index_out_exception.Source);
                }
                throw;
            }
            catch (System.ArgumentOutOfRangeException arugment_out_exception)
            {
                if (arugment_out_exception.Source != null)
                {
                    MessageBox.Show("Detected human is out of bound!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Console.WriteLine("IOException source: {0}", arugment_out_exception.Source);
                }
                throw;
            }
            catch (System.NullReferenceException nll_ref_exception) {
                throw;
            }
            //plot Calibration line


            // Draw hand pointers

            //if (_handTrackerData != null)
            //{

            //    foreach (var userHands in _handTrackerData.UsersHands)
            //    {
            //        if (userHands.LeftHand != null)
            //        {
            //            HandContent hand = userHands.LeftHand.Value;
            //            int size = hand.Click ? 20 : 30;
            //            //Cv2.Ellipse(skeleton_img, 
            //            //    new RotatedRect(new Point2f(hand.X * _bitmap.Width - size / 2, hand.Y * _bitmap.Height - size / 2),new Size2f(size,size),360), Scalar.DarkBlue, -1);
            //            SolidBrush brush = new SolidBrush(Color.Aquamarine);
            //            e.Graphics.FillEllipse(brush, hand.X * _bitmap.Width - size / 2, hand.Y * _bitmap.Height - size / 2, size, size);
            //        }

            //        if (userHands.RightHand != null)
            //        {
            //            HandContent hand = userHands.RightHand.Value;
            //            int size = hand.Click ? 20 : 30;
            //            //Cv2.Ellipse(skeleton_img,
            //            //    new RotatedRect(new Point2f(hand.X * _bitmap.Width - size / 2, hand.Y * _bitmap.Height - size / 2), new Size2f(size, size), 360), Scalar.DarkRed, -1);
            //            Brush brush = new SolidBrush(Color.DarkBlue);

            //            e.Graphics.FillEllipse(brush, hand.X * _bitmap.Width - size / 2, hand.Y * _bitmap.Height - size / 2, size, size);
            //        }
            //    }
            //}

            //Cv2.ImShow("skeleton", skeleton_img);
            //e.Graphics.DrawImage(MatToBitmap(skeleton_img), 0, 0);
            // Update Form

            //this.Invalidate();
        }

        private void onIssueDataUpdate(IssuesData issuesData)
        {
            _issuesData = issuesData;
        }

        // Event handler for the DepthSensorUpdate event
        private void onDepthSensorUpdate(DepthFrame depthFrame)
        {
            _depthFrame = depthFrame;
            
        }

        // Event handler for the ColorSensorUpdate event
        private void onColorSensorUpdate(ColorFrame colorFrame)
        {
            
            if (!_visualizeColorImage)
                return;

            _colorStreamEnabled = true;

            float wStep = (float)_bitmap.Width / colorFrame.Cols;
            float hStep = (float)_bitmap.Height / colorFrame.Rows;

            float nextVerticalBorder = hStep;

            Byte[] data = colorFrame.Data;
            int colorPtr = 0;
            int bitmapPtr = 0;
            const int elemSizeInBytes = 3;

            for (int i = 0; i < _bitmap.Height; ++i)
            {
                if (i == (int)nextVerticalBorder)
                {
                    colorPtr += colorFrame.Cols * elemSizeInBytes;
                    nextVerticalBorder += hStep;
                }

                int offset = 0;
                int argb = data[colorPtr]
                    | (data[colorPtr + 1] << 8)
                    | (data[colorPtr + 2] << 16)
                    | (0xFF << 24);
                float nextHorizontalBorder = wStep;
                for (int j = 0; j < _bitmap.Width; ++j)
                {
                    if (j == (int)nextHorizontalBorder)
                    {
                        offset += elemSizeInBytes;
                        argb = data[colorPtr + offset]
                            | (data[colorPtr + offset + 1] << 8)
                            | (data[colorPtr + offset + 2] << 16)
                            | (0xFF << 24);
                        nextHorizontalBorder += wStep;
                    }

                    _bitmap.Bits[bitmapPtr++] = argb;
                }
            }
        }

        // Event handler for the UserTrackerUpdate event
        private void onUserTrackerUpdate(UserFrame userFrame)
        {
            if (_visualizeColorImage && _colorStreamEnabled)
                return;
            if (_depthFrame == null)
                return;

            const int MAX_LABELS = 7;
            bool[] labelIssueState = new bool[MAX_LABELS];
            for (UInt16 label = 0; label < MAX_LABELS; ++label)
            {
                labelIssueState[label] = false;
                if (_issuesData != null)
                {
                    FrameBorderIssue frameBorderIssue = _issuesData.GetUserIssue<FrameBorderIssue>(label);
                    labelIssueState[label] = (frameBorderIssue != null);
                }
            }

            float wStep = (float)_bitmap.Width / _depthFrame.Cols;
            float hStep = (float)_bitmap.Height / _depthFrame.Rows;

            float nextVerticalBorder = hStep;

            Byte[] dataDepth = _depthFrame.Data;
            Byte[] dataUser = userFrame.Data;
            int dataPtr = 0;
            int bitmapPtr = 0;
            const int elemSizeInBytes = 2;
            for (int i = 0; i < _bitmap.Height; ++i)
            {
                if (i == (int)nextVerticalBorder)
                {
                    dataPtr += _depthFrame.Cols * elemSizeInBytes;
                    nextVerticalBorder += hStep;
                }

                int offset = 0;
                int argb = 0;
                int label = dataUser[dataPtr] | dataUser[dataPtr + 1] << 8;
                int depth = Math.Min(255, (dataDepth[dataPtr] | dataDepth[dataPtr + 1] << 8) / 32);
                float nextHorizontalBorder = wStep;
                for (int j = 0; j < _bitmap.Width; ++j)
                {
                    if (j == (int)nextHorizontalBorder)
                    {
                        offset += elemSizeInBytes;
                        label = dataUser[dataPtr + offset] | dataUser[dataPtr + offset + 1] << 8;
                        if (label == 0)
                            depth = Math.Min(255, (dataDepth[dataPtr + offset] | dataDepth[dataPtr + offset + 1] << 8) / 32);
                        nextHorizontalBorder += wStep;
                    }

                    if (label > 0)
                    {
                        int user = label * 40;
                        if (!labelIssueState[label])
                            user += 40;
                        argb = 0 | (user << 8) | (0 << 16) | (0xFF << 24);
                    }
                    else
                    {
                        argb = depth | (depth << 8) | (depth << 16) | (0xFF << 24);
                    }

                    _bitmap.Bits[bitmapPtr++] = argb;
                }
            }
        }

        // Event handler for the SkeletonUpdate event
        private void onSkeletonUpdate(SkeletonData skeletonData)
        {
            _skeletonData = skeletonData;
        }

        // Event handler for the HandTrackerUpdate event
        private void onHandTrackerUpdate(HandTrackerData handTrackerData)
        {
            _handTrackerData = handTrackerData;
        }

        // Event handler for the gesture detection event
        private void onNewGestures(GestureData gestureData)
        {
            // Display the information about detected gestures in the console
            foreach (var gesture in gestureData.Gestures)
                Console.WriteLine("Recognized {0} from user {1}", gesture.Type.ToString(), gesture.UserID);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            _visualizeColorImage = !_visualizeColorImage;
            
        }

        private void calib_start_Click(object sender, EventArgs e)
        {
            if (_skeletonData.NumUsers != 0)
            {
                split_point_list.Clear();
                
                split_second.Clear();
                left_count = 0;
                
                calib_start_line = true;
            }
            else {
                
                MessageBox.Show("No detected user","Error",MessageBoxButtons.OK,MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
               
            }
        }

        private void calib_end_Click(object sender, EventArgs e)
        {
            if ( !calib_hand_start.IsEmpty&& calib_start_line==false)
            {
                calib_end_line = true;
            }
            else {
                MessageBox.Show("Please label the start line firstly", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void clear_line_bt_Click(object sender, EventArgs e)
        {
            start_num = 0;
            motion_distance_saving = 0;
            end_num = 99;
            frame_count = 0;
            record_bitmaps.Clear();
            save_state = true;
            split_point_list_3d.Clear();
            split_point_list.Clear();
            left_count = 0;
            split_second.Clear();
            time_intervals.Clear();
            acceleration.Clear();
            measured_joints.Clear();
            save_bmp = false;
            //checked_direct = "";

            ///if (front_or_side_box.SelectedItem != null) {
            //    side_item = 0;
            //}
            

            if (!calib_hand_start.IsEmpty) {
                calib_hand_start = new PointF();
            }
            

            if (!calib_hand_end.IsEmpty)
            {
                calib_hand_end = new PointF();
            }
            
        }
        
        private void button_process_Click(object sender, EventArgs e)
        {
            if (!calib_hand_end.IsEmpty && !calib_hand_end.IsEmpty)
            {
                DialogResult dialogResult = MessageBox.Show("Whether to save data with video?", "option", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                

               
     
                //Bitmap final_bmp = new Bitmap(record_bitmaps[start_num]);
                Console.WriteLine("record length: {0}, start: {1}, end: {2}"
                    ,record_bitmaps.Count,start_num,end_num);
                //bmp.Save("C://Users//hpsin//Desktop//test.png", ImageFormat.Png);
                String filename = saveFile(final_plot);
                
                if (filename == "")
                {
                    clear_line_bt_Click(sender, e);
                }
                else
                {
                    

                    string file_image_name = Path.GetFileName(filename);
                    string directory_path = Path.GetDirectoryName(filename);
                    string plane_name = front_or_side_box.SelectedItem.ToString();
                    string txt_name = file_image_name.Split('.')[0]+plane_name.Split(' ')[0] + "_report.txt";
                    string txt_path = Path.Combine(directory_path, txt_name);
                    string avi_name = file_image_name.Split('.')[0] + plane_name.Split(' ')[0] + ".avi";

                    if (dialogResult == DialogResult.Yes)
                    {
                        //do something
                        VideoFileWriter writer = new VideoFileWriter();
                        writer.Open(Path.Combine(directory_path, avi_name), 848+200, 480+200, 25, VideoCodec.MPEG4);
      
                        for (int i = start_num-1; i <= end_num; i++)
                        {

                            writer.WriteVideoFrame(record_bitmaps[i]);
                        }
                        writer.Close();
                    }

                    // Coronal plane
                    string[] lines = new string[23];
                    if ((int)side_item == 0)
                    {

                        lines[0] = String.Format("Human arm length: {0:F} m", real_arm_length);
                        lines[1] = String.Format("Abduction angle: {0:F}", real_gap_angle);
                        lines[2] = String.Format("Motion area: {0:F} m2", real_area);
                        lines[3] = String.Format("Index      X_proj       Y_proj       Z_proj      X_real       Y_real       Z_real     Time_intervel(s)     Acceleration(m/s2)       motion_distance");
                        
                       // String.Format("Motion time interval: ({0}) s", String.Join(", ", time_intervals.ToArray())),
                      //  String.Format("Motion acceleration: ({0}) m/s2", String.Join(", ", acceleration.ToArray()))};

                        for (int i = 0; i <round_count; i++) {
                            if (i == 0)
                            {

                                lines[3 + i + 1] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                 , new object[] { i, start_hand.Real.X, start_hand.Real.Y, start_hand.Real.Z, start_hand.Proj.X, start_hand.Proj.Y, start_hand.Proj.Z, 0, 0, motion_distance_saving });
                                lines[3 + i + 2] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                                                    , new object[] { i+1, split_point_list_3d[i].Real.X, split_point_list_3d[i].Real.Y, split_point_list_3d[i].Real.Z, split_point_list_3d[i].Proj.X, split_point_list_3d[i].Proj.Y, split_point_list_3d[i].Proj.Z, time_intervals[i], acceleration[i], motion_distance_saving });
                            }
                            else
                                lines[3 + i + 2] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                                                    , new object[] { i+1, split_point_list_3d[i].Real.X, split_point_list_3d[i].Real.Y, split_point_list_3d[i].Real.Z, split_point_list_3d[i].Proj.X, split_point_list_3d[i].Proj.Y, split_point_list_3d[i].Proj.Z, time_intervals[i], acceleration[i], motion_distance_saving });
                        }
                    }

                    // Sagittal plane
                    else if ((int)side_item == 1)
                    {
                        


                        lines[0] = String.Format("Human arm length: {0:F} m", real_arm_length);
                        lines[1] = String.Format("Flexion angle: {0:F}", real_gap_angle);
                        lines[2] = String.Format("Motion area: {0:F} m2", real_area);
                        lines[3] = String.Format("Index      X_proj       Y_proj       Z_proj      X_real       Y_real       Z_real     Time_intervel(s)     Acceleration(m/s2)       motion_distance");

                        // String.Format("Motion time interval: ({0}) s", String.Join(", ", time_intervals.ToArray())),
                        //  String.Format("Motion acceleration: ({0}) m/s2", String.Join(", ", acceleration.ToArray()))};

                        for (int i = 0; i < round_count; i++)
                        {
                            if (i == 0)
                            {

                                lines[3 + i + 1] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                 , new object[] { i, start_hand.Real.X, start_hand.Real.Y, start_hand.Real.Z, start_hand.Proj.X, start_hand.Proj.Y, start_hand.Proj.Z, 0, 0, motion_distance_saving });
                                lines[3 + i + 2] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                                                    , new object[] { i+1, split_point_list_3d[i].Real.X, split_point_list_3d[i].Real.Y, split_point_list_3d[i].Real.Z, split_point_list_3d[i].Proj.X, split_point_list_3d[i].Proj.Y, split_point_list_3d[i].Proj.Z, time_intervals[i], acceleration[i], motion_distance_saving });
                            }
                            else
                                lines[3 + i + 2] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                                                    , new object[] { i+1, split_point_list_3d[i].Real.X, split_point_list_3d[i].Real.Y, split_point_list_3d[i].Real.Z, split_point_list_3d[i].Proj.X, split_point_list_3d[i].Proj.Y, split_point_list_3d[i].Proj.Z, time_intervals[i], acceleration[i], motion_distance_saving });
                        }
                    }

                    //Arm-axis plane
                    else if ((int)side_item == 2) {
                        

                        lines[0] = String.Format("Human arm length: {0:F} m", real_arm_length);
                        lines[1] = String.Format("External rotation: {0:F}", real_gap_angle);
                        lines[2] = String.Format("Motion area: {0:F} m2", real_area);
                        lines[3] = String.Format("Index      X_proj       Y_proj       Z_proj      X_real       Y_real       Z_real     Time_intervel(s)     Acceleration(m/s2)       motion_distance");


                        for (int i = 0; i < round_count; i++)
                        {
                            if (i == 0)
                            {

                                lines[3 + i + 1] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                 , new object[] { i, start_hand.Real.X, start_hand.Real.Y, start_hand.Real.Z, start_hand.Proj.X, start_hand.Proj.Y, start_hand.Proj.Z, 0, 0, motion_distance_saving });
                                lines[3 + i + 2] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                                                    , new object[] { i+1, split_point_list_3d[i].Real.X, split_point_list_3d[i].Real.Y, split_point_list_3d[i].Real.Z, split_point_list_3d[i].Proj.X, split_point_list_3d[i].Proj.Y, split_point_list_3d[i].Proj.Z, time_intervals[i], acceleration[i], motion_distance_saving });
                            }
                            else
                                lines[3 + i + 2] = String.Format("{0}      {1}       {2}       {3}      {4}      {5}      {6}       {7}      {8}       {9}"
                                                                    , new object[] { i+1, split_point_list_3d[i].Real.X, split_point_list_3d[i].Real.Y, split_point_list_3d[i].Real.Z, split_point_list_3d[i].Proj.X, split_point_list_3d[i].Proj.Y, split_point_list_3d[i].Proj.Z, time_intervals[i], acceleration[i], motion_distance_saving });
                        }


                    }
                    
                    
                    System.IO.File.WriteAllLines(@txt_path, lines);
                    //produce txt report
                    
                    clear_line_bt_Click(sender, e);
                }

            }
            else {
                MessageBox.Show("Please label the start line and end line firstly", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void calib_start_Click_1(object sender, EventArgs e)
        {
            if (_skeletonData.NumUsers != 0)
            {
                if (front_or_side_box.SelectedItem != null)
                {
                    side_item = front_or_side_box.SelectedIndex;
                    string plane_name = front_or_side_box.SelectedItem.ToString();
                    if (left_radio.Checked || right_radio.Checked)
                    {
                        
                        if (left_radio.Checked)
                        {
                            checked_direct = "left";
                        }
                        else if (right_radio.Checked) {
                            checked_direct = "right";
                        }
                        

                        split_point_list.Clear();
                        
                        split_second.Clear();
                        calib_start_line = true;
                    }
                    else {
                        MessageBox.Show("Please select right or left!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                else {
                    MessageBox.Show("Detection side has not been selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                
            }
            else
            {

                MessageBox.Show("No detected user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            }
        }

        private void left_radio_CheckedChanged(object sender, EventArgs e)
        {
            clear_line_bt_Click(sender, e);
        }

        private void right_radio_CheckedChanged(object sender, EventArgs e)
        {
            clear_line_bt_Click(sender, e);
        }

        private void button_setting_save_Click(object sender, EventArgs e)
        {
            if (front_or_side_box.SelectedItem != null)
            {
                side_item = front_or_side_box.SelectedIndex;
                string example_image_path = "";
                if (side_item == 0||side_item==2)
                {
                    example_image_path = "C://Users//hpsin//QT_RS//WindowsFormsApp1//body_front.png";
                }
                if (left_radio.Checked || right_radio.Checked)
                {

                    if (left_radio.Checked)
                    {
                        checked_direct = "left";
                        if(side_item==1)
                            example_image_path = "C://Users//hpsin//QT_RS//WindowsFormsApp1//body_left.png";
                    }
                    else if (right_radio.Checked)
                    {
                        checked_direct = "right";
                        if(side_item==1)
                            example_image_path = "C://Users//hpsin//QT_RS//WindowsFormsApp1//body_right.png";
                    }
                    

                    split_point_list.Clear();
                    split_second.Clear();

                    Form2 form_setting = new Form2(example_image_path);
                    form_setting.StartPosition = FormStartPosition.CenterParent;
                    form_setting.ShowDialog();

                    //panel_setting.Visible = false;
                    clear_line_bt_Click(sender, e);
                    this.panel_process.Visible = true;
                    //this.panel_setting.Visible = false;
                    //this.calib_start.Visible = true;
                    this.label_panel_name.Text = "Processing Panel";
                    //this.label_panel_name.Visible = true;
                }
                else
                {
                    MessageBox.Show("Body side has not been selected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            else
            {
                MessageBox.Show("Measurement plane has not been selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                
            }
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            this.panel_process.Visible = false;
            this.panel_setting.Visible = true;
            clear_line_bt_Click(sender, e);
        }

        
    }


}
class Bone
{
    
	public Bone(JointType _from, JointType _to, Vector3 _direction)
    {
        from = _from;
        to = _to;
        direction = _direction;
    }

    public JointType from;
    public JointType to;
    public Vector3 direction;
    
};

enum JointType
{
    None = 0,
    Head = 1,
    Neck = 2,
    Torso = 3,
    Waist = 4,
    LeftCollar = 5,
    LeftShoulder = 6,
    LeftElbow = 7,
    LeftWrist = 8,
    LeftHand = 9,
    LeftFingertip = 10,
    RightCollar = 11,
    RightShoulder = 12,
    RightElbow = 13,
    RightWrist = 14,
    RightHand = 15,
    RightFingertip = 16,
    LeftHip = 17,
    LeftKnee = 18,
    LeftAnkle = 19,
    LeftFoot = 20,
    RightHip = 21,
    RightKnee = 22,
    RightAnkle = 23,
    RightFoot = 24
};
public class DirectBitmap : IDisposable
{
    public Bitmap Bitmap { get; private set; }

    public Int32[] Bits { get; private set; }
    public bool Disposed { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }

    protected GCHandle BitsHandle { get; private set; }

    public DirectBitmap(int width, int height)
    {
        Width = width;
        Height = height;
        Bits = new Int32[width * height];
        BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
        Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
    }

    public void SetPixel(int x, int y, Color colour)
    {
        int index = x + (y * Width);
        int col = colour.ToArgb();

        Bits[index] = col;
    }

    public Color GetPixel(int x, int y)
    {
        int index = x + (y * Width);
        int col = Bits[index];
        Color result = Color.FromArgb(col);

        return result;
    }

    public void Dispose()
    {
        if (Disposed)
            return;
        Disposed = true;
        Bitmap.Dispose();
        BitsHandle.Free();
    }
}


