﻿﻿﻿﻿﻿using Quasar.Server.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Quasar.Server.Models;
using System.Threading.Tasks;

namespace Quasar.Server.Forms
{
    public partial class FrmCertificate : Form
    {
        private X509Certificate2 _certificate;

        public FrmCertificate()
        {
            InitializeComponent();
        }

        private void SetCertificate(X509Certificate2 certificate)
        {
            _certificate = certificate;
            txtDetails.Text = _certificate.ToString(false);
            btnSave.Enabled = true;
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            // 禁用按钮以防止重复点击
            btnCreate.Enabled = false;
            btnCreate.Text = "生成中...";
            
            try
            {
                // 使用异步操作避免界面冻结
                var certificate = await Task.Run(() => CertificateHelper.CreateCertificateAuthority("TcServer CA", 4096));
                SetCertificate(certificate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"创建证书时出错:\n{ex.Message}", "创建错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复按钮状态
                btnCreate.Enabled = true;
                btnCreate.Text = "创建";
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // 禁用按钮以防止重复点击
            btnImport.Enabled = false;
            btnImport.Text = "导入中...";
            
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.FileName = "TcServer";
                    ofd.CheckFileExists = true;
                    ofd.Filter = "*.p12|*.p12";
                    ofd.Multiselect = false;
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog(this) == DialogResult.OK)
                    {
                        try
                        {
                            SetCertificate(new X509Certificate2(ofd.FileName, "", X509KeyStorageFlags.Exportable));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, $"导入证书时出错:\n{ex.Message}", "保存错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            finally
            {
                // 恢复按钮状态
                btnImport.Enabled = true;
                btnImport.Text = "浏览并导入";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 禁用按钮以防止重复点击
            btnSave.Enabled = false;
            btnSave.Text = "保存中...";
            
            try
            {
                if (_certificate == null)
                    throw new ArgumentNullException();

                if (!_certificate.HasPrivateKey)
                    throw new ArgumentException();

                File.WriteAllBytes(Settings.CertificatePath, _certificate.Export(X509ContentType.Pkcs12));

                MessageBox.Show(this,
                    "请立即备份证书。证书丢失将导致失去所有客户端！",
                    "证书备份", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string argument = "/select, \"" + Settings.CertificatePath + "\"";
                Process.Start("explorer.exe", argument);

                this.DialogResult = DialogResult.OK;
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show(this, "请先创建或导入证书。", "保存错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(this,
                    "导入的证书没有关联的私钥。请导入其他证书。",
                    "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(this,
                    "保存证书时出错，请确保您对Quasar目录具有写入权限。",
                    "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复按钮状态
                btnSave.Enabled = true;
                btnSave.Text = "保存";
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}