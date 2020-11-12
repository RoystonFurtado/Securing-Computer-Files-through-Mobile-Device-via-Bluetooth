using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using InTheHand.Net.Sockets;

namespace BlueConnect
{
    public partial class BlueLock : Form
    {
        String connBlueClientName;
        String nickName, passToUnlock;
        String[] selectedFiles,selectedFilenames;
        //String appDataPath, lpath;

        //Initializes all of the components on the Layout and sets on click events to these functions.
        public BlueLock()
        {
            InitializeComponent();
            //nickName = "Royston Furtado";
        }

        private void BlueLock_Load(object sender, EventArgs e)
        {

            txtSelectedFiles.Enabled = false;
            btnOFD.Enabled = false;
            btnLock.Enabled = false;
            //appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //path = Path.Combine(appDataPath, @"\Roaming\test\");
        }

        //FileDialog Opens to select files to lock
        private void btnOFD_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileLock = new OpenFileDialog();
            oFileLock.Filter = "All Files (*.*)|*.*";
            oFileLock.FilterIndex = 1;
            oFileLock.Multiselect = true;
            DialogResult result = oFileLock.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                selectedFiles = oFileLock.FileNames;
                for (int i = 0; i < selectedFiles.Length; i++)
                {
                    txtSelectedFiles.AppendText(selectedFiles[i]);
                }
            }
        }

        //When Lock is Clicked
        private void btnLock_Click(object sender, EventArgs e)
        {
            unlock();
            DirectoryInfo thisFolder = new DirectoryInfo("D:\\Secure");
            //If D:\\Secure does not exist create it
            if (!thisFolder.Exists)
            {
                thisFolder.Create();
            }
            DirectoryInfo thisNickFolder = new DirectoryInfo("D:\\Secure\\" + nickName + "\\");

            //All files to be locked moved to D:\\Secure\\Nickname
            if (thisNickFolder.Exists)
            {
                for (int i = 0; i < selectedFiles.Length; i++)
                {
                    String temp = Path.GetFileName(selectedFiles[i]);
                    File.Move(selectedFiles[i], "D:\\Secure\\" + nickName + "\\" + temp);
                }
                lockForSecure();
            }
            //If D:\\Secure\\Nickname does not exist create it
            else
            {
                thisNickFolder.Create();
                for (int i = 0; i < selectedFiles.Length; i++)
                {
                    String temp = Path.GetFileName(selectedFiles[i]);
                    File.Move(selectedFiles[i], "D:\\Secure\\" + nickName + "\\" + temp);

                }
                lockForSecure();
            }
            MessageBox.Show("Files Locked !! To unlock use mobile app");
        }
        public void lockForSecure()
        {
            //All files in D:\\Secure have their hidden and System attributes enabled to completely hide them.
            String[] allfiles = System.IO.Directory.GetFiles("D:\\Secure\\", "*.*", System.IO.SearchOption.AllDirectories);
            foreach (string file in allfiles)
            {
                ExecuteCommand("attrib +h +s " + file, 6000);
                //MessageBox.Show(file);
            }
            ExecuteCommand("ren D:\\Secure\\ \"Control Panel.{21EC2020-3AEA-1069-A2DD-08002B30309D}\"", 6000);
            ExecuteCommand("attrib +h +s D:\\\"Control Panel.{21EC2020-3AEA-1069-A2DD-08002B30309D}\"", 6000);
        }
        public void unlock()
        {
            //Unhide the files and reset attributes
            ExecuteCommand("attrib -h -s D:\\\"Control Panel.{21EC2020-3AEA-1069-A2DD-08002B30309D}\"", 6000);
            ExecuteCommand("ren D:\\\"Control Panel.{21EC2020-3AEA-1069-A2DD-08002B30309D}\" Secure", 6000);
            ExecuteCommand("attrib -h -s D:\\Secure\\" + nickName + "\\*.*", 6000);
        }
        private void btnBlueConnect_Click(object sender, EventArgs e)
        {
            connectAsServer();
            //nickName = "developer";
            //unlock();
            //transferUnlock();
            //temp();
        }
        public void connectAsServer()
        {
            Thread BluetoothServerThread = new Thread(new ThreadStart(ServerConnectThread));
            BluetoothServerThread.Start();
        }

        Guid mUUID = new Guid("00001101-0000-1000-8000-00805F9B34F8");
        bool serverStarted = false;
        public void ServerConnectThread()
        {
            //Creates server connection thread to establish connection.Start Bluetooth listener to listen for devices
            serverStarted = true;
            updateUI("Server Started !! Waiting for client .....");
            BluetoothListener blueListen = new BluetoothListener(mUUID);
            blueListen.Start();
            BluetoothClient conn = blueListen.AcceptBluetoothClient();
            connBlueClientName = conn.RemoteMachineName;
            nickName = conn.RemoteMachineName;
            updateUI("Client has Connected");
            updateUI("Connected with "+connBlueClientName);
            //Once connection is established enable the Select Files and Lock buttons
            if (this.btnLock.InvokeRequired)
            {

                this.btnLock.BeginInvoke((MethodInvoker)delegate () { this.btnLock.Enabled = true; ; });
                this.btnOFD.BeginInvoke((MethodInvoker)delegate () { this.btnOFD.Enabled = true; ; });
                this.txtSelectedFiles.BeginInvoke((MethodInvoker)delegate () { this.txtSelectedFiles.Enabled = true; ; });
            }
            Stream mStream = conn.GetStream();
            while (true)
            {
                try
                {
                    //handle Server Connection
                    byte[] received = new byte[1024];
                    //Reads the data coming from mobile device through BlueLock App
                    mStream.Read(received, 0, received.Length);
                    String receivedMessage = Encoding.ASCII.GetString(received);
                    //byte[] sent = Encoding.ASCII.GetBytes("Locking");
                    //Unlock command received from mobile app
                    if (receivedMessage.Contains("Unlock"))
                    {
                        transferUnlock();
                        MessageBox.Show("Files Unlocked \n Path = C:\\BlueLock\\"+nickName);
                        //unlockPath
                    }
                    else if (receivedMessage.Contains("Lock"))
                    {
                        //mStream.Write(sent, 0, sent.Length);
                        lockFrmMob();
                        MessageBox.Show("Files locked !!");
                    }
                    else { }
                    updateUI("Received : " + receivedMessage);
                    //byte[] sent = Encoding.ASCII.GetBytes("Locking");
                    //mStream.Write(sent, 0, sent.Length);

                }
                catch (Exception e)
                {
                    updateUI("Client has disconnected !!");
                }
            }

        }
        private void updateUI(String message)
        {
            Func<int> del = delegate ()
            {
                txtOut.AppendText(message + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }
        /*public void enableControls()
        {
            txtSelectedFiles.Enabled = true;
            btnOFD.Enabled = true;
            btnLock.Enabled = true;
        }*/
        public static int ExecuteCommand(string commnd, int timeout)
        {
            var pp = new ProcessStartInfo("cmd.exe", "/C" + commnd)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = "D:\\",
            };
            var process = Process.Start(pp);
            process.WaitForExit(timeout);
            process.Close();
            return 0;
        }

        //Lock from mobile device
        public void lockFrmMob()
        {
            //String temp = Path.GetFileName(selectedFiles[i]);
            //MessageBox.Show(temp);
            //File.Move(selectedFiles[i], "D:\\Secure\\" + nickName + "\\" + temp);
            DirectoryInfo lNick = new DirectoryInfo(@"C:\BlueLock\" + nickName);
            //Locks all the files present in C:\BlueLock\nickname folder
            if (lNick.Exists)
            {
                string[] dirs = Directory.GetFiles(@"C:\BlueLock\" + nickName + "\\", "*.*");
                if(dirs==null)
                {
                    MessageBox.Show("No files have been unlocked.");
                }
                else
                {
                    unlock();
                    foreach (string dir in dirs)
                    {
                        String temp = Path.GetFileName(dir);
                        File.Move(dir, @"D:\Secure\" + nickName + "\\" + temp);
                    }
                    lockForSecure();
                }
            }
        }

        //Unlock from mobile device
        public void transferUnlock()
        {
            unlock();
            DirectoryInfo secNick = new DirectoryInfo(@"D:\Secure\"+nickName);
            {
                //Create C:\BlueLock\<NickName> Dir tree
                if(secNick.Exists)
                {
                    DirectoryInfo unlockPath = new DirectoryInfo(@"C:\BlueLock\");
                    DirectoryInfo uPNick = new DirectoryInfo(@"C:\BlueLock\" + nickName);
                    if (!(unlockPath.Exists))
                    {
                        unlockPath.Create();
                    }
                    if (!(uPNick.Exists))
                    {
                        uPNick.Create();
                    }
                    //Hidden Files for that mobile device is retrieved and moved to C:\BlueLock\Nickname to make it visible to the user
                    string[] dirs = Directory.GetFiles(@"D:\Secure\" + nickName + "\\", "*.*");
                    //Console.WriteLine("The number of files starting with c is {0}.", dirs.Length);
                    foreach (string dir in dirs)
                    {
                        String temp = Path.GetFileName(dir);
                        File.Move(dir, @"C:\BlueLock\" + nickName + "\\" + temp);
                    }
                    //Hidden Path(without files in it) is locked again so that hidden path still wont be visible on unlock
                    lockForSecure();
                }
                else
                {
                    MessageBox.Show("You are a New user . You Need to lock the files first");
                }
            }
            //DirectoryInfo thisNickFolder = new DirectoryInfo("D:\\Secure\\" + nickName + "\\");  
        }
    }
}
/*
 var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var path = Path.Combine(appDataPath, @"\Roaming\test\");
if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
 */
