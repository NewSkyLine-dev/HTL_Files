using Gomoku.Models;
using System.Windows;

namespace Gomoku.Controller;

public class HotSeatController(in GameField model) : GameController(model)
{
    public override void HandleClick(Field field)
    {
        if (this._model.CurrentPlayer == null)
        {
            MessageBox.Show("Game did not start yet!");
        }
    }
}
