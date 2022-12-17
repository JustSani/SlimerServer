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

        int numberOfPlayers = 0;
        string playerOne = "";
        string playerTwo = "";



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
             * "*READY*" ==> Incrementa il numero di giocatori pronti
             * "*ASKING*" ==> 
             * 
             */

            string tipoRQ;
            string[] vDati;
            StreamWriter fileTXT;

            tipoRQ = Msg.messaggio.Split('*')[1];
            switch (tipoRQ)
            {
                case "TEST":
                    // Gestisco TEST di Connessione
                    Msg.esito = "*CONN*@In attesa che tutti i giocatori siano pronti";
                    break;

                case "READY":
                    // Gestisco il giocatore pronto
                    numberOfPlayers++;
                    if(numberOfPlayers == 1)
                        playerOne = Msg.ip;
                    else
                        playerTwo = Msg.ip;

                    Msg.esito = "*READY*";
                    break;
                case "ASKING":
                    if(numberOfPlayers == 1)
                        Msg.esito = "*WAIT*@In attesa di 1 giocatore";
                    else { 
                        if(playerOne == Msg.ip)
                            Msg.esito = "*START*@Tutti i giocatori sono pronti&One";
                        else
                            Msg.esito = "*START*@Tutti i giocatori sono pronti&Two";
                    }
                    break;

                case "SEND":
                    // Gestisco i Dati ricevuti dal Client
                    Msg.esito = "*tks*";

                    break;

                case "NEWS":
                    // Gestisco i Dati ricevuti dal Client
                    Msg.esito = "*HERE*@";

                    break;

                case "_END":

                    
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
        

        private void btnPulisci_Click(object sender, EventArgs e)
        {
            lstLOG.Items.Clear();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            numberOfPlayers++;
        }
    }
}
