using System.Collections.Generic;

namespace IdentityCustomized.Models
{
    public class ControllerData
    {
        public string ControllerName { get; set; }
        public string ControllerNameLocalized { get; set; }
        public string ControllerNamespace { get; set; }
        public bool RequiresAuthorization { get; set; }
        public List<ActionData> ActionsList { get; set; }
        public ControllerData()
        {
            ActionsList = new List<ActionData>();
        }
    }
}