﻿// •————————————————————————————————————————————————————————————————————————————————————————————————————————————•
// |                                                                                                            |
// |    brussels open data - villo.stations is a proof of concept (PoC). <https://github.com/neojudgment/>      |
// |                                                                                                            |
// |    villo.stations is a free Wi-Fi network covering certain zones of the territory of                       |
// |    the territory of the Brussels-Capital Region in Belgium.                                                |
// |                                                                                                            |
// |    brussels open data - villo.stations uses Microsoft WindowsAPICodePack and GMap.NET to                   |
// |    demonstrate how to retrieve data from Brussels open data Store.                                         |
// |                                                                                                            |
// |    brussels open data - villo.stations uses Elysium library that implements Modern UI for                  |
// |    Windows Presentation Foundation.                                                                        |
// |                                                                                                            |
// |    This program is under Microsoft Reciprocal License (Ms-RL)                                              |
// |                                                                                                            |
// |    This program is distributed in the hope that it will be useful but WITHOUT ANY WARRANTY;                |
// |    without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.               |
// |                                                                                                            |
// |    You should have received a copy of the Microsoft Reciprocal License (Ms-RL)                             |
// |    along with this program.  If not, see <http://opensource.org/licenses/MS-RL>.                           |
// |                                                                                                            |
// |    Copyright © Pascal Hubert - Brussels, Belgium 2022. <mailto:pascal.hubert@outlook.com>                  |
// •————————————————————————————————————————————————————————————————————————————————————————————————————————————•

using System.Threading;
using System.Windows;

namespace OpenData
{
    public partial class Application
    {
        private Mutex _mutex;

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            _mutex.Close();
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            _mutex = new Mutex(false, "villo.stations_MUTEX");
            if (!_mutex.WaitOne(0, false))
            {
                _mutex.Close();
                System.Environment.Exit(1);
            }
        }

        public Application()
        {
            SubscribeToEvents();
        }

        private bool EventsSubscribed = false;

        private void SubscribeToEvents()
        {
            if (EventsSubscribed)
                return;
            else
                EventsSubscribed = true;

            @Exit += ApplicationExit;
            Startup += ApplicationStartup;
        }
    }
}