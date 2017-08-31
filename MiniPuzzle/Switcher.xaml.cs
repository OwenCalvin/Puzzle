using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MiniPuzzle {
    public partial class Switcher : Border {
        private bool bCanSwitch = true;
        public bool CanSwitch {
            get { return bCanSwitch; }
            set {
                if (value) {
                    Ellipse.Fill = Brushes.White;
                    Cursor = Cursors.Hand;
                }
                else {
                    Ellipse.Fill = Brushes.Transparent;
                    Cursor = Cursors.Arrow;
                }
                bCanSwitch = value;
            }
        }

        private bool bOn = false;
        public bool On {
            get { return bOn; }
            set {
                if (value) {
                    Background = new SolidColorBrush(Color.FromRgb(30, 215, 96));
                    Grid.SetColumn(Ellipse, 1);
                } else {
                    Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                    Grid.SetColumn(Ellipse, 0);
                }
                bOn = value;
            }
        }

        public Switcher() {
            InitializeComponent();
        }
        private void this_MouseDown(object sender, MouseButtonEventArgs e) {
            if (CanSwitch) On = !On;
        }
    }
}
