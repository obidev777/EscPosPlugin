using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscPosPlugin
{
    public class EscPos
    {
        public const string eClear = "\u001b@";
        public const string eCentre = "\u001ba1";
        public const string eLeft = "\u001ba0";
        public const string eRight = "\u001ba2";
        public const string eDrawer = "\u001b@\u001bp\0.}";
        public const string eCut = "\u001bi\r\n";
        public const string eSmlText = "\u001b!\u0001";
        public const string eNmlText = "\u001b!\0";
        public const string eInit = "\u001b!\0\r\u001bc6\u0001\u001bR3\r\n";
        public const string eBigCharOn = "\u001b!8";
        public const string eBigCharOff = "\u001b!\0";

        private RawPrinterHelper prn = new RawPrinterHelper();

        private string PrinterName = "EPSON TM-T20 Receipt";

        public bool Opened() => prn.PrinterIsOpen;

        public void Open(string name)
        {
            prn.OpenPrint(PrinterName);
        }

        public void PrintHeader()
        {
            Print(eInit + eCentre + "My Shop");
            Print("Tel:0123 456 7890");
            Print("Web: www.????.com");
            Print("sales@????.com");
            Print("VAT Reg No:123 4567 89" + eLeft);
            PrintDashes();
        }

        public void PrintBody()
        {
            Print(eSmlText + "Tea                                          T1   1.30");

            PrintDashes();

            Print(eSmlText + "                                         Total:   1.30");

            Print("                                     Paid Card:   1.30");
        }

        public void PrintFooter()
        {
            Print(eCentre + "Thank You For Your Support!" + eLeft);
            Print(Constants.vbLf + Constants.vbLf + Constants.vbLf + Constants.vbLf + Constants.vbLf + eCut + eDrawer);
        }

        public void Print(string Line)
        {
            prn.SendStringToPrinter(PrinterName, Line + Constants.vbLf);
        }

        public void PrintDashes()
        {
            Print(eLeft + eNmlText + "-".PadRight(42, '-'));
        }

        public void EndPrint()
        {
            prn.ClosePrint();
        }

        private void bnExit_Click(object sender, EventArgs e)
        {
            prn.ClosePrint();
        }

    }
}
