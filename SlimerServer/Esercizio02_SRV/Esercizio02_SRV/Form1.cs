using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

using System.IO;
using System.Security.Policy;

namespace Esercizio02_SRV
{
    public delegate void aggiornaGraficaEventHandler(clsMessaggio Msg);

    public partial class Form1 : Form
    {

        private clsSocket serverSocket;
        string[] vFiles = new string[99]; // MAX 99 Files
        private string pathFiles;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int I;

            clsAddress.cercaIP();
            lstIndirizziIP.DataSource = clsAddress.ipVett;
            lstIndirizziIP.SelectedIndex = 1;

            cmbPorta.SelectedIndex = 1;

            lblStatoServer.Text = "STOPPED";

            pathFiles = Application.StartupPath + "\\FtpFiles\\";

            for (I = 0; I < vFiles.Length; I++)
                vFiles[I] = null;

        }

        private void btnAvvia_Click(object sender, EventArgs e)
        {
            IPAddress ip;
            bool errore = false;

            try
            {
                if (serverSocket == null)
                {
                    // Creo l'IP su cui attivare il Server
                    ip = clsAddress.ipVett[lstIndirizziIP.SelectedIndex];

                    // Creo il Server Socket
                    serverSocket = new clsSocket(true, Convert.ToInt32(cmbPorta.Text), ip);

                    // Aggiungo l'Evento datiRicevuti
                    serverSocket.datiRicevutiEvent += new datiRicevutiEventHandler(datiRicevuti);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ATTENZIONE: " + ex.Message);
                errore = true;
            }

            if (!errore)
            {
                // Avvio del Socket
                serverSocket.avviaServer();

                lstIndirizziIP.Enabled = false;
                cmbPorta.Enabled = false;
                btnAvvia.Enabled = false;
                btnArresta.Enabled = true;
                grpLog.Enabled = true;

                lblStatoServer.Text = "RUNNING";
            }

        }
        private void btnArresta_Click(object sender, EventArgs e)
        {
            // Arresto il Socket
            serverSocket.arrestaServer();

            lstIndirizziIP.Enabled = true;
            cmbPorta.Enabled = true;
            btnAvvia.Enabled = true;
            btnArresta.Enabled = false;
            grpLog.Enabled = false;

            lblStatoServer.Text = "STOPPED";

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Se il Socket Server è attivo => lo chido
            if (serverSocket != null)
                serverSocket.Dispose();

        }

        private void datiRicevuti(clsMessaggio Msg)
        {

            /*
             * "*TEST*" ==> test x la Connessione
             * "*SEND*" ==> invio Dati dal Client
             *              es. "*SEND*@nomeFile@dati"
             * "*_END*" ==> fine trasferimento File
             */

            string tipoRQ;
            string[] vDati;
            StreamWriter fileTXT;

            tipoRQ = Msg.messaggio.Substring(0, 6);

            switch (tipoRQ)
            {
                case "*TEST*":

                    // Gestisco TEST di Connessione
                    Msg.esito = "*OK";
                    break;

                case "*SEND*":

                    // Gestisco i Dati ricevuti dal Client

                    vDati = Msg.messaggio.Split('@');

                    // Nome del File
                    Msg.nomeFile = vDati[1];

                    // Controllo se il Nome File è già stato gestito
                    if (chkNomeFile(vDati[1]))
                    {
                        // Gestisco il Dato ricevuto dal Clinet
                        // Aggiungo al File sul Server precedentemente creato
                        fileTXT = new StreamWriter(pathFiles + vDati[1], true);
                        fileTXT.WriteLine(vDati[2]);
                        fileTXT.Close();
                        Msg.esito = "*RSP*@File aggiornato";
                    }
                    else
                    {
                        // Aggiungo il File nell'Elenco
                        // Creo il File sul Server
                        fileTXT = new StreamWriter(pathFiles + vDati[1]);
                        fileTXT.Close();
                        Msg.esito = "*RSP*@File creato con successo";
                    }

                    break;

                case "*_END*":

                    // Gestisco la Fine dei Dati dal Client

                    vDati = Msg.messaggio.Split('@');

                    // Nome del File
                    Msg.nomeFile = vDati[1];

                    // Rimuovo il Nome File dall' Elenco 
                    removeNomeFile(vDati[1]);

                    Msg.esito = "*RSP*@Fine File";

                    break;

                default:
                    Msg.esito = "ERR_TXRQ*";
                    break;
            }

            // Invio la Risposta (Messaggio) al Client
            serverSocket.inviaMsgSERVER(Msg.esito);

            // Visualizzo il Messaggio ricevuto dal Client nella Lista LOG
            aggiornaGraficaEventHandler pt = new aggiornaGraficaEventHandler(aggiornaGrafica);
            this.Invoke(pt, Msg);
        }

        private void aggiornaGrafica(clsMessaggio Msg)
        {
            lstLOG.Items.Add(Msg.ToString());
        }

        private bool chkNomeFile(string NomeFile)
        {
            bool esito = false;
            int I;

            // Scorro l'Elenco dei File "in Elaborazione"
            for (I = 0; I < vFiles.Length; I++)
                if (vFiles[I] == NomeFile)
                {
                    esito = true;
                    break;
                }

            if (!esito)
            {
                for(I=0; I< vFiles.Length; I++)
                    if (vFiles[I] == null)
                    {
                        vFiles[I] = NomeFile;
                        break;
                    }
            }

            return esito;
        }

        private void removeNomeFile(string NomeFile)
        {
            int I;

            // Scorro l'Elenco dei File "in Elaborazione"
            for (I = 0; I < vFiles.Length; I++)
                if (vFiles[I] == NomeFile)
                {
                    vFiles[I] = null;
                    break;
                }
        }

        private void btnPulisci_Click(object sender, EventArgs e)
        {
            lstLOG.Items.Clear();
        }
    }
}
