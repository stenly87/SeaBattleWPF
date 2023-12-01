using SeaBattleRepository.DTO;
using SeaBattleWPF.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SeaBattleWPF.API.Game
{
    static internal class Game
    {
        internal static GameState currentState;
        internal static GameDTO CurrentGame;
        internal static bool CreatorIsCurrentUser;

        static Dictionary<States, GameState> states;

        static Game()
        {
            states = new()
            {
                { States.WaitJoin, new GameStateWaitOpponentToJoin() },
                { States.WaitTurn, new GameStateWaitOpponentTurn() },
                { States.MyTurn, new GameStateMyTurn() }
            };
        }

        internal static GameState GetState(States key)
        {
            return states[key];
        }

        static internal void RegisterField(Canvas fieldUser, bool currentUser)
        { 
            foreach (var state in states.Values)
                state.RegisterField(fieldUser, currentUser);

            RedrawMyField(fieldUser, new byte[100]);
        }

        static public void RedrawMyField(Canvas fieldUserCanvas, byte[] fieldUser)
        {
            fieldUserCanvas.Children.Clear();
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    var index = i + j * 10;
                    Rectangle box = new Rectangle();
                    box.Width = 30;
                    box.Height = 30;
                    box.Fill = fieldUser[index] == 0 ?
                         Brushes.Blue :
                            fieldUser[index] == 2 ?
                            Brushes.Red :
                            Brushes.Brown;
                    Canvas.SetLeft(box, i * 30);
                    Canvas.SetTop(box, j * 30);
                    fieldUserCanvas.Children.Add(box);
                }
        }
    }
}
