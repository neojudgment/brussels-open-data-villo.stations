// •————————————————————————————————————————————————————————————————————————————————————————————————————————————•
// |                                                                                                            |
// |    brussels open data - villo.stations is a proof of concept (PoC). <https://github.com/neojudgment/>      |
// |                                                                                                            |
// |    villo.stations shows bike stations in territory of the Brussels-Capital Region in Belgium.              |
// |                                                                                                            |
// |    brussels open data - villo.stations uses Microsoft WindowsAPICodePack and GMap.NET to                   |
// |    demonstrate how to retrieve data from Brussels open data Store.                                         |
// |                                                                                                            |
// |    brussels open data - villo.stations uses Elysium library that implements Modern UI for                  |
// |    Windows Presentation Foundation.                                                                        |
// |                                                                                                            |
// |    This program is under Microsoft Public License (Ms-RL)                                                  |
// |                                                                                                            |
// |    This program is distributed in the hope that it will be useful but WITHOUT ANY WARRANTY;                |
// |    without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.               |
// |                                                                                                            |
// |    You should have received a copy of the Microsoft Public License (Ms-RL)                                 |
// |    along with this program.  If not, see <http://opensource.org/licenses/MS-RL>.                           |
// |                                                                                                            |
// |    Copyright © Pascal Hubert - Brussels, Belgium 2017. <mailto:pascal.hubert@outlook.com>                  |
// •————————————————————————————————————————————————————————————————————————————————————————————————————————————•

namespace OpenData
{
    public class Json2Csharp
    {
        public class Position
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class RootObject
        {
            public int number { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public Position position { get; set; }
            public bool banking { get; set; }
            public bool bonus { get; set; }
            public string status { get; set; }
            public string contract_name { get; set; }
            public int bike_stands { get; set; }
            public int available_bike_stands { get; set; }
            public int available_bikes { get; set; }
            public object last_update { get; set; }
        }
    }
}