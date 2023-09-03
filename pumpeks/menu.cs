using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pumpeks
{
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }

        // Make window draggable
        protected override void WndProc(ref Message m)
        {

            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);

        }

        private bool _fileselected = false;

        private void GithubBtn(object sender, EventArgs e)
        {
            Process.Start("explorer", "https://github.com/wessly0");
        }


        private void button2_Click_1(object sender, EventArgs e)
        {

            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {

                string fileName = openFileDialog1.SafeFileName;
                button2.Text = fileName;
                _fileselected = true;
            }

        }

        private void pompalaStart(object sender, EventArgs e)
        {
            if (!_fileselected)
            {
                MessageBox.Show("Dosya seçmedin!");
                return;
            }

            int Boyut;
            string Birim;

            if (!int.TryParse(boyut.Text, out Boyut))
            {
                MessageBox.Show("boyut alanına sayı girmen gerekiyor.", "?", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Hesapla
            Birim = birim.Text;
            switch (Birim)
            {
                case "KB":
                    Boyut = Boyut * 1024;
                    break;
                case "MB":
                    Boyut = Boyut * 1024 * 1024;
                    break;
                case "GB":
                    Boyut = Boyut * 1024 * 1024 * 1024;
                    break;
                default:
                    MessageBox.Show("birim seçmemişsin ki.", "?", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
            }

            DialogResult onayla = MessageBox.Show($"Dosyayı {boyut.Text} {birim.Text} {(ustuneEkle.Checked ? "kadar uzatmak" : "olarak ayarlamak")} ister misin?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onayla == DialogResult.No)
            {
                return;
            }


            #region Dosyaya yaz

            var file = File.OpenWrite(openFileDialog1.FileName);
            var siz = file.Seek(0, SeekOrigin.End);

            // Üstüne ekle açıksa
            if (ustuneEkle.Checked)
            {
                byte[] bytes = new byte[Boyut];
                file.Write(bytes, 0, bytes.Length);
            }
            // Belirtilen boyut kadar olacak
            else
            {
               file.SetLength(Boyut);
            }


            file.Close();

            #endregion

            MessageBox.Show("İşlem tamamlandı. Githubuma göz atmayı unutma!", "Yazıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ustuneEkle_CheckedChanged(object sender, EventArgs e)
        {
            bool isc = ustuneEkle.Checked;
            if (isc)
            {
                MessageBox.Show("Üstüne ekle açıldığında, belirtilen boyut dosyanın üzerine eklenir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Üstüne ekle kapalı, dosyanın boyutu belirtilen boyut kadar olacak. Yani üstüne eklenmeyecek, örnek 25MB yazdıysanız dosya tam olarak 25MB olucaktır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
