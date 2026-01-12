using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Drei_Raucher
{
    public partial class MainWindow : Window
    {
        public Agent Agent { get; set; }
        public Smoker SmokerA { get; private set; }
        public Smoker SmokerB { get; private set; }
        public Smoker SmokerC { get; private set; }
        private Thread _agentThread, _smokerAThread, _smokerBThread, _smokerCThread;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            InitializeSimulation();
        }

        private void InitializeSimulation()
        {
            Agent = new Agent();
            SmokerA = new Smoker(Ingredients.Tobacco, Agent);
            SmokerB = new Smoker(Ingredients.CigarettePaper, Agent);
            SmokerC = new Smoker(Ingredients.Match, Agent);

            DataContext = this;
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            _agentThread = new Thread(Agent.Run);
            _smokerAThread = new Thread(SmokerA.TryToSmoke);
            _smokerBThread = new Thread(SmokerB.TryToSmoke);
            _smokerCThread = new Thread(SmokerC.TryToSmoke);

            _agentThread.Start();
            _smokerAThread.Start();
            _smokerBThread.Start();
            _smokerCThread.Start();
        }
    }
}
