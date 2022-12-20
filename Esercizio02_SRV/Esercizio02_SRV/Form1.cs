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
        bool playerOnePendingAttack;
        bool playerTwoPendingAttack;
        string playerOnePendingKilled;
        string playerTwoPendingKilled;
        string[] playerOneVector = new string[2]; //[0] --> x
        string[] playerTwoVector = new string[2]; //[1] --> y



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

            playerOneVector[0] = "0";
            playerOneVector[1] = "0";
            playerTwoVector[0] = "0";
            playerTwoVector[1] = "0";
            numberOfPlayers = 0;
            playerOne = "";
            playerTwo = "";
            playerOnePendingAttack = false;
            playerTwoPendingAttack = false;
            playerOnePendingKilled = "";
            playerTwoPendingKilled = "";

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


                    //MESSAGGIO DI ATTACCO
                    if (Msg.messaggio.Contains("KILLED"))
                    {
                        if (Msg.ip == playerOne)
                            playerOnePendingKilled = Msg.messaggio.Split('-')[1];
                        else
                            playerTwoPendingKilled = Msg.messaggio.Split('-')[1];
                    }
                    if (Msg.messaggio.Contains("FIRE"))
                    {
                        if (Msg.ip == playerOne)
                            playerOnePendingAttack = true;
                        else
                            playerTwoPendingAttack = true;
                    }//MESSAGGIO DI MOVE
                    else { 
                        if(Msg.ip == playerOne)
                        {
                            playerOneVector[0] = Msg.messaggio.Split(':')[1].Split('#')[0];
                            playerOneVector[1] = Msg.messaggio.Split(':')[2];
                            //MessageBox.Show("Your cords: " + playerOneVector[0] + ", " + playerOneVector[1]);
                        }
                        else
                        {
                            playerTwoVector[0] = Msg.messaggio.Split(':')[1].Split('#')[0];
                            playerTwoVector[1] = Msg.messaggio.Split(':')[2];
                            //MessageBox.Show("Your cords: " + Msg.messaggio);
                            //MessageBox.Show("Your cords: " + playerTwoVector[0] + ", " + playerTwoVector[1]);
                        }
                    }

                    break;

                case "NEWS":
                    // Rispondo con le coordinate
                    if(playerOne == Msg.ip) {
                        if (playerTwoPendingAttack && playerTwoPendingKilled != "")
                        {
                            Msg.esito = "*HERE*FIRE&KILLED-" + playerTwoPendingKilled + "-";
                            playerTwoPendingAttack = false;
                            playerTwoPendingKilled = "";
                        }
                        else if (playerTwoPendingAttack)
                        {
                            Msg.esito = "*HERE*FIRE";
                            playerTwoPendingAttack = false;
                        }
                        else 
                            Msg.esito = "*HERE*@p2Position=X:" + playerTwoVector[0].ToString() + "#Y:" + playerTwoVector[1].ToString();
                    }
                    else
                    {
                        if (playerOnePendingAttack && playerOnePendingKilled != "")
                        {
                            Msg.esito = "*HERE*FIRE&KILLED-" + playerOnePendingKilled + "-";
                            playerOnePendingAttack = false;
                            playerOnePendingKilled = "";
                        }
                        else if (playerOnePendingAttack)
                        {
                            Msg.esito = "*HERE*FIRE";
                            playerOnePendingAttack = false;
                        }
                        else
                            Msg.esito = "*HERE*@p1Position=X:" + playerOneVector[0].ToString() + "#Y:" + playerOneVector[1].ToString();
                    }
                    
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
