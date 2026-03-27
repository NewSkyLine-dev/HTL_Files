using Gomoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Controller;

public class PlayerVsAiController(in GameField model) : GameController(model)
{
    public override void HandleClick(Field field)
    {
        throw new NotImplementedException();
    }
}
