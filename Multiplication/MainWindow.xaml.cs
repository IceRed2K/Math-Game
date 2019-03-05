using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Multiplication
{
    public partial class MainWindow
    {
        #region Fields

        private bool _activeTimer;
        private bool _generateNumber;
        private int _result;
        private Thread _t;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            CounterLabel.Content = "Hello " + Environment.UserName;
        }

        public void Btn_Click(object o, RoutedEventArgs routedEventArgs)
        {
            var sender = o as Button;
            switch (sender?.Content)
            {
                case "Start":
                    PlusLable.Content = "+";
                    GameMethods(1);
                    sender.Content = "Stop";
                    break;
                case "Stop":
                    GameMethods(2);
                    sender.Content = "Start";
                    break;
            }
        }
        
        private void GenerateNums()
        {
            int x = new Random(DateTime.Now.Millisecond).Next(0, 100);
            Thread.Sleep(1);
            int y = new Random(DateTime.Now.Millisecond).Next(0, 100);
            ChangeText(Val1, x.ToString());
            ChangeText(Val2, y.ToString());
            _result = x + y; // Replace to * //
        }

        private void Text_Update(object sender, TextChangedEventArgs args)
        {
            var tb = sender as TextBox;
            Int32.TryParse(tb?.Text, out int tbVal);
            if (tbVal == _result)
            {
                _generateNumber = true;
            }
        }

        private void ChangeText(Label sender, string data) =>
            sender.Dispatcher.BeginInvoke((Action) (() => sender.Content = data));

        
        private void GameMethods(int state)
        {
            switch (state)
            {
                case 1:
                    _t = new Thread(CheckValues);
                    _t.IsBackground = true;
                    _generateNumber = _activeTimer = true;
                    _t.Start();
                    break; 
                case 2:
                    _generateNumber = _activeTimer = false;
                    _t.Abort();
                    break;
                
            }
        }

        private void CheckValues()
        {
            Start:
            for (int i = 30; i >= 0; i--)
            {
                if (_generateNumber)
                {
                    _generateNumber = false;
                    GenerateNums();
                    goto Start;    
                }
                
                if (_activeTimer)
                {
                    ChangeText(CounterLabel, $"Time: {i}");
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }   
            }
            
            _generateNumber = true;
            goto Start;
        }
    }
}