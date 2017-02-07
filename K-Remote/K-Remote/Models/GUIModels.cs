using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{
    class GuiPropertiesResponse
    {
        public string id;
        public string jsonrpc;
        public GuiPropertiesResponseResult result;

    }

    class GuiPropertiesResponseResult
    {
        public GuiCurrentControl currentcontrol;
        public GuiCurrentWindow currentwindow;
        public bool fullscreen;
        public GuiSkin skin;
    }

    class GuiCurrentControl
    {
        public string label;
    }

    class GuiCurrentWindow
    {
        public int id;
        public string label;
    }

    class GuiSkin
    {
        public string id;
        public string name;
    }
}
