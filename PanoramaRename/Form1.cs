using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PanoramaRename {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        static bool _isProcess = false;
        static string _inputDir = "";
        static string _outputDir = "";
        static string _imageName = "";
        static int _pollingTime = 2000;
        static int _minfilesize = 1000;

        // start
        private void button1_Click(object sender, EventArgs e) {
            _isProcess = true;
            ToggleTextBox(false);
            button1.Enabled = false;
            pictureBox1.Visible = true;

            _inputDir = textBox1.Text;
            _outputDir = textBox2.Text + "\\";
            _imageName = textBox4.Text;
            if (!int.TryParse(textBox3.Text, out _pollingTime)) {
                _pollingTime = 2000;
            }
            if (_pollingTime <= 0) {
                _pollingTime = 2000;
            }
            if (!int.TryParse(textBox5.Text, out _minfilesize)) {
                _minfilesize = 1000;
            }
            if (_minfilesize <= 0) {
                _minfilesize = 1000;
            }

            // ポーリング開始
            Task.Run(() => Polling());
        }

        // stop
        private void button2_Click(object sender, EventArgs e) {
            _isProcess = false;
            ToggleTextBox(true);
            pictureBox1.Visible = true;
            button1.Enabled = true;
        }

        // close
        private void button3_Click(object sender, EventArgs e) {
            this.Close();
        }

        // 全テキストボックス変更
        void ToggleTextBox(bool toggle) {
            textBox1.Enabled = toggle;
            textBox2.Enabled = toggle;
            textBox3.Enabled = toggle;
            textBox4.Enabled = toggle;
            textBox5.Enabled = toggle;
            if (toggle) {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        //
        static void Polling() {

            try {
                // Photoshop側でバッチ処理対象のファイル名一覧を得る
                string[] inputFiles = System.IO.Directory.GetFiles(_inputDir, "*", System.IO.SearchOption.TopDirectoryOnly);
                //Array.Sort(inputFiles, (x, y) => DateTime.Compare(System.IO.File.GetLastWriteTime(x), System.IO.File.GetLastWriteTime(y)));

                string findname = _outputDir + _imageName;  // 検索対象のファイル名

                // 永久ループ
                int count = 0;
                Console.WriteLine("Loop...");
                while (_isProcess) {
                    System.Threading.Thread.Sleep(_pollingTime);

                    // ファイルあった？
                    if (System.IO.File.Exists(findname)) {
                        System.IO.FileInfo fi = new System.IO.FileInfo(findname);
                        long filesize = fi.Length;
                        if (filesize > _minfilesize * 1000) {   // ファイルサイズが指定したサイズKB以上だけ処理する
                            // リネーム
                            string dst = _outputDir + System.IO.Path.GetFileName(inputFiles[count]);
                            System.IO.File.Move(findname, dst);

                            Console.WriteLine("Renamed: " + dst);
                            //Console.WriteLine("Next file name: " + dst);

                            count++;
                        }
                    }
                }
            } catch {
                MessageBox.Show("ERROR: Stop processes.");
            } finally {
                // 後始末。UI復活
                Program.GetInstance().Invoke((Action)(() => {
                    Program.GetInstance().ToggleTextBox(true);
                    Program.GetInstance().pictureBox1.Visible = false;
                }));
            }
        }
    }
}
