using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MiniPuzzle {

    public partial class MainWindow : Window {

        DispatcherTimer _tmr = new DispatcherTimer();
        bool bChrono = false;
        int iTime = 0;
        int iClick = 0;

        ReductString _strGame = new ReductString(true, 0, 9, "...");
        string strWin;
        string strActual;

        public MainWindow() {
            InitializeComponent();

            _tmr.Interval = TimeSpan.FromSeconds(1);
            _tmr.Tick += new EventHandler(tmr_Tick);
            _tmr.Start();

            beginGame(3);
        }

        private void beginGame(int iMode) {
            grdGame.RowDefinitions.Clear();
            grdGame.ColumnDefinitions.Clear();
            grdGame.Children.Clear();
            strWin = null;
            iTime = 0;
            iClick = 0;
            cbxABC123.CanSwitch = true;

            int iR = 0;
            Random r = new Random();
            List<int> lR = new List<int>();

            for (int i = 1; i < Math.Pow(iMode, 2); i++) strWin += i;
            strWin += "#";

            for (int y = 0; y < iMode; y++) {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(1, GridUnitType.Star);
                grdGame.RowDefinitions.Add(rd);

                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(1, GridUnitType.Star);
                grdGame.ColumnDefinitions.Add(cd);

                for (int x = 0; x < iMode; x++) {
                    do iR = r.Next(0, (int)Math.Pow(iMode, 2));
                    while (lR.Contains(iR));
                    lR.Add(iR);

                    Label lbl = new Label();
                    if (iR > 0) {
                        if (cbxABC123.On) lbl = CreateLabel(NumberLetter(iR.ToString(), false), iR.ToString(), "lblStyleGame");
                        else lbl = CreateLabel(iR.ToString(), iR.ToString(), "lblStyleGame");
                    } else lbl = CreateLabel("#", "#", "lblStyleEmpty");

                    Grid.SetColumn(lbl, x);
                    Grid.SetRow(lbl, y);

                    grdGame.Children.Add(lbl);
                }
            }
            bChrono = true;
            TestPositionOrWin();
        }

        private void lbl_MouseDown(object sender, EventArgs e) {
            Label lbl = (Label)sender;

            foreach (Label l in grdGame.Children) {
                int iXE = Grid.GetColumn(l);
                int iYE = Grid.GetRow(l);

                if (l.Uid.Equals("#")) {
                    int iX = Grid.GetColumn(lbl);
                    int iY = Grid.GetRow(lbl);

                    if ((Math.Abs(iYE - iY) <= 1 && Math.Abs(iXE - iX) <= 0) || (Math.Abs(iYE - iY) <= 0 && Math.Abs(iXE - iX) <= 1)) {
                        Grid.SetColumn(lbl, iXE);
                        Grid.SetRow(lbl, iYE);
                        Grid.SetColumn(l, iX);
                        Grid.SetRow(l, iY);
                        iClick++;
                    }
                }
            }

            TestPositionOrWin();
        }

        private void TestPositionOrWin() {
            strActual = null;
            _strGame.OriginalText = null;

            if (iClick > 0) {
                if (iClick > 1) lblClicks.Content = iClick + " Clics";
                else lblClicks.Content = iClick + " Clic";
            }

            for (int y = 0; y < grdGame.RowDefinitions.Count; y++) {
                for (int x = 0; x < grdGame.ColumnDefinitions.Count; x++) {
                    Label l = (Label)(grdGame.Children.Cast<UIElement>().FirstOrDefault(a => Grid.GetColumn(a) == x && Grid.GetRow(a) == y)); // RECUPERE LES POSITION DE CHAQUE CELLULES

                    _strGame.OriginalText += l.Content.ToString();
                    strActual += l.Uid;

                    if (strActual.Equals(strWin)) {
                        cbxABC123.CanSwitch = false;
                        bChrono = false;

                        grdGame.Children.Clear();
                        grdGame.ColumnDefinitions.Clear();
                        grdGame.RowDefinitions.Clear();

                        l = CreateLabel("Vous avez gagné", "#", "lblStyleEnd");
                        grdGame.Children.Add(l);

                        l = CreateLabel("Terminé en " + TimeSpan.FromSeconds(iTime).Minutes + " minutes et " + TimeSpan.FromSeconds(iTime).Seconds + " secondes avec " + iClick + " clics\nSéléctionnez un mode de jeu pour rejouer !", "#", "lblStyleEndDesc");
                        grdGame.Children.Add(l);
                    }
                }
            }

            if (_strGame.Reducted) lblGame.Content = _strGame.Reduct();
            else lblGame.Content = _strGame.Unreduct();
        }

        private Label CreateLabel(string strContent, string strUID, string strStyle) {
            Label lbl = new Label();

            lbl.Style = FindResource(strStyle) as Style;
            lbl.Uid = strUID;
            lbl.Content = strContent;

            return lbl;
        }

        private void cbxABC123_MouseDown(object sender, MouseButtonEventArgs e) {
            if (cbxABC123.CanSwitch) {
                foreach (Label lbl in grdGame.Children) {
                    if (cbxABC123.On) lbl.Content = NumberLetter(lbl.Content.ToString(), false);   // NOMBRE -> LETTRES
                    else lbl.Content = NumberLetter(lbl.Content.ToString(), true);                 // LETTRES -> NOMBRE
                }
                TestPositionOrWin();
            }
        }

        private string NumberLetter(string str, bool bNumber) {
            if (!str.Equals("#")) {
                if (bNumber) return (char.ToUpper(str[0]) - 'A' + 1).ToString();
                else return ((char)(64 + Convert.ToInt32(str))).ToString();
            } else { return str; }
        }

        private void tmr_Tick(object sender, EventArgs e) {
            lblHour.Content = DateTime.Now.ToString("HH:mm:ss");
            if (bChrono) {
                lblTime.Content = "Temps: " + TimeSpan.FromSeconds(iTime).ToString(@"mm\:ss");
                iTime++;
            }
        }

        private void btnMode_Click(object sender, RoutedEventArgs e) {
            beginGame(Convert.ToInt32(((Button)sender).Uid));
        }

        private void lblGame_MouseDown(object sender, MouseButtonEventArgs e) {
            if (_strGame.Reducted) lblGame.Content = _strGame.Unreduct();
            else lblGame.Content = _strGame.Reduct();
        }
    }
}