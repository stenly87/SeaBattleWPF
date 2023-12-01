using SeaBattleRepository.DTO;
using SeaBattleWPF.API;
using SeaBattleWPF.API.Game;
using SeaBattleWPF.mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SeaBattleWPF.VM
{
    public class GameVM : BaseVM
    {
        private string message;

        public string Message
        {
            get => message;
            set
            {
                message = value;
                Signal();
            }
        }
        
        Thread threadApi;
        public GameVM()
        {
            OnPageClose += GameVM_OnPageClose;
            if (Game.CreatorIsCurrentUser)
                Game.SetState(States.WaitJoin);
            else
                Game.SetState(States.WaitTurn);
            threadApi = new Thread(ApiDDOS);
            threadApi.Start();
        }

        bool timerRunning = true;
        private void GameVM_OnPageClose(object? sender, EventArgs e)
        {
            OnPageClose -= GameVM_OnPageClose;
            timerRunning = false;
        }

        private void ApiDDOS(object? obj)
        {
            while (timerRunning)
            {
                Thread.Sleep(1000);
                TestTurn();
            }
        }

        private void TestTurn()
        {
            Task.Run(async () =>
            {
                var turnInfo = await Client.Instance.PostMessageAsync<GameTurn>("Game/IsMyTurn?idGame=" + Game.CurrentGame.Id);
                if (turnInfo.Item1 == System.Net.HttpStatusCode.OK)
                    dispatcher.Invoke(() =>
                        Game.TestTurn(turnInfo.Item2));
            });
        }


        internal void ClickField(Canvas fieldUser, MouseButtonEventArgs e)
        {
            Game.ClickField(fieldUser, e);
        }

        internal void RegisterField(Canvas fieldUser, bool currentUser)
        {
            Game.RegisterField(fieldUser, currentUser);
        }
        Dispatcher dispatcher;
        internal void RegisterDispatcher(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;


        }
    }
}
