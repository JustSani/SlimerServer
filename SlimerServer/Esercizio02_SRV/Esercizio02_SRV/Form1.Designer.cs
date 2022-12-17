namespace Esercizio02_SRV
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnArresta = new System.Windows.Forms.Button();
            this.btnAvvia = new System.Windows.Forms.Button();
            this.lblStatoServer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPorta = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstIndirizziIP = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.lstLOG = new System.Windows.Forms.ListBox();
            this.btnPulisci = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnArresta);
            this.groupBox1.Controls.Add(this.btnAvvia);
            this.groupBox1.Controls.Add(this.lblStatoServer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbPorta);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lstIndirizziIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 177);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametri SERVER";
            // 
            // btnArresta
            // 
            this.btnArresta.BackColor = System.Drawing.Color.LightCoral;
            this.btnArresta.Enabled = false;
            this.btnArresta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArresta.ForeColor = System.Drawing.Color.Blue;
            this.btnArresta.Location = new System.Drawing.Point(565, 37);
            this.btnArresta.Name = "btnArresta";
            this.btnArresta.Size = new System.Drawing.Size(176, 61);
            this.btnArresta.TabIndex = 7;
            this.btnArresta.Text = "A R R E S T A";
            this.btnArresta.UseVisualStyleBackColor = false;
            this.btnArresta.Click += new System.EventHandler(this.btnArresta_Click);
            // 
            // btnAvvia
            // 
            this.btnAvvia.BackColor = System.Drawing.Color.LightGreen;
            this.btnAvvia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvvia.ForeColor = System.Drawing.Color.Blue;
            this.btnAvvia.Location = new System.Drawing.Point(324, 37);
            this.btnAvvia.Name = "btnAvvia";
            this.btnAvvia.Size = new System.Drawing.Size(176, 61);
            this.btnAvvia.TabIndex = 6;
            this.btnAvvia.Text = "A V V I A";
            this.btnAvvia.UseVisualStyleBackColor = false;
            this.btnAvvia.Click += new System.EventHandler(this.btnAvvia_Click);
            // 
            // lblStatoServer
            // 
            this.lblStatoServer.AutoSize = true;
            this.lblStatoServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatoServer.ForeColor = System.Drawing.Color.Blue;
            this.lblStatoServer.Location = new System.Drawing.Point(186, 105);
            this.lblStatoServer.Name = "lblStatoServer";
            this.lblStatoServer.Size = new System.Drawing.Size(0, 17);
            this.lblStatoServer.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(186, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Stato Server:";
            // 
            // cmbPorta
            // 
            this.cmbPorta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPorta.FormattingEnabled = true;
            this.cmbPorta.Items.AddRange(new object[] {
            "8080",
            "8888",
            "9090",
            "9999"});
            this.cmbPorta.Location = new System.Drawing.Point(189, 37);
            this.cmbPorta.Name = "cmbPorta";
            this.cmbPorta.Size = new System.Drawing.Size(74, 21);
            this.cmbPorta.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Porta:";
            // 
            // lstIndirizziIP
            // 
            this.lstIndirizziIP.FormattingEnabled = true;
            this.lstIndirizziIP.Location = new System.Drawing.Point(29, 37);
            this.lstIndirizziIP.Name = "lstIndirizziIP";
            this.lstIndirizziIP.Size = new System.Drawing.Size(120, 121);
            this.lstIndirizziIP.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Indirizzi IP:";
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.lstLOG);
            this.grpLog.Controls.Add(this.btnPulisci);
            this.grpLog.Enabled = false;
            this.grpLog.Location = new System.Drawing.Point(12, 195);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(776, 442);
            this.grpLog.TabIndex = 1;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "Log Operazioni SERVER";
            // 
            // lstLOG
            // 
            this.lstLOG.FormattingEnabled = true;
            this.lstLOG.Location = new System.Drawing.Point(6, 44);
            this.lstLOG.Name = "lstLOG";
            this.lstLOG.ScrollAlwaysVisible = true;
            this.lstLOG.Size = new System.Drawing.Size(764, 381);
            this.lstLOG.TabIndex = 1;
            // 
            // btnPulisci
            // 
            this.btnPulisci.Location = new System.Drawing.Point(695, 19);
            this.btnPulisci.Name = "btnPulisci";
            this.btnPulisci.Size = new System.Drawing.Size(75, 19);
            this.btnPulisci.TabIndex = 0;
            this.btnPulisci.Text = "Pulisci";
            this.btnPulisci.UseVisualStyleBackColor = true;
            this.btnPulisci.Click += new System.EventHandler(this.btnPulisci_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 645);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "FTP - CLIENT  - SERVER";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnArresta;
        private System.Windows.Forms.Button btnAvvia;
        private System.Windows.Forms.Label lblStatoServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbPorta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstIndirizziIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.ListBox lstLOG;
        private System.Windows.Forms.Button btnPulisci;
    }
}

