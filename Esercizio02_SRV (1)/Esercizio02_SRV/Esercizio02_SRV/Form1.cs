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
        clsSocket clientSocket;
        List<string> PlayerAddress = new List<string>();
        int playersReady = 0;
        int playersJoined = 0;
        IPAddress ipServer;
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
             * "*join*" ==> Player JOINED
             * "*REAY*" ==> Il giocatore è pronto
             *              es. "*SEND*@nomeFile@dati"
             * "*QUIT*" ==> Il giocatore sta quittando
             * "*_END*" ==> fine trasferimento File
             */

            string tipoRQ;
            string[] vDati;
            StreamWriter fileTXT;

            tipoRQ = Msg.messaggio.Substring(0, 6);

            switch (tipoRQ)
            {
                case "*JOIN*":
                    // Incremento il numero di giocatori che sono entrati
                    playersJoined++;

                    // Gestisco TEST di Connessione
                    Msg.esito = "*CONNECTED";

                    //PUSH DELL IP NELLA LISTA DEI GIOCATORI
                    PlayerAddress.Add(Msg.ip);
                    break;

                case "*REAY*":
                    // INCREMENTO IL NUM DI GIOCATORI PRONTI
                    playersReady++;

                    
                    Msg.esito = "*WAITING";
                    
                    break;
                case "*QUIT*":

                    // GIOCATORI ENTRATI DIMINUISCONO
                    playersJoined--;
                    int index = PlayerAddress.FindIndex(a => a.Contains(Msg.ip));
                    PlayerAddress.RemoveAt(index);

                    Msg.esito = "*OK";
                    
                    break;

                case "*SEND*":

                    // Gestisco i Dati ricevuti dal Client

                    vDati = Msg.messaggio.Split('@');

                    
                    Msg.esito = "*Ok*";
                    foreach(string addr in PlayerAddress)
                    {
                        //INVIO AL ALTRO GIOCATORE IL MOVIMENTO DI QUESTO CATTIVO RAGAZZO
                        if(addr != Msg.ip)
                        {
                            try
                            {
                                ipServer = clsAddress.cercaIP(addr);
                            }
                            catch (Exception ex)
                            {
                                //inputField.Focus();
                                ipServer = null;
                            }

                            if (ipServer != null)
                            {
                                // provo a Connettermi al SERVER

                                try
                                {
                                    aggiornaGraficaEventHandler pt1 = new aggiornaGraficaEventHandler(aggiornaGrafica);
                                    clsMessaggio MessaggioServer = new clsMessaggio();
                                    MessaggioServer.ip = "0.0.0.0";
                                    MessaggioServer.porta = 8888;
                                    MessaggioServer.messaggio = "*MOVE*";
                                    MessaggioServer.esito = "*Ok";
                                    this.Invoke(pt1, MessaggioServer);
                                    inviaDatiServer("*MOVE*");


                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ATTENZIONE Errore nella connessione all PC: " + ex.Message);
                                }

                            }
                        }
                    }
                    

                    break;

                case "*_END*":

                    // Gestisco la Fine dei Dati dal Client

                    vDati = Msg.messaggio.Split('@');


                    // Rimuovo il Nome File dall' Elenco 
                    removeNomeFile(vDati[1]);

                    Msg.esito = "*RSP*@Fine File";

                    break;

                default:
                        Msg.esito = "ERR_TXRQ";
                    break;
            }

            // Invio la Risposta (Messaggio) al Client
            serverSocket.inviaMsgSERVER(Msg.esito);

            // Visualizzo il Messaggio ricevuto dal Client nella Lista LOG
            aggiornaGraficaEventHandler pt = new aggiornaGraficaEventHandler(aggiornaGrafica);
            this.Invoke(pt, Msg);

            // SE TUTTI I GIOCATORI SONO PRONTI E SONO >= 2 ALLORA INVIO A TUTTI L'INIZIO DELLA PARTITA
            if (playersReady == playersJoined && playersJoined >= 2)
            {
                foreach(string addr in PlayerAddress) { 
                    try
                    {
                        ipServer = clsAddress.cercaIP(addr);
                    }
                    catch (Exception ex)
                    {
                        //inputField.Focus();
                        ipServer = null;
                    }

                    if (ipServer != null)
                    {
                        // provo a Connettermi al SERVER

                        try
                        {
                            aggiornaGraficaEventHandler pt1 = new aggiornaGraficaEventHandler(aggiornaGrafica);
                            clsMessaggio MessaggioServer = new clsMessaggio();
                            MessaggioServer.ip = "0.0.0.0";
                            MessaggioServer.porta = 8888;
                            MessaggioServer.messaggio = "*STAR";
                            MessaggioServer.esito = "*EXPECTED TKS";
                            this.Invoke(pt1, MessaggioServer);
                            inviaDatiServer("*STAR*");


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ATTENZIONE: " + ex.Message);
                        }

                    }
                }
            }

        }

        public void inviaDatiServer(string strIN)
        {
            // Instanzio il Client Socket
            clientSocket = new clsSocket(false, Convert.ToInt16(6969), ipServer);

            // Invio il Messaggio al Server
            clientSocket.inviaMsgCLIENT(strIN);

            // Aspetto il Messaggio di Risposta del Server
            clsMessaggio msgByServer = clientSocket.clientRicevi();

            // Aggiungo alla Lista la Risposta del Server
            aggiornaGraficaEventHandler pt = new aggiornaGraficaEventHandler(aggiornaGrafica);
            this.Invoke(pt, msgByServer);

            // Chiudo il Socket
            clientSocket.Dispose();

        }


        private void aggiornaGrafica(clsMessaggio Msg)
        {
            lstLOG.Items.Add(Msg.ToString());
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
