using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Speedy_Launcher
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer Player = new SoundPlayer();
        private SolidColorBrush Red = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        private SolidColorBrush Green = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        private bool VolumeOn = true;
        private Random RNG = new Random();
        private string Operations = "+-";
        private bool answered = false;
        private float result;
        private int t = 0;
        private int f = 0;
        private int p = 0;

        public MainWindow()
        {
            InitializeComponent();
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            AdditionT.BorderBrush = Green;
            SubstractionT.BorderBrush = Green;
            MultiplicationT.BorderBrush = Red;
            DividationT.BorderBrush = Red;
        }

        private void DifficultyButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string identifier = button.Uid.ToString();
            if (button.BorderBrush == Green && Operations.Length > 1 || button.BorderBrush == Red)
            {
                Operations = button.BorderBrush == Green ? Operations.Replace(button.Uid, "") : Operations + identifier;
                button.BorderBrush = button.BorderBrush == Red ? Green : Red;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void GameWindow_Back_Click(object sender, MouseButtonEventArgs e)
        {
            GameWindowB.Visibility = Visibility.Hidden;
            LauncherWindowB.Visibility = Visibility.Visible;
            t = 0;
            f = 0;
            p = 0;
        }

        private void SpeedyMat_PreviewLeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            LauncherWindowB.Visibility = Visibility.Hidden;
            GameWindowB.Visibility = Visibility.Visible;
            Game.Visibility = Visibility.Hidden;
            Settings.Visibility = Visibility.Visible;
        }

        private void SetVolume_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VolumeOn = !VolumeOn;
            VolumeImg.Data = VolumeOn == false ? Geometry.Parse("M379 895 l-145 -145 -105 0 c-68 0 -109 -4 -117 -12 -17 -17 -17 -429 0 -446 8 -8 50 -12 123 -12 l110 0 140 -140 c77 -77 145 -140 152 -140 6 0 19 7 27 16 14 14 16 73 16 500 0 316 -4 492 -10 505 -6 10 -18 19 -28 19 -9 0 -83 -65 -163 -145z M717 672 c-26 -29 -21 -42 35 -99 l52 -53 -52 -53 c-56 -57 -62 -75 -34 -100 29 -26 42 -21 99 35 l53 52 53 -52 c57 -56 75 -62 100 -34 26 29 21 42 -35 99 l-52 53 52 53 c56 57 62 75 34 100 -29 26 -42 21 -99 -35 l-53 -52 -53 52 c-57 56 -75 62 -100 34z") : Geometry.Parse("M379 895 l-145 -145 -105 0 c-68 0 -109 -4 -117 -12 -17 -17 -17 -429 0 -446 8 -8 50 -12 123 -12 l110 0 140 -140 c77 -77 145 -140 152 -140 6 0 19 7 27 16 14 14 16 73 16 500 0 316 -4 492 -10 505 -6 10 -18 19 -28 19 -9 0 -83 -65 -163 -145z M648 1024 c-13 -12 -9 -91 5 -101 6 -6 29 -13 50 -17 104 -19 216 -106 268 -207 77 -154 47 -341 -73 -462 -52 -52 -136 -96 -199 -104 -50 -7 -61 -21 -57 -75 l3 -43 67 1 c142 1 298 119 376 281 l37 78 0 145 0 145 -37 78 c-75 156 -226 273 -370 284 -34 3 -66 2 -70 -3z M652 758 c-6 -6 -12 -31 -12 -55 0 -39 3 -45 40 -68 51 -32 75 -69 75 -115 0 -46 -24 -83 -75 -115 -37 -23 -40 -29 -40 -69 0 -62 17 -73 78 -51 89 34 162 139 162 235 0 50 -30 126 -64 167 -50 58 -138 97 -164 71z");
        }

        private async void AnswerBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                AnswerBox.Visibility = Visibility.Hidden;
                Checkmark.Visibility = Visibility.Hidden;
                bool c = Convert.ToSingle(AnswerBox.Text) == result ? true : false;
                AnswerBox.Text = "";
                QuestionLabel.Content = c == true ? "Doğru cevap!" : "Yanlış cevap!";
                if (c == true)
                {
                    t++;
                    p++;
                }
                else
                {
                    f++;
                    if (f % 3 == 0 && p > 0)
                    {
                        p--;
                    }
                }
                StatT.Content = $"Doğru: {t}";
                StatF.Content = $"Yanlış: {f}";
                StatP.Content = $"Puan: {p}";
                if (VolumeOn == true)
                {
                    Player.SoundLocation = c == true ? Path.GetFullPath("SFX/Correct.wav") : Path.GetFullPath("SFX/Incorrect.wav");
                    Player.Play();
                }
                await Task.Delay(VolumeOn == true ? (c == true ? 5000 : 1100) : 1000);
                MessageBox.Visibility = Visibility.Hidden;
                QuestionLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Checkmark.Visibility = Visibility.Visible;
                AnswerBox.Visibility = Visibility.Visible;
                answered = true;
            };
        }

        private async void Checkmark_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            AnswerBox.Visibility = Visibility.Hidden;
            Checkmark.Visibility = Visibility.Hidden;
            QuestionLabel.Content = Convert.ToSingle(AnswerBox.Text) == result ? "Doğru cevap!" : "Yanlış cevap!";
            AnswerBox.Text = "";
            if (VolumeOn == true)
            {
                Player.SoundLocation = Convert.ToString(QuestionLabel.Content) == "Doğru cevap!" ? Path.GetFullPath("SFX/Correct.wav") : Path.GetFullPath("SFX/Incorrect.wav");
                Player.Play();
            }
            await Task.Delay(VolumeOn == true ? (Convert.ToString(QuestionLabel.Content) == "Doğru cevap!" ? 5000 : 1100) : 1000);
            MessageBox.Visibility = Visibility.Hidden;
            QuestionLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Checkmark.Visibility = Visibility.Visible;
            AnswerBox.Visibility = Visibility.Visible;
            answered = true;
        }

        private async void StartGame()
        {
            string operations = "";
            foreach (char val in Operations)
            {
                operations += val;
            }
            while (true)
            {
                char operation = operations[RNG.Next(0, operations.Length)];
                int n2 = (operation == '+' || operation == '-') ? RNG.Next(1, 100) : RNG.Next(1, 10);
                int n1 = (operation == '+' || operation == '-') ? RNG.Next(1, 100) : RNG.Next(1, 10) * n2;
                result = operation == '+' ? (n1 + n2) : operation == '-' ? (n1 - n2) : operation == '*' ? (n1 * n2) : (n1 / n2);
                
                QuestionLabel.Content = $"{n1}{operation}{n2} işleminin sonucu nedir?";
                MessageBox.Visibility = Visibility.Visible;
                AnswerBox.Focus();

                do
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(0.5));
                } while (answered != true);
                answered = false;
            }
        }

        private void LaunchSpeedyMat_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Settings.Visibility = Visibility.Hidden;
            MessageBox.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Visible;
            StartGame();
        }
    }
}
