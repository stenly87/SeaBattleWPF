using SeaBattleRepository.DTO;
using SeaBattleWPF.API;
using SeaBattleWPF.API.Game;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SeaBattleWPF.VM
{
    internal class GameStateMyTurn : GameState
    {
        public override void ClickField(Canvas fieldUser, MouseButtonEventArgs e)
        {
            if (fieldUser == fieldUser2)
            {
                var position = e.GetPosition(fieldUser);
                int x = (int)Math.Round(position.X) / 30;
                int y = (int)Math.Round(position.Y) / 30;
                Task.Run( async () =>
                {
                    var result = await Client.Instance.
                    PostMessagePlainAsync($"Game/MakeTurn?idGame={Game.CurrentGame.Id}&x={x}&y={y}");
                    fieldUser.Dispatcher.Invoke(() =>
                    {
                        if (result.Item1 == System.Net.HttpStatusCode.OK)
                        {
                            int.TryParse(result.Item2, out int hitResult);
                            TurnResult hit = (TurnResult)hitResult;
                            // TODO
                            Rectangle box = new Rectangle();
                            box.Width = 30;
                            box.Height = 30;
                            Canvas.SetLeft(box, x * 30);
                            Canvas.SetTop(box, y * 30);
                            if (hit == TurnResult.Hit)
                            {
                                box.Fill = Brushes.Red;
                            }
                            else if (hit == TurnResult.Lose)
                            {
                                box.Fill = Brushes.Black;
                                Game.SetState(States.WaitTurn);
                            }
                            else
                            {
                                MessageBox.Show("Победа");
                            }
                            fieldUser.Children.Add(box);
                        }
                    });
                }
                );
                
            }


        }
    }
}