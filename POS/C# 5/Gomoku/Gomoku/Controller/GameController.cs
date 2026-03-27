using Gomoku.Models;

namespace Gomoku.Controller;

public abstract class GameController(in GameField model)
{
    protected readonly GameField _model = model;

    public abstract void HandleClick(Field field);
    public abstract bool CheckWin(Field field);
    public abstract bool CheckWin();
}
