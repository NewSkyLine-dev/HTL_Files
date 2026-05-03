using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotersteuerung
{
    public class Context
    {
        public RobotField Field { get; }
        public Context(RobotField field) => Field = field;
    }
}
