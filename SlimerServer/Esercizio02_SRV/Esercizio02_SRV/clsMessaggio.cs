using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercizio02_SRV
{
    public class clsMessaggio
    {

        public string ip;
        public UInt16 porta;
        public string messaggio;
        public string esito;

        public override string ToString()
        {
            // Ritorno del Metodo ToString() base
            // return base.ToString();

            return this.ip + " : " +
                this.porta.ToString() + " - " +
                this.messaggio +
                " ==> " +
                this.esito;
        }

    }
}
