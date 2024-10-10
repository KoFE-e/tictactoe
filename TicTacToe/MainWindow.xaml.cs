using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TicTacToe
{
    
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        bool isMatchCircle = false;
        bool isMatchCross = false;
        bool isFull = false;
        bool playWithPc;
        bool isPcFirst = false;
        bool queue;
        bool start;
        int crossScore = 0;
        int circleScore = 0;

        public MainWindow(bool playPc)
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            start = true;
            playWithPc = playPc;

            // SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0");
            New.Content = "Новая игра";
            Score.Text = "Крестики : " + crossScore + "\nНолики: " + circleScore;
        }

        public void BlockField()
        {
            foreach (Button b in Field.Children.OfType<Button>())
            {
                string bText = b.Content?.ToString();
                if (bText == emoji[0] || bText == emoji[1] || bText == null)
                {
                    b.IsEnabled = false;
                }
            }
        }

        private bool IsMatch(List<string> elem, string str)
        {
            for (int i = 0; i < elem.Count; i++)
            {
                if ((i == 0 || i == 1 || i == 2) && elem[i] == elem[i + 3] && elem[i] == elem[i + 6] && elem[i] == str)
                {
                    return true;
                }
                else if ((i == 0 || i == 3 || i == 6) && elem[i] == elem[i + 1] && elem[i] == elem[i + 2] &&
                         elem[i] == str)
                {
                    return true;
                }
                else if (i == 0 && elem[i] == elem[i + 4] && elem[i] == elem[i + 8] && elem[i] == str)
                {
                    return true;
                }
                else if (i == 2 && elem[i] == elem[i + 2] && elem[i] == elem[i + 4] && elem[i] == str)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetUpGame()
        {
            tenthsOfSecondsElapsed = 0;
            isMatchCircle = false;
            isMatchCross = false;
            isFull = false;

            foreach (Button b in Field.Children.OfType<Button>())
            {
                b.Content = null;
                b.IsEnabled = true;
            }

            queue = start;
            if (start)
            {
                Queue.Text = "Ход крестиков";
            }
            else
            {
                Queue.Text = "Ход ноликов";
            }

            start = !start;
            if (isPcFirst) ClickPc();
        }

        List<string> emoji = new List<string>()
            {
                "❌",
                "⭕",
            };

        private void Change_Emoji(Button button)
        {
            if (queue)
            {
                button.Content = emoji[0];
                button.IsEnabled = false;
                queue = false;
                Queue.Text = "Ход ноликов";
            }
            else if (!queue)
            {
                button.Content = emoji[1];
                button.IsEnabled = false;
                queue = true;
                Queue.Text = "Ход крестиков";
            }
        }

        private void Is_Win()
        {
            if (isMatchCross)
            {
                if (playWithPc) isPcFirst = !isPcFirst;
                timer.Stop();
                Queue.Text = "Крестики победили!";
    
                BlockField();

                crossScore++;
            }
            else if (isMatchCircle)
            {
                if (playWithPc) isPcFirst = !isPcFirst;
                timer.Stop();
                Queue.Text = "Нолики победили!";

                BlockField();

                circleScore++;
            }
            else if (isFull)
            {
                if (playWithPc) isPcFirst = !isPcFirst;
                timer.Stop();
                Queue.Text = "Ничья!";

                BlockField();

                circleScore++;
                crossScore++;
            }
        }

        private void Play_Click(Button button)
        {
            if(button.Content == null)
            {
                Change_Emoji(button);

                List<string> elem = new List<string>();
                foreach (Button b in Field.Children.OfType<Button>())
                {
                    string bText = b.Content?.ToString();
                    if (bText == emoji[0] || bText == emoji[1] || bText == null)
                    {
                        elem.Add(bText);
                    }
                }

                isMatchCircle = IsMatch(elem, emoji[1]);
                isMatchCross = IsMatch(elem, emoji[0]);
                
                if (!elem.Contains(null))
                {
                    isFull = true;
                }
            }

            Is_Win();
        }
        private void ClickPc()
        {
            IEnumerable<Button> ButtonList = Field.Children.OfType<Button>();
            List<Button> AvailableButtons = ButtonList.Where(b => b.IsEnabled).ToList();
            Random rnd = new Random();
            if (AvailableButtons.Count() > 0)
            {
                int index = rnd.Next(0, AvailableButtons.Count());
                Button turn = AvailableButtons[index];
                Play_Click(turn);
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Console.WriteLine(isPcFirst);
            if (playWithPc)
            {
                Play_Click(button);
                ClickPc();
            }
            else Play_Click(button);
        }

        private void Reset_Game(object sender, RoutedEventArgs e)
        {
            Score.Text = "Счет";
            Queue.Text = "Очередь";
            timeTextBlock.Text = "Таймер";
            start = true;
            crossScore = 0;
            circleScore = 0;
            BlockField();
            timer.Stop();
            if (playWithPc) isPcFirst = false;
        }

        private void New_game(object sender, RoutedEventArgs e)
        {
            timer.Start();
            SetUpGame();
        }

        private void Closing_Window(object sender, CancelEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
        }
    }
}