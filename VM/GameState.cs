﻿using SeaBattleRepository.DTO;
using SeaBattleWPF.API.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeaBattleWPF.VM
{
    internal abstract class GameState
    {
        protected Canvas fieldUser1;
        protected Canvas fieldUser2;

        public abstract void ClickField(Canvas fieldUser, MouseButtonEventArgs e);

        public void RegisterField(Canvas fieldUser, bool currentUser)
        {
            if (currentUser)
                fieldUser1 = fieldUser;
            else
                fieldUser2 = fieldUser;
        }

        public GameState TestTurn(GameTurn item2)
        {
            if (item2.FieldUser.Count(s => s != 0) > 0)
            {
                Game.RedrawMyField(fieldUser1, item2.FieldUser);
                return Game.GetState(States.MyTurn);
            }
            return Game.GetState(States.WaitTurn);
        }
    }
}
