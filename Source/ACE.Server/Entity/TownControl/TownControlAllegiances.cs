using ACE.Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Entity.TownControl
{
    public static class TownControlAllegiances
    {
        private static List<int> _tcAllegianceList;
        private static string _tcAllegianceListString = string.Empty;

        public static List<int> AllowedAllegianceList
        {
            get
            {
                try
                {
                    var allegWhitelistString = PropertyManager.GetString("town_control_alleglist").Item ?? string.Empty;

                    if (_tcAllegianceList == null || !allegWhitelistString.Equals(_tcAllegianceListString))
                    {
                        _tcAllegianceList = new List<int>();
                        var allegWhitelistArray = allegWhitelistString.Split(",");

                        foreach (var allowedMonarchIdString in allegWhitelistArray)
                        {
                            if (Int32.TryParse(allowedMonarchIdString, out int allowedMonarchID))
                            {
                                _tcAllegianceList.Add(allowedMonarchID);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    return new List<int>();
                }

                return _tcAllegianceList;
            }
        }

        public static bool IsAllowedAllegiance(int monarchID)
        {
            return AllowedAllegianceList.Contains(monarchID);
        }
    }
}
